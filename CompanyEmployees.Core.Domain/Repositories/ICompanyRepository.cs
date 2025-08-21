using CompanyEmployees.Core.Domain.Entities;

namespace CompanyEmployees.Core.Domain.Repositories;

public interface ICompanyRepository
{
    IEnumerable<Company> GetallCompanies(bool trackChanges);
}