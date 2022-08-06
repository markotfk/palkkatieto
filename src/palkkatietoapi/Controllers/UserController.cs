using Microsoft.AspNetCore.Mvc;
using System.Net;
using palkkatietoapi.Model;
using palkkatietoapi.Services;

namespace palkkatietoapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> logger;
    private readonly IUserService userService;

    public UsersController(ILogger<UsersController> logger, IUserService userService) 
    {
        this.logger = logger;
        this.userService = userService;
    }

    [HttpGet]
    [Route("getById/{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var ret = await userService.GetById(id, cancellationToken);
        if (ret == null) return NotFound();
        var result = new JsonResult(ret);
        result.StatusCode = (int)HttpStatusCode.OK;
        return result;
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(User user, CancellationToken cancellationToken) 
    {
        await userService.Add(user, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    [Route("remove/{id:long}")]
    public async Task<IActionResult> Remove(long id, CancellationToken cancellationToken) 
    {
        await userService.Remove(id, cancellationToken);
        return Ok();
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(User user, CancellationToken cancellationToken)
    {
        await userService.Update(user, cancellationToken);
        return Ok();
    }

}