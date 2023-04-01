using System.Collections.Generic;
using website.Dto;

namespace website.Interface
{
    public interface ICustomerRepository
    {
        CustomerRegistrationDTO GetCustomerPrimaryData(int id);
        List<CustomerRegistrationDTO> GetActiveCustomers();
        List<CustomerRegistrationDTO> GetNonActiveCustomers();
        string SaveCustomer();
        bool DeleteCustomer(int id);
    }
}