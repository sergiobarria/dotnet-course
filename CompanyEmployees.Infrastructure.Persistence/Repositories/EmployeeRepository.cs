using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace CompanyEmployees.Infrastructure.Persistence.Repositories;

public class EmployeeRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Employee>(repositoryContext), IEmployeeRepository
{
    private readonly RepositoryContext _repositoryContext = repositoryContext;

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters,
        bool trackChanges,
        CancellationToken ct = default)
    {
        var page = Math.Max(1, employeeParameters.PageNumber);
        var size = Math.Max(1, employeeParameters.PageSize);

        var baseQuery = FindByCondition(e => e.CompanyId == companyId, trackChanges).OrderBy(e => e.Name);

        var count = await baseQuery.CountAsync(ct);

        var items = await baseQuery.Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(ct);

        return PagedList<Employee>
            .ToPagedList(items, count, employeeParameters.PageNumber, employeeParameters.PageSize);
    }

    public async Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges,
        CancellationToken ct = default)
    {
        return await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync(ct)!;
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Company company, Employee employee)
    {
        using var transaction = _repositoryContext.Database.BeginTransaction();

        Delete(employee);

        _repositoryContext.SaveChanges();

        if (!FindByCondition(e => e.CompanyId == company.Id, false).Any())
        {
            _repositoryContext.Companies!.Remove(company);
            _repositoryContext.SaveChanges();
        }

        transaction.Commit();
    }
}