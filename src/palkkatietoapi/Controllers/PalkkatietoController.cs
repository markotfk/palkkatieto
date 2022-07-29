using Microsoft.AspNetCore.Mvc;
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
        return new JsonResult(await palkkatietoService.GetByQuery(query, cancellationToken));
    }

    [HttpGet]
    [Route("getById/{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        return new JsonResult(await palkkatietoService.GetById(id, cancellationToken));
    }

    [HttpPost]
    [Route("add")]
    public async Task AddPalkka(Palkka palkka, CancellationToken cancellationToken) 
    {
        await palkkatietoService.Add(palkka, cancellationToken);

    }

    [HttpDelete]
    [Route("remove")]
    public async Task RemovePalkka(long palkkaId, CancellationToken cancellationToken) 
    {
        await palkkatietoService.Remove(palkkaId, cancellationToken);
    }

    [HttpPut]
    [Route("update")]
    public async Task Update(Palkka palkka, CancellationToken cancellationToken)
    {
        await palkkatietoService.Update(palkka, cancellationToken);
    }
}
