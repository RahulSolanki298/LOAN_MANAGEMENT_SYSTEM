using System.Linq;
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
        public ActionResult CreateBranch(CompanyBranchDetail companyBranchDetail)
        {
            try
            {
                var response = _branchRepo.SaveBranchDetail(companyBranchDetail);
                TempData["Success"] = "Branch successfully saved.";

                return RedirectToAction("CreateBranch");
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = $"Exception : {ex.Message}";
                TempData["Error"] = "Branch successfully saved.";
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
                var response = _branchRepo.SaveBranchAdmin(branchAdmin);

                TempData["Success"] = "Branch admin successfully saved.";
                return RedirectToAction("CreateBranchAdmin");

            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Exception : {ex.Message}";
                return View();
            }
        }


    }
}