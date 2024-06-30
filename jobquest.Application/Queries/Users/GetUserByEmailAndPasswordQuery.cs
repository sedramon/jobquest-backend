using AutoMapper;
using jobquest.Application.Commands.Authentication;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Users;

public record GetUserByEmailAndPasswordQuery(string Email, string Password) : IRequest<UserDtoWithToken>;

public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailAndPasswordQuery, UserDtoWithToken>
{
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public GetUserByEmailHandler(IMapper mapper, ITokenService tokenService)
    {
        _mapper = mapper;
        _tokenService = tokenService;
    }


    public async Task<UserDtoWithToken> Handle(GetUserByEmailAndPasswordQuery request, CancellationToken cancellationToken)
    {
        var user = await DB.Find<User>().Match(u => u.Email == request.Email && u.Password == request.Password).ExecuteSingleAsync(cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var token = _tokenService.GenerateToken(user.Email);
        var userDto = _mapper.Map<UserDto>(user);

        return new UserDtoWithToken(userDto, token);
    }
}