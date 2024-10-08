using Amazon.Runtime.Internal;
using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Users;


public record GetAllQuery : IRequest<List<UserDisplayDto>>;

public class GetAllHandler : IRequestHandler<GetAllQuery, List<UserDisplayDto>>
{
    private readonly IMapper _mapper;

    public GetAllHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<UserDisplayDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<UserDisplayDto>>(await DB.Fluent<User>().ToListAsync(cancellationToken: cancellationToken));
    }
}