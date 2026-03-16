using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Insurance.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

/// <summary>
/// Controller para operações de seguro
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InsuranceController : ControllerBase
{
    private readonly IInsuranceApplicationService _insuranceService;
    private readonly ILogger<InsuranceController> _logger;

    public InsuranceController(
        IInsuranceApplicationService insuranceService,
        ILogger<InsuranceController> logger)
    {
        _insuranceService = insuranceService;
        _logger = logger;
    }

    /// <summary>
    /// Registra um novo seguro
    /// </summary>
    /// <param name="request">Dados do seguro a ser criado</param>
    /// <returns>Dados completos do seguro criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(InsuranceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InsuranceResponse>> CreateInsurance(
        [FromBody] CreateInsuranceRequest request)
    {
        try
        {
            // Validação manual (também pode usar FluentValidation middleware)
            var validator = new CreateInsuranceRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return ValidationProblem(ModelState);
            }

            _logger.LogInformation("Requisição para criar seguro recebida: CPF {Cpf}, Veículo {Model}, Valor {Value}",
                request.InsuredCpf, request.VehicleModel, request.VehicleValue);

            var result = await _insuranceService.CreateInsuranceAsync(request);

            _logger.LogInformation("Seguro criado com sucesso. ID: {Id}", result.Id);

            return CreatedAtAction(
                nameof(GetInsurance),
                new { id = result.Id },
                result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar seguro");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar seguro");
            return StatusCode(500, new { message = "Erro interno ao processar requisição" });
        }
    }

    /// <summary>
    /// Obtém os detalhes de um seguro por ID
    /// </summary>
    /// <param name="id">ID do seguro</param>
    /// <returns>Dados completos do seguro</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InsuranceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InsuranceResponse>> GetInsurance(int id)
    {
        try
        {
            _logger.LogInformation("Buscando seguro com ID: {Id}", id);

            var result = await _insuranceService.GetInsuranceByIdAsync(id);

            if (result == null)
            {
                _logger.LogWarning("Seguro com ID {Id} não encontrado", id);
                return NotFound(new { message = $"Seguro com ID {id} não encontrado" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar seguro {Id}", id);
            return StatusCode(500, new { message = "Erro interno ao processar requisição" });
        }
    }

    /// <summary>
    /// Obtém todas as apólices de seguro
    /// </summary>
    /// <returns>Lista de todas as apólices</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InsuranceResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InsuranceResponse>>> GetAllInsurances()
    {
        try
        {
            _logger.LogInformation("Listando todas as apólices de seguro");

            var result = await _insuranceService.GetAllInsurancesAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar apólices");
            return StatusCode(500, new { message = "Erro interno ao processar requisição" });
        }
    }

    /// <summary>
    /// Obtém relatório com médias de seguros
    /// </summary>
    /// <returns>Relatório com médias</returns>
    [HttpGet("report")]
    [ProducesResponseType(typeof(InsuranceReportResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<InsuranceReportResponse>> GetReport()
    {
        try
        {
            _logger.LogInformation("Gerando relatório de seguros");

            var result = await _insuranceService.GetReportAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório");
            return StatusCode(500, new { message = "Erro interno ao processar requisição" });
        }
    }
}
