using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mango_Api.Models;
using Mango_Api.Models.Dto;
using Mango_Api.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Mango_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApiResponse _response;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly string secretKey;

    public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _response = new ApiResponse();
        secretKey = configuration.GetValue<string>("ApiSettings:Secret") ?? string.Empty;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Models.Dto.RegisterRequestDto registerRequestDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new()
                {
                    Email = registerRequestDto.Email,
                    UserName = registerRequestDto.Email,
                    Name = registerRequestDto.Name,
                    NormalizedEmail = registerRequestDto.Email.ToUpper()
                };

                var result = await _userManager.CreateAsync(newUser, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer));
                    }

                    if (registerRequestDto.Role.Equals(StaticDetails.Role_Admin, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_Customer);
                    }

                    _response.StatusCode = System.Net.HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
                else
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    foreach (var error in result.Errors)
                    {
                        _response.ErrorMessages.Add(error.Description);
                    }
                    return BadRequest(_response);
                }
            }
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.IsSuccess = false;

            return BadRequest(_response);

        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(ex.Message);
            return StatusCode(500, _response);
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Models.Dto.LoginRequestDto loginRequestDto)
    {
        try
        {
            if (ModelState.IsValid)
            {

                var userFromDb = await _userManager.FindByEmailAsync(loginRequestDto.Email);

                if (userFromDb != null)
                {
                    bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDto.Password);
                    if (!isValid)
                    {
                        _response.Result = new LoginResponseDto();
                        _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Invalid Password");
                        return BadRequest(_response);
                    }

                    JwtSecurityTokenHandler tokenHandler = new();
                    byte[] key = System.Text.Encoding.ASCII.GetBytes(secretKey);

                    SecurityTokenDescriptor tokenDescriptor = new()
                    {
                        Subject = new ClaimsIdentity(
                        [
                            new (ClaimTypes.Name, userFromDb.Id.ToString()),
                            new (ClaimTypes.Email, userFromDb.Email!),
                            new (ClaimTypes.Role, _userManager.GetRolesAsync(userFromDb).Result.FirstOrDefault()!)
                        ]),
                        Expires = DateTime.UtcNow.AddDays(5),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                    LoginResponseDto loginResponseDto = new()
                    {
                        Email = userFromDb.Email!,
                        Token = tokenHandler.WriteToken(token),
                        Role = _userManager.GetRolesAsync(userFromDb).Result.FirstOrDefault()!
                    };

                    _response.Result = loginResponseDto;
                    _response.StatusCode = System.Net.HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
                _response.Result = new LoginResponseDto();
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid Password");
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.IsSuccess = false;

            return BadRequest(_response);

        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(ex.Message);
            return StatusCode(500, _response);
        }
    }

}
