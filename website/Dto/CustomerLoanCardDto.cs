using System;
using System.ComponentModel.DataAnnotations;

namespace website.Dto
{
    public class CustomerLoanCardDto
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public int? BranchId { get; set; }
        public string LoanAccNo { get; set; }
        public string LoanNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RepaymentDate { get; set; }

        public decimal? Inst_Amount { get; set; }

        public string PaiderName { get; set; }

        public decimal? AmountCollected { get; set; }

        public string PaidBy { get; set; }

        public decimal? Principal { get; set; }

        public decimal? Intrest { get; set; }

        public decimal? OS_pricipal { get; set; }

        public bool? PaidStatus { get; set; }
        
        public int? CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        public int? EntryBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EntryDate { get; set;}

    }
}