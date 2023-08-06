using System;
using System.ComponentModel.DataAnnotations;

namespace website.Dto
{
    public class CustomerLoanManagerDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string CustomerName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LoanApplyAmountDate { get; set; }
        public decimal? LoanApplyAmount { get; set; }
        public decimal? LoanNetAmount { get; set; }
        public decimal? LoanEMI { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SectionAmountDate { get; set; }
        public decimal? SectionAmount { get; set; }
        public int? CreatedById { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public int? UpdateById { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedById { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeletedDate { get; set; }
        public bool? IsNoOfDays { get; set; }
        public bool? IsNoOfWeeks { get; set; }
        public bool? IsNoOfMonths { get; set; }
        public bool? IsNoOfYear { get; set; }
        public string NoOfDays { get; set; }
        public string NoOfWeeks { get; set; }
        public string NoOfMonths { get; set; }
        public string NoOfYears { get; set; }
        public decimal? LoanIntrest { get; set; }

        public decimal? ReceivedAmount { get; set; }
        public string LoanAccNo { get; set; }
        public string LoanNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LoanDate { get; set; }
    }
}