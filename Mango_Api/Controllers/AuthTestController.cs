using Mango_Api.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthTestController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public ActionResult<string> GetAuthorized()
    {
        return Ok("Auth Test Successful");
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public ActionResult<string> GetSomething(int id)
    {
        return Ok("You have got some data with role of admin id: " + id);
    }

}
