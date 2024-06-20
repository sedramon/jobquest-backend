using MongoDB.Entities;

namespace jobquest.Domain.Entities;

[Collection("users")]
public class User : Entity, ICreatedOn, IModifiedOn
{
    [Field("firstName")]
    public string FirstName { get; set; }
    [Field("lastName")]
    public string LastName { get; set; }
    [Field("phone")]
    public string Phone { get; set; }
    [Field("email")]
    public string Email { get; set; }
    [Field("password")]
    public string Password { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
}