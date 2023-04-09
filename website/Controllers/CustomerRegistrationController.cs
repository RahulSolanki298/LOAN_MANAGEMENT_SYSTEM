using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using website.Dto;
using website.Interface;

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
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();
            var usrId = Convert.ToInt32(id);
            if (usrId > 0)
            {
                Response = _customerRepo.GetCustomerPrimaryData(usrId);
                return View(Response);
            }

            return View(Response);
        }

        [HttpPost]
        public ActionResult CreateRegister(CustomerRegistrationDTO customer)
        {
            try
            {
                ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();

                if (ModelState.IsValid)
                {
                    var response = _customerRepo.SaveCustomer(customer);

                    TempData["Success"] = "Customer personal detail saved successfully.";
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Please enter customer registration.";
                return RedirectToAction("Index");

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
            return RedirectToAction("Index");
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
                string path = Server.MapPath("~/App_Data/AadharCards");
                string adharFileName = Path.GetFileName(documentation.AadharCardImagePathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + adharFileName.Split('.').Last();

                string adharFullPath = Path.Combine(path, renameFile);
                documentation.AadharCardImagePathFile.SaveAs(adharFullPath);
                documentation.AadharCardImagePath = renameFile;
            }

            if (documentation.PancardImagePathFile != null)
            {
                string path = Server.MapPath("~/App_Data/Pancards");
                string pancardFileName = Path.GetFileName(documentation.PancardImagePathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + pancardFileName.Split('.').Last();

                string pancardFullPath = Path.Combine(path, renameFile);
                documentation.PancardImagePathFile.SaveAs(pancardFullPath);
                documentation.PancardImagePath = renameFile;
            }

            if (documentation.PassBookImgPathFile != null)
            {
                string path = Server.MapPath("~/App_Data/PassBooks");
                string passbookFileName = Path.GetFileName(documentation.PassBookImgPathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + passbookFileName.Split('.').Last();

                string passbookFullPath = Path.Combine(path, renameFile);
                documentation.PassBookImgPathFile.SaveAs(passbookFullPath);
                documentation.PassBookImgPath = renameFile;
            }

            if (documentation.CheckBooKImgPathFile != null)
            {
                string path = Server.MapPath("~/App_Data/CheckBooks");
                string checkBookFileName = Path.GetFileName(documentation.CheckBooKImgPathFile.FileName);
                string renameFile = Convert.ToString(Guid.NewGuid()) + "." + checkBookFileName.Split('.').Last();

                string checkBookFullPath = Path.Combine(path, renameFile);
                documentation.CheckBooKImgPathFile.SaveAs(checkBookFullPath);
                documentation.CheckBooKImgPath = renameFile;
            }

            if (documentation.ProfileImgPathFile != null)
            {
                string path = Server.MapPath("~/App_Data/Profiles");
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

    }
}