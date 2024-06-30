using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Companies;

public record GetCompanyByEmailAndPasswordQuery(string email, string password) : IRequest<CompanyDto>;

public class GetCompanyByEmailAndPasswordHandler : IRequestHandler<GetCompanyByEmailAndPasswordQuery, CompanyDto>
{
    private readonly IMapper _mapper;
    
    public GetCompanyByEmailAndPasswordHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    
    public async Task<CompanyDto> Handle(GetCompanyByEmailAndPasswordQuery request, CancellationToken cancellationToken)
    {
        var company = await DB.Find<Company>().Match(c => c.Email == request.email && c.Password == request.password).ExecuteSingleAsync(cancellationToken);
        
        if(company == null)
        {
            throw new CompanyNotFoundException(request.email);
        }

        return _mapper.Map<CompanyDto>(company);
    }
}