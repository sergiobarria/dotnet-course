using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence.Repositories;

internal sealed class CompanyRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges, CancellationToken ct = default)
    {
        return await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync(ct);
    }

    public void CreateCompany(Company company)
    {
        Create(company);
    }

    public void DeleteCompany(Company company)
    {
        Delete(company);
    }

    public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default)
    {
        return await FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync(ct)!;
    }

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges,
        CancellationToken ct = default)
    {
        return await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync(ct);
    }
}