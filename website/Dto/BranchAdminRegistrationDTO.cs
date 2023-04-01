using System;
using website.Helpers;

namespace website.Dto
{
    public class BranchAdminRegistrationDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string WhatsAppNo { get; set; }
        public string EmailId { get; set; }
        public int? BranchId { get; set; }
        public string RoleName { get; set; } = ApplicationRole.BranchAdmin;
        public bool? IsActive { get; set; }
    }
}