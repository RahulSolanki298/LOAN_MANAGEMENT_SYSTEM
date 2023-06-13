namespace website.Dto
{
    public class LoanApplyDTO
    {
        public int? Id { get; set; }

        public int? UserId { get; set; }

        public int? BranchId { get; set; }

        public decimal? LoanApplyAmount { get; set; }

        public decimal? LoanNetAmount { get; set; }

        public decimal? LoanEMI { get; set; }

        public string LoanAccNo { get; set; }

        public string NoOfDays { get; set; }

        public decimal? LoanIntrest { get; set; }
    }
}