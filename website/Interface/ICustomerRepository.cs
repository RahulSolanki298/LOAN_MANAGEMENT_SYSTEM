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
        bool SaveCustomer(CustomerRegistrationDTO customer);
        bool SaveCustomerAddress(AddressMasterDto address);
        bool SaveDocumentation(DocumentationDTO documenation);
        bool DeleteCustomer(int id);
    }
}