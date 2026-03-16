namespace Insurance.Application.DTOs;

/// <summary>
/// Dados retornados pelo serviço externo de consulta de segurados
/// </summary>
public class ExternalInsuredData
{
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public int Age { get; set; }
}
