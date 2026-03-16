using Insurance.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

/// <summary>
/// Controller que simula o serviço externo de consulta de segurados
/// Este é apenas um mock para demonstração
/// </summary>
[ApiController]
[Route("[controller]")]
public class ExternalController : ControllerBase
{
    private readonly IExternalInsuredService _externalService;
    private readonly ILogger<ExternalController> _logger;

    public ExternalController(
        IExternalInsuredService externalService,
        ILogger<ExternalController> logger)
    {
        _externalService = externalService;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint mock que simula o serviço externo de consulta de segurados
    /// GET /external/insured/{cpf}
    /// </summary>
    /// <param name="cpf">CPF do segurado</param>
    /// <returns>Dados do segurado</returns>
    [HttpGet("insured/{cpf}")]
    [ProducesResponseType(typeof(ExternalInsuredDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExternalInsuredDto>> GetInsuredByCpf(string cpf)
    {
        _logger.LogInformation("Mock: Consultando segurado com CPF: {Cpf}", cpf);

        var result = await _externalService.GetInsuredByCpfAsync(cpf);

        if (result == null)
        {
            return NotFound(new { message = $"Segurado com CPF {cpf} não encontrado" });
        }

        return Ok(result);
    }
}
