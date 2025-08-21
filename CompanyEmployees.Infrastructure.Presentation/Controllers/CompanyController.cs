using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Infrastructure.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompanyController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCompanies()
    {
        var companies = service.CompanyService.GetAllCompanies(false);

        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = service.CompanyService.GetCompany(id, false);

        return Ok(company);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = service.CompanyService.GetByIds(ids, false);

        return Ok(companies);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto? company)
    {
        if (company is null) return BadRequest("Company is null");

        var createdCompany = service.CompanyService.CreateCompany(company);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = service.CompanyService.CreateCompanyCollection(companyCollection);

        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }
}