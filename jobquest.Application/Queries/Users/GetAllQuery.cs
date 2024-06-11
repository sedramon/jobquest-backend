using Amazon.Runtime.Internal;
using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Users;


public record GetAllQuery : IRequest<List<UserDto>>;

public class GetAllHandler : IRequestHandler<GetAllQuery, List<UserDto>>
{
    private readonly IMapper _mapper;

    public GetAllHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<UserDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<UserDto>>(await DB.Fluent<User>().ToListAsync(cancellationToken: cancellationToken));
    }

}