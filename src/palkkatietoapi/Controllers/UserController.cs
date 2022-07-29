using Microsoft.AspNetCore.Mvc;
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
        return new JsonResult(await userService.GetById(id, cancellationToken));
    }

    [HttpPost]
    [Route("add")]
    public async Task Add(User user, CancellationToken cancellationToken) 
    {
        await userService.Add(user, cancellationToken);

    }

    [HttpDelete]
    [Route("remove")]
    public async Task Remove(long id, CancellationToken cancellationToken) 
    {
        await userService.Remove(id, cancellationToken);
    }

    [HttpPut]
    [Route("update")]
    public async Task Update(User user, CancellationToken cancellationToken)
    {
        await userService.Update(user, cancellationToken);
    }

}