using AutoMapper;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;

namespace CompanyEmployees.Core.Services;

public sealed class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService =
        new(() => new CompanyService(repositoryManager, logger, mapper));

    private readonly Lazy<IEmployeeService>
        _employeeService = new(() => new EmployeeService(repositoryManager, logger, mapper));

    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}