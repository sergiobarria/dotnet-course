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
    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeesFromDb = repository.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        return employeesDto;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeFromDb = repository.Employee.GetEmployee(companyId, id, trackChanges);
        if (employeeFromDb is null) throw new EmployeeNotFoundException(id);

        var employee = mapper.Map<EmployeeDto>(employeeFromDb);

        return employee;
    }

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation,
        bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = mapper.Map<Employee>(employeeForCreation);

        repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        repository.Save();

        var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, true);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeForCompany = repository.Employee.GetEmployee(companyId, id, trackChanges);
        if (employeeForCompany is null) throw new EmployeeNotFoundException(id);

        repository.Employee.DeleteEmployee(company, employeeForCompany);
    }
}