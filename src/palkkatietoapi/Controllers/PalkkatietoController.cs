using Microsoft.AspNetCore.Mvc;
using System.Net;
using palkkatietoapi.Model;

namespace palkkatietoapi.Controllers;

[ApiController]
[Route("[controller]")]
public class PalkkatietoController : ControllerBase
{
    readonly IPalkkatietoService palkkatietoService;
    private readonly ILogger<PalkkatietoController> logger;

    public PalkkatietoController(ILogger<PalkkatietoController> logger, IPalkkatietoService palkkatietoService)
    {
        this.logger = logger;
        this.palkkatietoService = palkkatietoService;
    }

    [HttpPost]
    [Route("getByQuery")]
    public async Task<IActionResult> GetByQuery([FromBody] PalkkaQuery query, CancellationToken cancellationToken)
    {
        var result = new JsonResult(await palkkatietoService.GetByQuery(query, cancellationToken));
        result.StatusCode = (int)HttpStatusCode.OK;
        return result;

    }

    [HttpGet]
    [Route("getById/{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var ret = await palkkatietoService.GetById(id, cancellationToken);
        if (ret == null) return NotFound();
        var result = new JsonResult(ret);
        result.StatusCode = (int)HttpStatusCode.OK;
        return result;
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddPalkka(Palkka palkka, CancellationToken cancellationToken) 
    {
        await palkkatietoService.Add(palkka, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    [Route("remove/{id:long}")]
    public async Task<IActionResult> RemovePalkka(long id, CancellationToken cancellationToken) 
    {
        await palkkatietoService.Remove(id, cancellationToken);
        return Ok();
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(Palkka palkka, CancellationToken cancellationToken)
    {
        await palkkatietoService.Update(palkka, cancellationToken);
        return Ok();
    }
}
