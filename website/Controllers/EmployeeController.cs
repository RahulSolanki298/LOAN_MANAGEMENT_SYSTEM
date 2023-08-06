using System.IO;
using System.Linq;
using System;
using System.Web.Mvc;
using website.Dto;
using website.Helpers;
using website.Interface;

namespace website.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        public readonly IEmployeeRepository _empRepo;
        public readonly IBranchRepository _branchRepo;

        public EmployeeController(IEmployeeRepository employee, IBranchRepository branchRepo)
        {
            _empRepo = employee;
            _branchRepo = branchRepo;
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EmployeeRegistration(int id = 0)
        {
            if (id > 0)
            {
                var response = _empRepo.GetEmployeePrimaryData(id);
                return View(response);
            }
            return View(new EmployeeRegistrationDTO());
        }

        public ActionResult ActivatedEmployeeList()
        {
            var response = _empRepo.GetActiveEmployees();
            return View(response);
        }

        public ActionResult DeactivedEmployeeList()
        {
            var response = _empRepo.GetNonActiveEmployees();
            return View(response);
        }

        public ActionResult EmployeeIndex(int? id = 0)
        {
            ViewBag.UserId = id;
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();

            return View("~/Views/EmployeeRegistration/Index.cshtml", new EmployeeRegistrationDTO());
        }

        [HttpGet]
        public ActionResult CreateRegister(string id = "0")
        {
            var Response = new EmployeeRegistrationDTO();
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();
            var usrId = Convert.ToInt32(id);
            if (usrId > 0)
            {
                Response = _empRepo.GetEmployeePrimaryData(usrId);
                return View("~/Views/EmployeeRegistration/EmployeeRegistration.cshtml", Response);
            }

            return View("~/Views/EmployeeRegistration/EmployeeRegistration.cshtml", Response);
        }

        [HttpPost]
        public ActionResult CreateRegister(EmployeeRegistrationDTO employee)
        {
            try
            {
                ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();

                if (ModelState.IsValid)
                {
                    employee.RoleName = ApplicationRole.Employee;
                    var id = _empRepo.SaveEmployees(employee);

                    TempData["Success"] = "Employee personal detail saved successfully.";
                    return RedirectToAction("Index", new { id = id });
                }
                TempData["Error"] = "Please enter employee registration.";
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
                Response = _empRepo.GetEmployeeAddress(userId);

                return View("~/Views/EmployeeRegistration/CreateAddressDetails.cshtml", Response);
            }

            return View("~/Views/EmployeeRegistration/CreateAddressDetails.cshtml", Response);
        }

        [HttpPost]
        public ActionResult CreateAddressDetails(AddressMasterDto address)
        {

            var Response = _empRepo.SaveEmployeesAddress(address);
            TempData["Success"] = "Employee address saved successfully.";
            return RedirectToAction("Index", new { userId = Response });
        }

        [HttpGet]
        public ActionResult CreateDocumentation(int userId = 0)
        {
            var Response = new DocumentationDTO();
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();
            if (userId > 0)
            {
                Response = _empRepo.GetDocumentation(userId);
                if (Response != null)
                {
                    return View("~/Views/EmployeeRegistration/CreateDocumentation.cshtml", Response);
                }

                return View("~/Views/EmployeeRegistration/CreateDocumentation.cshtml", new DocumentationDTO());
            }

            return View("~/Views/EmployeeRegistration/CreateDocumentation.cshtml", Response);
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

            var Response = _empRepo.SaveDocumentation(documentation);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateAccountDetails(int userId = 0)
        {
            var Response = new EmployeeRegistrationDTO();
            var usrId = Convert.ToInt32(userId);
            if (usrId > 0)
            {
                Response = _empRepo.GetEmployeePrimaryData(usrId);

                if (Response == null)
                {
                    Response.UserId = userId;
                }

                return View("~/Views/EmployeeRegistration/CreateAccountDetails.cshtml", Response);
            }

            return View("~/Views/EmployeeRegistration/CreateAccountDetails.cshtml", Response);
        }

        [HttpPost]
        public ActionResult CreateAccountDetails(EmployeeRegistrationDTO register)
        {

            var response = _empRepo.SaveEmployees(register);

            return RedirectToAction("Index");
        }
    }
}