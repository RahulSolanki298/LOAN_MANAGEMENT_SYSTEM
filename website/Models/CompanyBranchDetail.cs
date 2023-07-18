using System;
using System.ComponentModel.DataAnnotations;

namespace website.Models
{
    public partial class CompanyBranchDetail
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250,ErrorMessage ="Please enter title.")]
        public string Title { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Please enter subtitle.")]
        public string SubTitle { get; set; }

        [Required]
        public string Description { get; set; }

        public string Certificate { get; set; }

        public string CompanyLogo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CompanyRegisterDate { get; set; }

        public bool? IsActivated { get; set; }

        public string CompanyBranchType { get; set; }
    }
}