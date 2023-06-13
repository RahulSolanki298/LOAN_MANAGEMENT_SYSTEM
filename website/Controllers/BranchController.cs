using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website.Dto;
using website.Interface;
using website.Models;

namespace website.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {
        public readonly IBranchRepository _branchRepo;
        public BranchController(IBranchRepository branchRepo)
        {
            _branchRepo = branchRepo;
        }

        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBranchList()
        {
            var response = _branchRepo.GetBranchList();
            return View(response);
        }

        public ActionResult GetBranchAdminUserIndex()
        {
            return View();
        }

        public ActionResult GetAdminBranchList()
        {
            var response = _branchRepo.getBranchAdminList();
            return View(response);
        }

        public ActionResult CreateBranch(int? id = 0)
        {
            if (id > 0)
            {
                var branchDT = _branchRepo.GetBranchDetail((int)id);
                return View(branchDT);
            }
            return View(new CompanyBranchDetail());
        }

        [HttpPost]
        public ActionResult CreateBranch(CompanyBranchDetail companyBranchDetail, 
            IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int index = 1;

                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                string subPath = "~/App_Data/uploads"; // Your code goes here

                                bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                                if (!exists)
                                    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath(subPath), fileName);
                                file.SaveAs(path);
                                if (index == 1)
                                {
                                    companyBranchDetail.Certificate = fileName;
                                }
                                if (index == 2)
                                {
                                    companyBranchDetail.CompanyLogo = fileName;
                                }
                            }
                            index++;
                        }
                    }

                    var response = _branchRepo.SaveBranchDetail(companyBranchDetail);
                    TempData["Success"] = "Branch successfully saved.";

                    return RedirectToAction("CreateBranch");
                }
                TempData["Error"] = "Please enter require fields.";
                return View();

            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Branch could not be added..Error is {ex.Message}";
                return View();
            }
        }

        public ActionResult CreateBranchAdmin(int? id = 0)
        {
            ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();

            if (id > 0)
            {

                var branchDT = _branchRepo.GetBranchAdmin((int)id);
                return View(branchDT);
            }
            return View(new BranchAdminRegistrationDTO());
        }

        [HttpPost]
        public ActionResult CreateBranchAdmin(BranchAdminRegistrationDTO branchAdmin)
        {
            try
            {
                ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();

                if (ModelState.IsValid)
                {
                    var response = _branchRepo.SaveBranchAdmin(branchAdmin);

                    TempData["Success"] = "Branch admin successfully saved.";
                    return RedirectToAction("CreateBranchAdmin");
                }
                TempData["Error"] = "Please enter require fields.";
                return View();

            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Exception : {ex.Message}";
                return View();
            }
        }

    }
}