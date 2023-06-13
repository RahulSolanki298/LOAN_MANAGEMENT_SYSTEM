using website.Models;

namespace website.Dto
{
    public class SectionLatterDTO
    {
        public int? UserId { get; set; }
        public CompanyBranchDetail branchData { get; set; }
        public AddressMasterDto branchAddress { get; set; }
        public ApplicationCustomer customerData { get; set; }
        public AddressMasterDto customerAddress { get; set; }
        public CustomerLoanManagerDTO loanData { get; set; }
    }
}