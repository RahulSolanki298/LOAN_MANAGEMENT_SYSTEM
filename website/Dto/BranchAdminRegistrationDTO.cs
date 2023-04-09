using System;
using System.ComponentModel.DataAnnotations;
using website.Helpers;

namespace website.Dto
{
    public class BranchAdminRegistrationDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        public string Gender { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", 
            ErrorMessage = "Not a valid phone number")]
        public string MobileNo { get; set; }

        [Required]
        public string WhatsAppNo { get; set; }

        [Required]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Not a valid email id")]
        public string EmailId { get; set; }

        [Required]
        
        public int? BranchId { get; set; }
        public string RoleName { get; set; } = ApplicationRole.BranchAdmin;
        public bool? IsActive { get; set; }
    }
}