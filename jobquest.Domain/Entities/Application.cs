using MongoDB.Entities;

namespace jobquest.Domain.Entities;

[Collection("applications")]
public class Application : Entity, ICreatedOn, IModifiedOn
{
    [Field("jobpost_id")]
    public One<JobPost> JobPost { get; set; }
    [Field("user_id")]
    public One<User> User { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
}