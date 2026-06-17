using APIKros.Data;
using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
    public async Task<IActionResult> GetAll()
    {
        var companies = await _context.Companies
            .Select(c => CompanyDTO.CreateInstance(c))
            .ToListAsync();

        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(CompanyDTO.CreateInstance(company));
    }

    [HttpPost]
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
            CompanyDTO.CreateInstance(company)
        );
    }
    [HttpPut("{id}")]
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

        return Ok(CompanyDTO.CreateInstance(company));
    }

    [HttpDelete("{id}")]
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
    public async Task<IActionResult> GetDetails(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Director)
            .Include(c => c.Divisions)
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(DetailedCompanyDTO.CreateInstance(company));
    }


    [HttpGet("{id}/structured")]
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

        return Ok(StructuredCompanyDTO.CreateInstance(company));
    }

    [HttpGet("{id}/employees")]
    public async Task<IActionResult> GetEmployees(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var employeesDto = company.Employees
            .Select(EmployeeDTO.CreateInstance)
            .ToList();
        return Ok(employeesDto);
    }

    [HttpGet("{id}/divisions")]
    public async Task<IActionResult> GetDivisions(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Divisions)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var divisionsDto = company.Divisions
            .Select(DivisionDTO.CreateInstance)
            .ToList();
        return Ok(divisionsDto);
    }
    
    [HttpGet("{id}/projects")]
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
            .Select(ProjectDTO.CreateInstance)
            .ToList();
        return Ok(projectsDto);
    }
    
    [HttpGet("{id}/departments")]
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
            .Select(DepartmentDTO.CreateInstance)
            .ToList();
        return Ok(departmentsDto);
    }

    [HttpPut("{id}/director/{employeeId}")]
    public async Task<IActionResult> AssignDirector(int id, int employeeId)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound("Company not found.");

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee is null)
            return NotFound("Employee not found.");

        if (employee.CompanyId != id)
            return BadRequest("Employee must belong to this company.");

        company.DirectorId = employeeId;

        await _context.SaveChangesAsync();

        return Ok(CompanyDTO.CreateInstance(company));
    }
}