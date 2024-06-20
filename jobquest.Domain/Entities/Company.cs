using MongoDB.Entities;

namespace jobquest.Domain.Entities;

[Collection("companies")]
public class Company : Entity, ICreatedOn, IModifiedOn
{
    [Field("companyName")]
    public string CompanyName { get; set; }
    [Field("companyAddress")]
    public string CompanyAddress { get; set; }
    [Field("companyPhone")]
    public string CompanyPhone { get; set; }
    [Field("activity")]
    public string Activity { get; set; }
    [Field("mb")]
    public string MB { get; set; }
    [Field("pib")]
    public string PIB { get; set; }
    [Field("email")]
    public string Email { get; set; }
    [Field("password")]
    public string Password { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
}