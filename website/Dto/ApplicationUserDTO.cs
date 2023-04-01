using System;

namespace website.Interface
{
    public class ApplicationUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string WhatsAppNo { get; set; }
        public string EmailId { get; set; }
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string RoleName { get; set; }
        public string LoanAppAccountNo { get; set; }
        public bool? IsActive { get; set; }
    }
}