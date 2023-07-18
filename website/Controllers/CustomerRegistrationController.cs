using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using website.Dto;
using website.Helpers;
using website.Interface;
using static iTextSharp.text.pdf.XfaXpathConstructor;

namespace website.Controllers
{
    [Authorize]
    public class CustomerRegistrationController : Controller
    {
        // GET: CustomerRegistration
        public readonly IBranchRepository _branchRepo;
        public readonly ICustomerRepository _customerRepo;

        public CustomerRegistrationController(IBranchRepository branchRepo, ICustomerRepository customerRepo)
        {
            _branchRepo = branchRepo;
            _customerRepo = customerRepo;
        }

        public ActionResult Index(int? id = 0)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpGet]
        public ActionResult CreateRegister(string id = "0")
        {
            var Response = new CustomerRegistrationDTO();
            var branchId = 0;
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();
            var usrId = Convert.ToInt32(id);

            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            if (usrId > 0)
            {
                Response = _customerRepo.GetCustomerPrimaryData(usrId);
                return View(Response);
            }
            if (branchId > 0)
            {
                Response.BranchId = branchId;
            }

            return View(Response);
        }

        [HttpPost]
        public ActionResult CreateRegister(CustomerRegistrationDTO customer)
        {
            try
            {
                ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();

                //if (ModelState.IsValid)
                //{
                customer.RoleName = ApplicationRole.Customer;
                var id = _customerRepo.SaveCustomer(customer);

                TempData["Success"] = "Customer personal detail saved successfully.";
                return RedirectToAction("Index", new { id = id });
                //}
                //TempData["Error"] = "Please enter customer registration.";
                //return RedirectToAction("Index");


            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Exception : {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateAddressDetails(int userId = 0)
        {
            var Response = new AddressMasterDto();
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();
            if (userId > 0)
            {
                Response = _customerRepo.GetCustomerAddress(userId);
                return View(Response);
            }

            return View(Response);
        }

        [HttpPost]
        public ActionResult CreateAddressDetails(AddressMasterDto address)
        {

            var Response = _customerRepo.SaveCustomerAddress(address);
            TempData["Success"] = "Customer address saved successfully.";
            return RedirectToAction("Index", new { userId = Response });
        }

        [HttpGet]
        public ActionResult CreateDocumentation(int userId = 0)
        {
            var Response = new DocumentationDTO();
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();
            if (userId > 0)
            {
                Response = _customerRepo.GetDocumentation(userId);
                return View(Response);
            }

            return View(Response);
        }

        [HttpPost]
        public ActionResult CreateDocumentation(DocumentationDTO documentation)
        {
            if (documentation.AadharCardImagePathFile != null)
            {
                string path = Server.MapPath("~/Documentation/AadharCards");
                string adharFileName = Path.GetFileName(documentation.AadharCardImagePathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + adharFileName.Split('.').Last();

                string adharFullPath = Path.Combine(path, renameFile);
                documentation.AadharCardImagePathFile.SaveAs(adharFullPath);
                documentation.AadharCardImagePath = renameFile;
            }

            if (documentation.PancardImagePathFile != null)
            {
                string path = Server.MapPath("~/Documentation/Pancards");
                string pancardFileName = Path.GetFileName(documentation.PancardImagePathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + pancardFileName.Split('.').Last();

                string pancardFullPath = Path.Combine(path, renameFile);
                documentation.PancardImagePathFile.SaveAs(pancardFullPath);
                documentation.PancardImagePath = renameFile;
            }

            if (documentation.PassBookImgPathFile != null)
            {
                string path = Server.MapPath("~/Documentation/PassBooks");
                string passbookFileName = Path.GetFileName(documentation.PassBookImgPathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + passbookFileName.Split('.').Last();

                string passbookFullPath = Path.Combine(path, renameFile);
                documentation.PassBookImgPathFile.SaveAs(passbookFullPath);
                documentation.PassBookImgPath = renameFile;
            }

            if (documentation.CheckBooKImgPathFile != null)
            {
                string path = Server.MapPath("~/Documentation/CheckBooks");
                string checkBookFileName = Path.GetFileName(documentation.CheckBooKImgPathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + checkBookFileName.Split('.').Last();

                string checkBookFullPath = Path.Combine(path, renameFile);
                documentation.CheckBooKImgPathFile.SaveAs(checkBookFullPath);
                documentation.CheckBooKImgPath = renameFile;
            }

            if (documentation.ProfileImgPathFile != null)
            {
                string path = Server.MapPath("~/Documentation/Profiles");
                string profileFileName = Path.GetFileName(documentation.ProfileImgPathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + profileFileName.Split('.').Last();

                string profileFullPath = Path.Combine(path, renameFile);
                documentation.ProfileImgPathFile.SaveAs(profileFullPath);
                documentation.ProfileImgPath = renameFile;
            }

            var Response = _customerRepo.SaveDocumentation(documentation);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateAccountDetails(int userId = 0)
        {
            var Response = new CustomerRegistrationDTO();
            var usrId = Convert.ToInt32(userId);
            if (usrId > 0)
            {
                Response = _customerRepo.GetCustomerPrimaryData(usrId);
                return View(Response);
            }

            return View(Response);
        }

        [HttpPost]
        public ActionResult CreateAccountDetails(CustomerRegistrationDTO register)
        {

            var response = _customerRepo.SaveCustomer(register);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            if (file != null && file.ContentLength > 0)
            {
                string path = Server.MapPath("~/Documentation/CustomerImports/");
                FileInfo fileInfo = new FileInfo(Path.Combine(path, file.FileName));
                file.SaveAs(fileInfo.FullName);

                int userId = Convert.ToInt32(Session["UserId"].ToString());
                using (var package = new ExcelPackage(fileInfo))
                //using (var package = new ExcelPackage(file.InputStream))
                {
                    //var worksheet = package.Workbook.Worksheets[1];
                    var worksheet = package.Workbook.Worksheets["Sheet1"];
                    //ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;


                    List<ApplicationUserDTO> excelDataList = new List<ApplicationUserDTO>();
                    var hmac = new HMACSHA512();

                    for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                    {
                        hmac = new HMACSHA512();
                        ApplicationUserDTO excelData = new ApplicationUserDTO();
                        excelData.FirstName = worksheet.Cells[row, 1].Value.ToString();
                        excelData.MiddleName = worksheet.Cells[row, 2].Value.ToString();
                        excelData.LastName = worksheet.Cells[row, 3].Value.ToString();
                        excelData.DateOfBirth = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString());
                        excelData.MobileNo = worksheet.Cells[row, 5].Value.ToString();
                        excelData.WhatsAppNo = worksheet.Cells[row, 6].Value.ToString();
                        excelData.EmailId = worksheet.Cells[row, 7].Value.ToString();
                        excelData.UserName = worksheet.Cells[row, 7].Value.ToString();
                        excelData.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(worksheet.Cells[row, 8].Value.ToString()));
                        excelData.PasswordSalt = hmac.Key;
                        excelData.Gender = worksheet.Cells[row, 9].Value.ToString();
                        excelData.RoleName = ApplicationRole.Customer;
                        excelData.CreatedById = userId;
                        excelData.CreatedDate = DateTime.Now;
                        excelData.BranchId = _branchRepo.GetBranchByName(worksheet.Cells[row, 10].Value.ToString()).Id;
                        excelData.IsActive = bool.Parse(worksheet.Cells[row, 11].Value.ToString());
                        excelData.LoanAppAccountNo = GenerateAccount(DateTime.Parse(worksheet.Cells[row, 4].Value.ToString()));
                        excelDataList.Add(excelData);
                    }
                    var result = _customerRepo.SaveMultipleCustomer(excelDataList);

                    // Process the excelDataList as needed (e.g., save to a database)
                    return Json(result);
                }
            }

            return Json("Fail");
        }

        private string GenerateAccount(DateTime date)
        {
            Random random = new Random();
            string accountNumber = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + random.Next(100000, 9999999).ToString();
            return accountNumber;
        }

    }
}