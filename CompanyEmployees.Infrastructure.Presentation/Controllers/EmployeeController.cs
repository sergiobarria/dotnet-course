using CompanyEmployees.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

[Route("api/companies/{companyId}/employees")]
[ApiController]
public class EmployeeController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = service.EmployeeService.GetEmployees(companyId, false);

        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = service.EmployeeService.GetEmployee(companyId, id, false);

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto? employee)
    {
        if (employee is null) return BadRequest("Employee object is null");

        var employeeToReturn =
            service.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
    }
}