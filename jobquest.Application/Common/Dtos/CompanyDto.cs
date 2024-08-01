namespace jobquest.Application.Common.Dtos;

public record CompanyDto
{
    public string? ID { get; init; }
    public string? CompanyName { get; init; }
    public string? CompanyAddress { get; init; }
    public string? CompanyPhone { get; init; }
    public string? Activity { get; init; }
    public string? MB { get; init; }
    public string? PIB { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }

    public CompanyDto()
    {
        // Default values or null values for properties
    }

    public CompanyDto(
        string? id,
        string? companyName,
        string? companyAddress,
        string? companyPhone,
        string? activity,
        string? mb,
        string? pib,
        string? email,
        string? password
    )
    {
        ID = id;
        CompanyName = companyName;
        CompanyAddress = companyAddress;
        CompanyPhone = companyPhone;
        Activity = activity;
        MB = mb;
        PIB = pib;
        Email = email;
        Password = password;
    }
}