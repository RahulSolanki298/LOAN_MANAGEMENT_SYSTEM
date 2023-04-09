using System;
using System.ComponentModel.DataAnnotations;

namespace website.Models
{
    public partial class CompanyBranchDetail
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25,ErrorMessage ="Please enter title")]
        public string Title { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Please enter subtitle")]
        public string SubTitle { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        public string Certificate { get; set; }
        public string CompanyLogo { get; set; }
        public DateTime? CompanyRegisterDate { get; set; }
        public bool? IsActivated { get; set; }
        public string CompanyBranchType { get; set; }
    }
}