using System;

namespace website.Dto
{
    public class SectionLetterInvoiceDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string EmailId { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }

        public string Pancard { get; set; }

        public string AadharCard { get; set; }

        public DateTime? LoanApplyAmountDate { get; set; }

        public decimal? LoanApplyAmount { get; set; }

        public decimal? LoanEMI { get; set; }

        public DateTime? SectionAmountDate { get; set; }

        public decimal? SectionAmount { get; set; }

        public string NoOfDays { get; set; }

        public decimal? LoanIntrest { get; set; }

        public string LoanAccNo { get; set; }
        
    }
}