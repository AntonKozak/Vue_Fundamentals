using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mango_Api.Data;
using Mango_Api.Models;
using Mango_Api.Models.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // added

namespace Mango_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuItemController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ApiResponse _response;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public MenuItemController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _response = new ApiResponse();
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult GetMenuItems()
    {
        _response.Result = _db.MenuItems.ToList();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetMenuItem")]
    public IActionResult GetMenuItem(int id)
    {
        if (id == 0)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Invalid Id");
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        var menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == id);
        if (menuItem == null)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Menu Item Not Found");
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }
        _response.Result = menuItem;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }


    [HttpPost]
    public async Task<IActionResult> CreateMenuItem([FromForm] MenuItemCreateDto menuItemCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (menuItemCreateDto.File == null || menuItemCreateDto.File.Length == 0)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Image file is required");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var webRoot = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var imagesPath = Path.Combine(webRoot, "images");
            Directory.CreateDirectory(imagesPath);

            var originalExt = Path.GetExtension(menuItemCreateDto.File.FileName);
            var baseName = Path.GetFileNameWithoutExtension(menuItemCreateDto.File.FileName);

            // slugify: letters, numbers, - and _
            baseName = Regex.Replace(baseName, @"[^a-zA-Z0-9-_]+", "-").Trim('-');

            var fileName = $"{baseName}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{originalExt}";
            var filePath = Path.Combine(imagesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await menuItemCreateDto.File.CopyToAsync(stream);
            }

            var menuItem = new MenuItem
            {
                Name = menuItemCreateDto.Name,
                Description = menuItemCreateDto.Description,
                Price = menuItemCreateDto.Price,
                Category = menuItemCreateDto.Category,
                SpecialTag = menuItemCreateDto.SpecialTag,
                Image = $"images/{fileName}"
            };

            _db.MenuItems.Add(menuItem);
            await _db.SaveChangesAsync();

            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            return CreatedAtRoute("GetMenuItem", new { id = menuItem.Id }, _response);
        }
        catch (System.Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages = new List<string> { ex.Message };
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDto menuItemUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            // replaced invalid call
            MenuItem? menuItemFromDb = await _db.MenuItems.FirstOrDefaultAsync(u => u.Id == id);
            if (menuItemFromDb == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Menu Item Not Found");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }


            menuItemFromDb.Name = menuItemUpdateDto.Name;
            menuItemFromDb.Description = menuItemUpdateDto.Description;
            menuItemFromDb.Price = menuItemUpdateDto.Price;
            menuItemFromDb.Category = menuItemUpdateDto.Category;
            menuItemFromDb.SpecialTag = menuItemUpdateDto.SpecialTag;

            if (menuItemUpdateDto.File != null && menuItemUpdateDto.File.Length > 0)
            {
                var webRoot = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var imagesPath = Path.Combine(webRoot, "images");
                Directory.CreateDirectory(imagesPath);

                var originalExt = Path.GetExtension(menuItemUpdateDto.File.FileName);
                var baseName = Path.GetFileNameWithoutExtension(menuItemUpdateDto.File.FileName);

                // slugify: letters, numbers, - and _
                baseName = Regex.Replace(baseName, @"[^a-zA-Z0-9-_]+", "-").Trim('-');

                var fileName = $"{baseName}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{originalExt}";
                var filePath = Path.Combine(imagesPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await menuItemUpdateDto.File.CopyToAsync(stream);
                }

                menuItemFromDb.Image = $"images/{fileName}";
            }
            _db.MenuItems.Update(menuItemFromDb);
            await _db.SaveChangesAsync();
            _response.Result = menuItemFromDb;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (System.Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages = new List<string> { ex.Message };
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        if (id == 0)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Invalid Id");
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        var menuItem = await _db.MenuItems.FirstOrDefaultAsync(u => u.Id == id);
        if (menuItem == null)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Menu Item Not Found");
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }

        // delete image file if present
        try
        {
            var webRoot = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!string.IsNullOrWhiteSpace(menuItem.Image))
            {
                var relative = menuItem.Image.TrimStart('\\', '/');
                var fullPath = Path.Combine(webRoot, relative);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
        catch
        {
            // ignore file delete failures
        }

        _db.MenuItems.Remove(menuItem);
        await _db.SaveChangesAsync();

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}
