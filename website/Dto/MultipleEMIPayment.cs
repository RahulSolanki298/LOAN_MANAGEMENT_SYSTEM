using System.Collections.Generic;

namespace website.Dto
{
    public class MultipleEMIPayment
    {
        public List<int> selectedIds { get; set; }

        public string paidBy { get; set; }

        public int branchId { get; set; }

        public string loginUserRole { get; set; }

        public int userId { get; set; }
    }
}