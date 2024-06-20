using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Commands.JobPosts;

public record DeleteJobPostCommand(string JobPostId) : IRequest<Task>;

public class DeleteJobPostHandler : IRequestHandler<DeleteJobPostCommand, Task>
{
    public async Task<Task> Handle(DeleteJobPostCommand request, CancellationToken cancellationToken)
    {
        var article = await DB.Fluent<JobPost>().Match(article => article.ID == request.JobPostId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        await article.DeleteAsync(cancellation: cancellationToken);

        return Task.CompletedTask;
    } 
}