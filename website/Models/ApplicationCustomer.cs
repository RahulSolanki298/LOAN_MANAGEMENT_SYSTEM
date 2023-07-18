using System.ComponentModel.DataAnnotations;
using System;

namespace website.Models
{
    public class ApplicationCustomer
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string Gender { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        public string WhatsAppNo { get; set; }

        [Required]
        public string EmailId { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }

        public bool? IsActive { get; set; }

        public string LoanAppAccountNo { get; set; }
    }
}