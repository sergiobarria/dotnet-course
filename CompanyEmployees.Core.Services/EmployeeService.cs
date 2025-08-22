using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace CompanyEmployees.Core.Services;

internal sealed class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    : IEmployeeService
{
    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId,
        EmployeeParameters employeeParameters,
        bool trackChanges,
        CancellationToken ct = default)
    {
        if (!employeeParameters.ValidAgeRange) throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeesWithMetaData =
            await repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges, ct);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

        return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,
        EmployeeForCreationDto employeeForCreation,
        bool trackChanges, CancellationToken ct = default)
    {
        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeEntity = mapper.Map<Employee>(employeeForCreation);

        repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await repository.SaveAsync(ct);

        var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate,
        bool compTrackChanges,
        bool empTrackChanges, CancellationToken ct = default)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, compTrackChanges, ct);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = await repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges, ct);
        if (employee is null) throw new EmployeeNotFoundException(id);

        mapper.Map(employeeForUpdate, employee);
        await repository.SaveAsync(ct);
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges, CancellationToken ct = default)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges, ct);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, compTrackChanges, ct);

        var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity,
        CancellationToken ct = default)
    {
        mapper.Map(employeeToPatch, employeeEntity);
        await repository.SaveAsync(ct);
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges,
        CancellationToken ct = default)
    {
        var company = await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges, ct);

        repository.Employee.DeleteEmployee(company, employeeDb);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges,
        CancellationToken ct = default)
    {
        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeFromDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges, ct);

        var employee = mapper.Map<EmployeeDto>(employeeFromDb);

        return employee;
    }

    private async Task<Company> CheckIfCompanyExists(Guid companyId, bool trackChanges, CancellationToken ct)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges, ct);
        if (company is null) throw new CompanyNotFoundException(companyId);

        return company;
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId,
        Guid id, bool trackChanges, CancellationToken ct)
    {
        var employeeDb = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges, ct);
        if (employeeDb is null) throw new EmployeeNotFoundException(id);

        return employeeDb;
    }
}