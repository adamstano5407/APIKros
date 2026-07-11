#load "../.teapie/Definitions/ResponseClasses.csx"
using System.Text.Json;
using Xunit;

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

tp.Test("Hierarchy should be created correctly.", async () =>
{
    // Kontrola create requestov
    Equal(201, tp.Responses["CreateCompanyRequest"].StatusCode());
    Equal(201, tp.Responses["CreateEmployeeRequest"].StatusCode());
    Equal(201, tp.Responses["CreateDivisionRequest"].StatusCode());
    Equal(201, tp.Responses["CreateProjectRequest"].StatusCode());
    Equal(201, tp.Responses["CreateDepartmentRequest"].StatusCode());

    // Načítanie response body
    var companyJson = await tp.Responses["CreateCompanyRequest"]
        .Content.ReadAsStringAsync();

    var employeeJson = await tp.Responses["CreateEmployeeRequest"]
        .Content.ReadAsStringAsync();

    var divisionJson = await tp.Responses["CreateDivisionRequest"]
        .Content.ReadAsStringAsync();

    var projectJson = await tp.Responses["CreateProjectRequest"]
        .Content.ReadAsStringAsync();

    var departmentJson = await tp.Responses["CreateDepartmentRequest"]
        .Content.ReadAsStringAsync();

    // Deserializácia
    var company = JsonSerializer.Deserialize<CompanyResponse>(
        companyJson,
        jsonOptions);

    var employee = JsonSerializer.Deserialize<EmployeeResponse>(
        employeeJson,
        jsonOptions);

    var division = JsonSerializer.Deserialize<DivisionResponse>(
        divisionJson,
        jsonOptions);

    var project = JsonSerializer.Deserialize<ProjectResponse>(
        projectJson,
        jsonOptions);

    var department = JsonSerializer.Deserialize<DepartmentResponse>(
        departmentJson,
        jsonOptions);

    // Null kontroly
    NotNull(company);
    NotNull(employee);
    NotNull(division);
    NotNull(project);
    NotNull(department);

    // Kontrola ID
    True(company.Id > 0);
    True(employee.Id > 0);
    True(division.Id > 0);
    True(project.Id > 0);
    True(department.Id > 0);

    // Kontrola vzťahov
    Equal(company.Id, employee.CompanyId);

    Equal(company.Id, division.CompanyId);
    Equal(employee.Id, division.ManagerId);

    Equal(division.Id, project.DivisionId);
    Equal(employee.Id, project.ManagerId);

    Equal(project.Id, department.ProjectId);
    Equal(employee.Id, department.ManagerId);
});

tp.Test("Created hierarchy should be retrievable.", () =>
{
    Equal(200, tp.Responses["GetCompanyRequest"].StatusCode());
    Equal(200, tp.Responses["GetEmployeeRequest"].StatusCode());
    Equal(200, tp.Responses["GetDivisionRequest"].StatusCode());
    Equal(200, tp.Responses["GetProjectRequest"].StatusCode());
    Equal(200, tp.Responses["GetDepartmentRequest"].StatusCode());
});

tp.Test("Company should be deleted.", () =>
{
    Equal(204, tp.Responses["DeleteCompanyRequest"].StatusCode());
});

tp.Test("Deleted hierarchy should not be found.", () =>
{
    var deletedRequestNames = new[]
    {
        "GetDeletedCompanyRequest",
        "GetDeletedEmployeeRequest",
        "GetDeletedDivisionRequest",
        "GetDeletedProjectRequest",
        "GetDeletedDepartmentRequest"
    };

    foreach (var requestName in deletedRequestNames)
    {
        Equal(404, tp.Responses[requestName].StatusCode());
    }
});

tp.Test("Test variables should be removed.", () =>
{
    tp.RemoveVariablesWithTag("hierarchy-001");
});