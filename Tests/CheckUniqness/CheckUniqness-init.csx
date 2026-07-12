#load "../../.teapie/Definitions/Company/Company.csx"
#load "../../.teapie/Definitions/Division/Division.csx"
#load "../../.teapie/Definitions/Project/Project.csx"
#load "../../.teapie/Definitions/Department/Department.csx"
#load "../../.teapie/Definitions/Employee/Employee.csx"
#load "../../.teapie/Definitions/Generator.csx"
#load "../../.teapie/Definitions/SetObjectVariable.csx"

const string Tag = "hierarchy-002";

var company = GenerateCompany();
var employee = GenerateEmployee();
var division = GenerateDivision();
var project = GenerateProject();
var department = GenerateDepartment();

var companyClone = company.Clone();
var employeeClone = employee.Clone();
var divisionClone = division.Clone();
var projectClone = project.Clone();
var departmentClone = department.Clone();

var companyCodeClone = GenerateCompany();
companyCodeClone.Code = company.Code;

var divisionCodeClone = GenerateDivision();
divisionCodeClone.Code = division.Code;

var projectCodeClone = GenerateProject();
projectCodeClone.Code = project.Code;

var departmentCodeClone = GenerateDepartment();
departmentCodeClone.Code = department.Code;

var employeeEmailClone = GenerateEmployee();
employeeEmailClone.Email = employee.Email;

var employeeNumberClone = GenerateEmployee();
employeeNumberClone.EmployeeNumber = employee.EmployeeNumber;

tp.SetVariable("Company", company.ToJsonString(), Tag);
tp.SetVariable("CompanyClone", companyClone.ToJsonString(), Tag);
tp.SetVariable("CompanyCodeClone", companyCodeClone.ToJsonString(), Tag);


SetObjectVariables("Employee", employee, Tag);
SetObjectVariables("EmployeeClone", employeeClone, Tag);
SetObjectVariables("EmployeeEmailClone", employeeEmailClone, Tag);
SetObjectVariables("EmployeeNumberClone", employeeNumberClone, Tag);

SetObjectVariables("Division", division, Tag);
SetObjectVariables("DivisionClone", divisionClone, Tag);
SetObjectVariables("DivisionCodeClone", divisionCodeClone, Tag);

SetObjectVariables("Project", project, Tag);
SetObjectVariables("ProjectClone", projectClone, Tag);
SetObjectVariables("ProjectCodeClone", projectCodeClone, Tag);

SetObjectVariables("Department", department, Tag);
SetObjectVariables("DepartmentClone", departmentClone, Tag);
SetObjectVariables("DepartmentCodeClone", departmentCodeClone, Tag);