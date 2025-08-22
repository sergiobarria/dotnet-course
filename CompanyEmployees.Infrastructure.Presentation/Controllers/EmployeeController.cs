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
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
    {
        var employees = await service.EmployeeService.GetEmployeesAsync(companyId, false);

        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = await service.EmployeeService.GetEmployeeAsync(companyId, id, false);

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId,
        [FromBody] EmployeeForCreationDto? employee,
        [FromServices] IValidator<EmployeeForManipulationDto> validator)
    {
        if (employee is null) return BadRequest("Employee object is null");

        var validationResult = await validator.ValidateAsync(employee);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        // if (!ModelState.IsValid)
        // {
        //     ModelState.AddModelError(string.Empty, "Bad Request sent");
        //     return UnprocessableEntity(ModelState);
        // }

        var employeeToReturn =
            await service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdateDto? employee,
        [FromServices] IValidator<EmployeeForUpdateDto> validator)
    {
        if (employee is null) return BadRequest("Employee object is null");

        var validationResult = await validator.ValidateAsync(employee);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        // if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        await service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee, false, true);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto>? patchDoc)
    {
        if (patchDoc is null) return BadRequest("Employee object is null");

        var result =
            await service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);

        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        await service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        await service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, false);

        return NoContent();
    }
}