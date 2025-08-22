using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Services.Abstractions;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);

    Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);

    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);

    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto
    > companyCollection);

    Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);

    Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
}