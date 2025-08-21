using AutoMapper;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Services;

internal sealed class CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    : ICompanyService
{
    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = repository.Company.GetallCompanies(trackChanges);
            var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}