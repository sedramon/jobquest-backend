namespace jobquest.Application.Common.Dtos;

public record JobPostDto
{
    public string? ID { get; init; }
    public string? Title { get; init; }
    public CompanyDto? Company { get; init; }
    public string? Description { get; init; }
    public string? FieldOfWork { get; init; }
    public string? Location { get; init; }
    public DateTime? EndsAt { get; init; }
    public DateTime? CreatedOn { get; init; }

    public JobPostDto()
    {
        // Default values or null values for properties
    }

    public JobPostDto(
        string? id,
        string? title,
        CompanyDto? company,
        string? description,
        string? fieldOfWork,
        string? location,
        DateTime? endsAt,
        DateTime? createdOn
    )
    {
        ID = id;
        Title = title;
        Company = company;
        Description = description;
        FieldOfWork = fieldOfWork;
        Location = location;
        EndsAt = endsAt;
        CreatedOn = createdOn;
    }
}