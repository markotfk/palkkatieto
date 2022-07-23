using Microsoft.AspNetCore.Mvc;
using palkkatietoapi.Model;

namespace palkkatietoapi.Controllers;

[ApiController]
[Route("[controller]")]
public class PalkkatietoController : ControllerBase
{
    
    private readonly ILogger<PalkkatietoController> _logger;

    public PalkkatietoController(ILogger<PalkkatietoController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Palkka> Get([FromBody] PalkkaQuery query)
    {
        return new List<Palkka>();
    }
}
