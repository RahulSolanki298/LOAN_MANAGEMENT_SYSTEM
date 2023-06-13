using System;

namespace website.Dto
{
    public class CustomerLoanCardDto
    {
        public int Id { get; set; }

        public string LoanAccNo { get; set; }
        public string LoanNo { get; set; }

        public DateTime? RepaymentDate { get; set; }

        public decimal? Inst_Amount { get; set; }

        public decimal? Principal { get; set; }

        public decimal? Intrest { get; set; }

        public decimal? OS_pricipal { get; set; }

        public string PaiderName { get; set; }

        public decimal? AmountCollected { get; set; }

        public bool? PaidStatus { get; set; }

    }
}