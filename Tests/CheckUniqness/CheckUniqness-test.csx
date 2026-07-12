#load "../../.teapie/Definitions/ResponseClasses.csx"
using System.Text.Json;
using Xunit;

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

tp.Test("Base Hiearch should be created", () => {
  Equal(201, tp.Responses["CreateCompanyRequest"].StatusCode());
  Equal(201, tp.Responses["CreateEmployeeRequest"].StatusCode());
  Equal(201, tp.Responses["CreateDivisionRequest"].StatusCode());
  Equal(201, tp.Responses["CreateProjectRequest"].StatusCode());
  Equal(201, tp.Responses["CreateDepartmentRequest"].StatusCode());
});


tp.Test("Unique constraint violation will throw BadRequest 400", () => {
  Equal(400, tp.Responses["CreateCompanyCloneRequest"].StatusCode());
  Equal(400, tp.Responses["CreateCompanyCodeCloneRequest"].StatusCode());

  Equal(400, tp.Responses["CreateEmployeeCloneRequest"].StatusCode());
  Equal(400, tp.Responses["CreateEmployeeEmailCloneRequest"].StatusCode());
  Equal(400, tp.Responses["CreateEmployeeNumberCloneRequest"].StatusCode());

  Equal(400, tp.Responses["CreateDivisionCloneRequest"].StatusCode());
  Equal(400, tp.Responses["CreateDivisionCodeCloneRequest"].StatusCode());

  Equal(400, tp.Responses["CreateProjectCloneRequest"].StatusCode());
  Equal(400, tp.Responses["CreateProjectCodeCloneRequest"].StatusCode());

  Equal(400, tp.Responses["CreateDepartmentCloneRequest"].StatusCode());
  Equal(400, tp.Responses["CreateDepartmentCodeCloneRequest"].StatusCode());
});


tp.Test("Cleanup should remove created hierarchy", () => {
  Equal(204, tp.Responses["DeleteDepartmentRequest"].StatusCode());
  Equal(204, tp.Responses["DeleteProjectRequest"].StatusCode());
  Equal(204, tp.Responses["DeleteDivisionRequest"].StatusCode());
  Equal(204, tp.Responses["DeleteEmployeeRequest"].StatusCode());
  Equal(204, tp.Responses["DeleteCompanyRequest"].StatusCode());
});
