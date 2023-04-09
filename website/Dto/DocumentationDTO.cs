using System.Web;

namespace website.Dto
{
    public class DocumentationDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string AadharCardNo { get; set; }
        public string AadharCardImagePath { get; set; }
        public HttpPostedFileBase AadharCardImagePathFile { get; set; }
        public string PancardNo { get; set; }
        public string PancardImagePath { get; set; }
        public HttpPostedFileBase PancardImagePathFile { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string IFSCode { get; set; }
        public string PassBookImgPath { get; set; }
        public HttpPostedFileBase PassBookImgPathFile { get; set; }
        public string CheckBooKImgPath { get; set; }
        public HttpPostedFileBase CheckBooKImgPathFile { get; set; }
        public string ProfileImgPath { get; set; }
        public HttpPostedFileBase ProfileImgPathFile { get; set; }

    }
}