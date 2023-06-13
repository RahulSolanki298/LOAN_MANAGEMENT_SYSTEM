using System.Collections.Generic;
using website.Dto;

namespace website.Interface
{
    public interface IEmployeeRepository
    {
        EmployeeRegistrationDTO GetEmployeePrimaryData(int id);
        AddressMasterDto GetEmployeeAddress(int userId);
        DocumentationDTO GetDocumentation(int userId);
        List<EmployeeRegistrationDTO> GetActiveEmployees();
        List<EmployeeRegistrationDTO> GetNonActiveEmployees();
        int SaveEmployees(EmployeeRegistrationDTO customer);
        int SaveEmployeesAddress(AddressMasterDto address);
        int SaveDocumentation(DocumentationDTO documenation);
    }
}