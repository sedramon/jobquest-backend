using Amazon.Runtime.Internal;
using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;

namespace jobquest.Application.Commands.Companies;

public record UpdateCompanyCommand(CompanyDto CompanyDto) : IRequest<CompanyDto>;

public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, CompanyDto>
{
    private readonly IMapper _mapper;

    public UpdateCompanyHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company();
        company.ID = request.CompanyDto.ID;
        company.CompanyName = request.CompanyDto.CompanyName;
        company.CompanyAddress = request.CompanyDto.CompanyAddress;
        company.CompanyPhone = request.CompanyDto.CompanyPhone;
        company.Activity = request.CompanyDto.Activity;
        company.MB = request.CompanyDto.MB;
        company.PIB = request.CompanyDto.PIB;
        company.Email = request.CompanyDto.Email;
        company.Password = request.CompanyDto.Password;

        await company.SaveAsync(cancellation: cancellationToken);

        return _mapper.Map<CompanyDto>(company);
    }
}