using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;

namespace jobquest.Application.Commands.Users;

public record CreateUserCommand(UserDto UserDto) : IRequest<UserDto>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IMapper _mapper;

    public CreateUserHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.UserDto);
        await user.SaveAsync(cancellation: cancellationToken);
        return _mapper.Map<UserDto>(user);
    }
}