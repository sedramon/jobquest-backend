using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Users;

public record GetUserByEmailQuery(string Email) : IRequest<UserDto>;

public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
{
    private readonly IMapper _mapper;

    public GetUserByEmailHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await DB.Find<User>().Match(u => u.Email == request.Email).ExecuteSingleAsync(cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.Email);
        }

        return _mapper.Map<UserDto>(user);
    }
}