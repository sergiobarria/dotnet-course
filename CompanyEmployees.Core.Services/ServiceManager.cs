using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;

namespace CompanyEmployees.Core.Services;

public sealed class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
    : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService = new(() => new CompanyService(repositoryManager, logger));

    private readonly Lazy<IEmployeeService>
        _employeeService = new(() => new EmployeeService(repositoryManager, logger));

    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}