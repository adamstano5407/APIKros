using APIKros.Data;
using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using APIKros.DTOs.Company;
using APIKros.DTOs.Department;
using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;
using APIKros.DTOs.Project;
using APIKros.Requests.Company;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompanyController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EndpointName("GetAllCompanies")]
    [EndpointSummary("Get all companies")]
    [EndpointDescription("Returns a list of all companies")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var companies = await _context.Companies
            .Select(c => CompanyDto.CreateInstance(c))
            .ToListAsync();

        return Ok(companies);
    }

    [HttpGet("{id}")]
    [EndpointName("GetCompanyDetail")]
    [EndpointSummary("Get Company By Id")]
    [EndpointDescription("Return Company with basic info")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(CompanyDto.CreateInstance(company));
    }

    [HttpPost]
    [EndpointName("CreateCompany")]
    [EndpointSummary("Create company")]
    [EndpointDescription("Creates a new company and optionally assigns an existing employee as the company director.")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest dto)
    {
        var company = new Company
        {
            Name = dto.Name,
            Code = dto.Code,

        };

        if (dto.DirectorId.HasValue)
            company.DirectorId = dto.DirectorId.Value;

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = company.Id },
            CompanyDto.CreateInstance(company)
        );
    }
    
    [HttpPut("{id}")]
    [EndpointName("UpdateCompany")]
    [EndpointSummary("Update company")]
    [EndpointDescription("Updates an existing company by ID. Only provided fields are changed.")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyRequest dto)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        if (dto.Name is not null)
            company.Name = dto.Name;

        if (dto.Code is not null)
            company.Code = dto.Code;

        if (dto.DirectorId.HasValue)
            company.DirectorId = dto.DirectorId.Value;

        await _context.SaveChangesAsync();

        return Ok(CompanyDto.CreateInstance(company));
    }

    [HttpDelete("{id}")]
    [EndpointName("DeleteCompany")]
    [EndpointSummary("Delete company")]
    [EndpointDescription("Deletes an existing company from the system.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/details")]
    [EndpointName("GetCompanyDetails")]
    [EndpointSummary("Get company details")]
    [EndpointDescription("Returns detailed information about a company, including director, divisions, and employees.")]
    [ProducesResponseType(typeof(DetailedCompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetails(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Director)
            .Include(c => c.Divisions)
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(DetailedCompanyDto.CreateInstance(company));
    }


    [HttpGet("{id}/structured")]
    [EndpointName("GetCompanyStructured")]
    [EndpointSummary("Get company structure")]
    [EndpointDescription("Returns the full organizational structure of a company, including divisions, projects, departments, and managers.")]
    [ProducesResponseType(typeof(StructuredCompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStructured(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Director)
            .Include(c => c.Employees)

            .Include(c => c.Divisions)
                .ThenInclude(d => d.Manager)

            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
                    .ThenInclude(p => p.Manager)

            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
                    .ThenInclude(p => p.Departments)
                        .ThenInclude(dep => dep.Manager)

            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(StructuredCompanyDto.CreateInstance(company));
    }

    [HttpGet("{id}/employees")]
    [EndpointName("GetCompanyEmployees")]
    [EndpointSummary("Get company employees")]
    [EndpointDescription("Returns all employees assigned to the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployees(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var employeesDto = company.Employees
            .Select(EmployeeDto.CreateInstance)
            .ToList();
        return Ok(employeesDto);
    }

    [HttpGet("{id}/divisions")]
    [EndpointName("GetCompanyDivisions")]
    [EndpointSummary("Get company divisions")]
    [EndpointDescription("Returns all divisions that belong to the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<DivisionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDivisions(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Divisions)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var divisionsDto = company.Divisions
            .Select(DivisionDto.CreateInstance)
            .ToList();
        return Ok(divisionsDto);
    }
    
    [HttpGet("{id}/projects")]
    [EndpointName("GetCompanyProjects")]
    [EndpointSummary("Get company projects")]
    [EndpointDescription("Returns all projects from all divisions of the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjects(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var projectsDto = company.Divisions
            .SelectMany(d => d.Projects)
            .Select(ProjectDto.CreateInstance)
            .ToList();
        return Ok(projectsDto);
    }
    
    [HttpGet("{id}/departments")]
    [EndpointName("GetCompanyDepartments")]
    [EndpointSummary("Get company departments")]
    [EndpointDescription("Returns all departments from all projects and divisions of the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartments(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
                    .ThenInclude(p => p.Departments)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var departmentsDto = company.Divisions
            .SelectMany(d => d.Projects)
            .SelectMany(p => p.Departments)
            .Select(DepartmentDto.CreateInstance)
            .ToList();
        return Ok(departmentsDto);
    }

    [HttpPut("assign-director")]
    [EndpointName("AssignCompanyDirector")]
    [EndpointSummary("Assign company director")]
    [EndpointDescription("Assigns an existing employee as the director of a company. The employee must belong to the specified company.")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignDirector([FromBody] AssignDirectorRequest request)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId);

        company!.DirectorId = request.NewDirectorId;

        await _context.SaveChangesAsync();

        return Ok(CompanyDto.CreateInstance(company));
    }
}