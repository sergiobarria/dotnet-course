using CompanyEmployees.Core.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
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
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto? employee,
        [FromServices] IValidator<EmployeeForManipulationDto> validator)
    {
        if (employee is null) return BadRequest("Employee object is null");

        var validationResult = validator.Validate(employee);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        // if (!ModelState.IsValid)
        // {
        //     ModelState.AddModelError(string.Empty, "Bad Request sent");
        //     return UnprocessableEntity(ModelState);
        // }

        var employeeToReturn =
            service.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto? employee,
        [FromServices] IValidator<EmployeeForUpdateDto> validator)
    {
        if (employee is null) return BadRequest("Employee object is null");

        var validationResult = validator.Validate(employee);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        // if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, false, true);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto>? patchDoc)
    {
        if (patchDoc is null) return BadRequest("Employee object is null");

        var result =
            service.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);

        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        service.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        service.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);

        return NoContent();
    }
}