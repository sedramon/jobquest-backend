using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;

namespace jobquest.Application.Commands.Companies;

public record CreateCompanyCommand(CompanyDto CompanyDto) : IRequest<CompanyDto>;

public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly IMapper _mapper;

    public CreateCompanyHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request.CompanyDto);
        await company.SaveAsync(cancellation: cancellationToken);
        return _mapper.Map<CompanyDto>(company);
    }
}