using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Services;

internal sealed class CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    : ICompanyService
{
    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges, CancellationToken ct = default)
    {
        var companies = await repository.Company.GetAllCompaniesAsync(trackChanges, ct);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges, ct);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var companyDto = mapper.Map<CompanyDto>(company);

        return companyDto;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges,
        CancellationToken ct = default)
    {
        if (ids is null) throw new IdParametersBadRequestException();

        var companyEntities = await repository.Company.GetByIdsAsync(ids, trackChanges, ct);
        if (ids.Count() != companyEntities.Count()) throw new CollectionByIdsBadRequestException();

        var companiesToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

        return companiesToReturn;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company, CancellationToken ct = default)
    {
        var companyEntity = mapper.Map<Company>(company);

        repository.Company.CreateCompany(companyEntity);
        await repository.SaveAsync(ct);

        var companyToReturn = mapper.Map<CompanyDto>(companyEntity);

        return companyToReturn;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(
        IEnumerable<CompanyForCreationDto> companyCollection, CancellationToken ct = default)
    {
        if (companyCollection is null) throw new CompanyCollectionBadRequest();

        var companyEntities = mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities) repository.Company.CreateCompany(company);
        await repository.SaveAsync(ct);

        var companyCollectionToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        return (companies: companyCollectionToReturn, ids);
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges,
        CancellationToken ct = default)
    {
        var companyEntity = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (companyEntity is null) throw new CompanyNotFoundException(companyId);

        mapper.Map(companyForUpdate, companyEntity);
        await repository.SaveAsync(ct);
    }

    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges, ct);
        if (company is null) throw new CompanyNotFoundException(companyId);

        repository.Company.DeleteCompany(company);
        await repository.SaveAsync(ct);
    }
}