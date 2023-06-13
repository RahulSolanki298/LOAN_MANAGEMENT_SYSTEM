using System.Collections.Generic;
using website.Dto;

namespace website.Interface
{
    public interface ICustomerRepository
    {
        CustomerRegistrationDTO GetCustomerPrimaryData(int id);
        AddressMasterDto GetCustomerAddress(int userId);
        DocumentationDTO GetDocumentation(int userId);
        List<CustomerRegistrationDTO> GetActiveCustomers();
        List<CustomerRegistrationDTO> GetNonActiveCustomers();
        int SaveCustomer(CustomerRegistrationDTO customer);
        int SaveCustomerAddress(AddressMasterDto address);
        int SaveDocumentation(DocumentationDTO documenation);
        bool DeleteCustomer(int id);
        DashboardDTO Dashboard();
    }
}