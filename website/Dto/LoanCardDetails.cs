using System;
using System.Collections.Generic;

namespace website.Dto
{
    public class LoanCardDetails
    {
        public CustomerCardInfo CustomerCardInfo { get; set; }

        public AddressMasterDto AddressMaster { get; set; }

        public List<CustomerLoanCardDto> LoanCardEMIs { get; set; }


    }
}