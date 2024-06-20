using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Companies;

public record GetAllQuery : IRequest<List<CompanyDto>>;

public class GetAllHandler : IRequestHandler<GetAllQuery, List<CompanyDto>>
{
    private readonly IMapper _mapper;

    public GetAllHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<CompanyDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<CompanyDto>>(await DB.Fluent<Company>().ToListAsync(cancellationToken: cancellationToken));
    }

}