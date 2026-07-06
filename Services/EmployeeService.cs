using APIKros.DTOs.Employee;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests.Employee;
using FluentValidation;

namespace APIKros.Services;


public interface IEmployeeService : IService<EmployeeDto, CreateEmployeeRequest,  UpdateEmployeeRequest, int>
{
    public Task ChangeCompany(int employeeId, ChangeCompanyRequest request);
    public Task UnassignEmployeeFromLeadershipPositions(int employeeId);
    public Task GetCompany(int employeeId);
}


public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    
    private readonly IValidator<CreateEmployeeRequest> _createEmployeeVal;
    private readonly IValidator<UpdateEmployeeRequest> _updateEmployeeVal;
    private readonly IValidator<ChangeCompanyRequest> _changeCompanyVal;

    public EmployeeService(IEmployeeRepository employeeRepository, IValidator<CreateEmployeeRequest> createEmployeeVal,  IValidator<UpdateEmployeeRequest> updateEmployeeVal, IValidator<ChangeCompanyRequest> changeCompanyVal)
    {
        _employeeRepository = employeeRepository;
        _createEmployeeVal = createEmployeeVal;
        _updateEmployeeVal = updateEmployeeVal;
        _changeCompanyVal = changeCompanyVal;
    }
    
    public async Task<EmployeeDto?> GetAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee == null ? throw new NotFoundException() : EmployeeDto.CreateInstance(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(employee => EmployeeDto.CreateInstance(employee)).ToList();
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeRequest request)
    {
        await _createEmployeeVal.ValidateAndThrowAsync(request);

        var employee = new Employee
        {
            Title = request.Title,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            CompanyId = request.CompanyId,
            EmployeeNumber = request.EmployeeNumber
        };

        await _employeeRepository.CreateAsync(employee);

        return EmployeeDto.CreateInstance(employee);
    }

    public async Task UpdateAsync(int id, UpdateEmployeeRequest request)
    {
        request.Id = id;

        var employee = await _employeeRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException("Employee not found.");

        await _updateEmployeeVal.ValidateAndThrowAsync(request);

        employee.Title = request.Title ?? employee.Title;
        employee.FirstName = request.FirstName ?? employee.FirstName;
        employee.LastName = request.LastName ?? employee.LastName;
        employee.Email = request.Email ?? employee.Email;
        employee.Phone = request.Phone ?? employee.Phone;
        employee.EmployeeNumber = request.EmployeeNumber ?? employee.EmployeeNumber;

        if (request.CompanyId.HasValue && request.CompanyId.Value != employee.CompanyId)
        {
            await _employeeRepository.UnassignEmployeeFromLeadershipPositionsAsync(employee.Id);
            employee.CompanyId = request.CompanyId.Value;
        }

        await _employeeRepository.UpdateAsync(employee);
        await _employeeRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeCompany(int employeeId ,ChangeCompanyRequest request)
    {
        request.EmployeeId = employeeId;
        await _changeCompanyVal.ValidateAndThrowAsync(request);
        
    }

    public async Task UnassignEmployeeFromLeadershipPositions(int employeeId)
    {
        await _employeeRepository.UnassignEmployeeFromLeadershipPositionsAsync(employeeId);
    }

    public Task GetCompany(int employeeId)
    {
        throw new NotImplementedException();
    }
}