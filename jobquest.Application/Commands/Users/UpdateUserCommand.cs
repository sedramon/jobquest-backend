using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;

namespace jobquest.Application.Commands.Users;

public record UpdateUserCommand(UserDto UserDto) : IRequest<UserDto>;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IMapper _mapper;

    public UpdateUserHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User();
        user.ID = request.UserDto.ID;
        user.FirstName = request.UserDto.FirstName;
        user.LastName = request.UserDto.LastName;
        user.Email = request.UserDto.Email;
        user.Phone = request.UserDto.Phone;
        user.Password = request.UserDto.Password;

        await user.SaveAsync(cancellation: cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}