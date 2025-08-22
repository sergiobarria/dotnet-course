using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Services;

internal sealed class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    : IEmployeeService
{
    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeesFromDb = await repository.Employee.GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        return employeesDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,
        EmployeeForCreationDto employeeForCreation,
        bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = mapper.Map<Employee>(employeeForCreation);

        repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await repository.SaveAsync();

        var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate,
        bool compTrackChanges,
        bool empTrackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = await repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if (employee is null) throw new EmployeeNotFoundException(id);

        mapper.Map(employeeForUpdate, employee);
        await repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = await repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if (employeeEntity is null) throw new EmployeeNotFoundException(id);

        var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        mapper.Map(employeeToPatch, employeeEntity);
        await repository.SaveAsync();
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, true);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeForCompany = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeForCompany is null) throw new EmployeeNotFoundException(id);

        repository.Employee.DeleteEmployee(company, employeeForCompany);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeFromDb = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeFromDb is null) throw new EmployeeNotFoundException(id);

        var employee = mapper.Map<EmployeeDto>(employeeFromDb);

        return employee;
    }
}