using CompanyEmployees.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCompanies()
    {
        try
        {
            var companies = service.CompanyService.GetAllCompanies(false);
            return Ok(companies);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}