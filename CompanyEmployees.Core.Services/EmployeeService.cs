using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;

namespace CompanyEmployees.Core.Services;

internal sealed class EmployeeService(IRepositoryManager repository, ILoggerManager logger) : IEmployeeService
{
    private readonly ILoggerManager _logger = logger;
    private readonly IRepositoryManager _repository = repository;
}