using System.Collections.Generic;

namespace website.Dto
{
    public class DashboardDTO
    {
        public int ActiveClientNo { get; set; }

        public int DeActiveClientNo { get; set; }

        public int TodayLoanApply { get; set; }

        public int ComplateLoan { get; set; }

        public List<CustomerLoanCardDto> CustomerLoanList { get; set; }

    }
}