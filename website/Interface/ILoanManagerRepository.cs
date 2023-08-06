using System.Collections.Generic;
using website.Dto;

namespace website.Interface
{
    public interface ILoanManagerRepository
    {
        List<CustomerLoanManagerDTO> GetCustomersLoan(int userId, int branchId, string loanStatus, string loginUserRole);

        List<CustomerLoanCardDto> GetLoanReceableDayBook(int userId, int branchId, string loanStatus, string loginUserRole);
        SectionLatterDTO GetCustomerLoan(int id);

        CustomerLoanManagerDTO GetCustomerLoanData(int id);

        bool SaveCustomerLoanApply(CustomerLoanManagerDTO data);

        bool UpdateLoanStatus(int id, string status);

        string SaveEMIForMultipleUser(List<int> selectedIds, string paidBy, int branchId, string loginUserRole,int userId);

        LoanCardDetails GetCardWithLoanNo(string loanNo,string loanAccountNo);

        CustomerLoanCardDto getLoanDaywise(int id);


        bool CustomEMIPaid(CustomerLoanCardDto loanData);
    }
}