using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;

namespace CompanyEmployees.Core.Services;

internal sealed class CompanyService(IRepositoryManager repository, ILoggerManager logger) : ICompanyService
{
    private readonly ILoggerManager _logger = logger;
    private readonly IRepositoryManager _repository = repository;
}