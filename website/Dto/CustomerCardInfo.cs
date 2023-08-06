using System;
using System.ComponentModel.DataAnnotations;

namespace website.Dto
{
    public class CustomerCardInfo
    {
        public int Id { get; set; }

        public string BranchName { get; set; }

        public string LoanNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LoanDate { get; set; }

        public string LoanAccNo { get; set; }

        public int? CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerProfile { get; set; }

        public decimal SectionAmount { get; set; }

        public int? NoOfDays { get; set; }

        public decimal LoanNetAmount { get; set; }

        public decimal? ReceivedAmount { get; set; }
    }
}