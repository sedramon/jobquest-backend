using MongoDB.Entities;

namespace jobquest.Domain.Entities;

[Collection("users")]
public class User : Entity, ICreatedOn, IModifiedOn
{
    [Field("username")]
    public string Username { get; set; }
    [Field("email")]
    public string Email { get; set; }
    [Field("password")]
    public string Password { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
}