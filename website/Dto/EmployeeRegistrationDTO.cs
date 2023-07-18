using System;
using System.ComponentModel.DataAnnotations;
using website.Helpers;

namespace website.Dto
{
    public class EmployeeRegistrationDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string WhatsAppNo { get; set; }
        public string EmailId { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string RoleName { get; set; } = ApplicationRole.Employee;
        public string EmployeeTitle { get; set; }
        public string EmployeeSalary { get; set; }
        public bool? IsActive { get; set; }
    }
}