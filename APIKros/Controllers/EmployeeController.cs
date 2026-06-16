
using APIKros.Data;
using APIKros.DTOs;
using APIKros.Models;
using APIKros.Requests;
using APIKros.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointName("GetAllEmployees")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _context.Employees
             .Select(e => EmployeeDTO.CreateInstance(e))
             .ToListAsync();

            return Ok(employees);
        }


        [HttpGet("{id}")]
        [EndpointName("GetEmployeeById")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(EmployeeDTO.CreateInstance(employee));
        }

        [HttpPost(Name = "CreateEmployee")]
[EndpointName("CreateEmployee")]
[EndpointSummary("Create employee")]
[EndpointDescription("Creates a new employee and assigns the employee to a company.")]
[ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
        {
            var employee = new Employee
            {
                Title = request.Title,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                CompanyId = request.CompanyId
            };
                
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                EmployeeDTO.CreateInstance(employee)
            );
        }

        [HttpPut("{id}")]
        [EndpointName("UpdateEmployee")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            if (request.Title is not null)
                employee.Title = request.Title;

            if (request.FirstName is not null)
                employee.FirstName = request.FirstName;

            if (request.LastName is not null)
                employee.LastName = request.LastName;

            if (request.Email is not null)
            {
                var emailExists = await _context.Employees
                    .AnyAsync(e => e.Email == request.Email && e.Id != id);

                if (emailExists)
                    return BadRequest("Employee with this email already exists.");
            }

            if (request.Phone is not null)
                employee.Phone = request.Phone;

            if (request.CompanyId.HasValue)
                employee.CompanyId = request.CompanyId.Value;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null)
                return NotFound("Employee not found.");

            var companies = await _context.Companies
                .Where(c => c.DirectorId == id)
                .ToListAsync();

            foreach (var company in companies)
                company.DirectorId = null;

            var divisions = await _context.Divisions
                .Where(d => d.ManagerId == id)
                .ToListAsync();

            foreach (var division in divisions)
                division.ManagerId = null;

            var projects = await _context.Projects
                .Where(p => p.ManagerId == id)
                .ToListAsync();

            foreach (var project in projects)
                project.ManagerId = null;

            var departments = await _context.Departments
                .Where(d => d.ManagerId == id)
                .ToListAsync();

            foreach (var department in departments)
                department.ManagerId = null;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPut("{id}/company/{companyId}")]
        public async Task<IActionResult> ChangeCompany(int id, int companyId)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null)
                return NotFound("Employee not found.");

            var companyExists = await _context.Companies.AnyAsync(c => c.Id == companyId);
            if (!companyExists)
                return BadRequest("Company does not exist.");

            var companies = await _context.Companies
                .Where(c => c.DirectorId == id)
                .ToListAsync();

            foreach (var company in companies)
                company.DirectorId = null;

            var divisions = await _context.Divisions
                .Where(d => d.ManagerId == id)
                .ToListAsync();

            foreach (var division in divisions)
                division.ManagerId = null;

            var projects = await _context.Projects
                .Where(p => p.ManagerId == id)
                .ToListAsync();

            foreach (var project in projects)
                project.ManagerId = null;

            var departments = await _context.Departments
                .Where(d => d.ManagerId == id)
                .ToListAsync();

            foreach (var department in departments)
                department.ManagerId = null;

            employee.CompanyId = companyId;

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}