using APIKros.Data;
using APIKros.DTOs;
using APIKros.Models;
using APIKRos.Requests.Company;
using Microsoft.AspNetCore.Mvc;
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
            DirectorId = dto.DirectorId
        };

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

        company.Name = dto.Name;
        company.Code = dto.Code;
        company.DirectorId = dto.DirectorId;

        await _context.SaveChangesAsync();

        return Ok(CompanyDTO.CreateInstance(company));
    }


    public async Task<IActionResult> Delete(DeleteCompanyRequest request)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.Id);

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



}