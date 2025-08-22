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
    public async Task<IActionResult> GetCompanies(CancellationToken ct)
    {
        var companies = await service.CompanyService.GetAllCompaniesAsync(false, ct);

        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id, CancellationToken ct)
    {
        var company = await service.CompanyService.GetCompanyAsync(id, false, ct);

        return Ok(company);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        var companies = await service.CompanyService.GetByIdsAsync(ids, false, ct);

        return Ok(companies);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto? company,
        CancellationToken ct)
    {
        if (company is null) return BadRequest("Company is null");

        var createdCompany = await service.CompanyService.CreateCompanyAsync(company, ct);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyForCreationDto> companyCollection, CancellationToken ct)
    {
        var result = await service.CompanyService.CreateCompanyCollectionAsync(companyCollection, ct);

        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto? companyForUpdate,
        CancellationToken ct)
    {
        if (companyForUpdate is null) return BadRequest("CompanyForUpdate is null");

        await service.CompanyService.UpdateCompanyAsync(id, companyForUpdate, true, ct);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id, CancellationToken ct)
    {
        await service.CompanyService.DeleteCompanyAsync(id, false, ct);

        return NoContent();
    }
}