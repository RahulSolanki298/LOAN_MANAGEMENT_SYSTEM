using System.Collections.Generic;
using website.Dto;

namespace website.Interface
{
    public interface ILoanManagerRepository
    {
        List<CustomerLoanManagerDTO> GetCustomerLoan();
    }
}