using MongoDB.Entities;

namespace jobquest.Domain.Entities;

[Collection("job_posts")]
public class JobPost : Entity, ICreatedOn, IModifiedOn
{
    [Field("title")]
    public string Title { get; set; }
    [Field("company_id")]
    public One<Company> Company { get; set; }
    [Field("description")]
    public string Description { get; set; }
    [Field("fieldOfWork")]
    public string FieldOfWork { get; set; }
    [Field("location")]
    public string Location { get; set; }
    [Field("endsAt")]
    public DateTime EndsAt { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
    
}