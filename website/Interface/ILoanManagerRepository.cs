﻿using System.Collections.Generic;
using website.Dto;

namespace website.Interface
{
    public interface ILoanManagerRepository
    {
        List<CustomerLoanManagerDTO> GetCustomersLoan(int userId, int branchId, string loanStatus, string loginUserRole);

        SectionLatterDTO GetCustomerLoan(int id);

        CustomerLoanManagerDTO GetCustomerLoanData(int id);

        bool SaveCustomerLoanApply(CustomerLoanManagerDTO data);

        bool UpdateLoanStatus(int id, string status);
    }
}