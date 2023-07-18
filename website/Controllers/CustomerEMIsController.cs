using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using website.Helpers;
using website.Interface;

namespace website.Controllers
{
    [Authorize]
    public class CustomerEMIsController : Controller
    {
        public readonly ILoanManagerRepository _customerLoanRepo;
        public readonly IBranchRepository _branchRepo;
        public readonly ICustomerRepository _customerRepo;

        public CustomerEMIsController(ILoanManagerRepository customerLoanRepo,
            IBranchRepository branchRepo,
            ICustomerRepository customerRepo)
        {
            _customerLoanRepo = customerLoanRepo;
            _branchRepo = branchRepo;
            _customerRepo = customerRepo;
        }


        // GET: LoanManager
        public ActionResult Index()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            string roleName = Session["RoleName"].ToString();

            var response = _customerLoanRepo.GetCustomersLoan(userId, branchId, LoanStages.Active, roleName);

            return View(response);
        }

        public ActionResult DayBook()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var branchId = 0;   
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }
            ViewBag.RoleName = Session["RoleName"].ToString();
            ViewBag.BranchList = _branchRepo.GetBranchList();
            ViewBag.Branch = branchId;

            return View();
        }

        public ActionResult GetDaybookData(int? branchId=0)
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            string roleName = Session["RoleName"].ToString();
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            var response = _customerLoanRepo.GetLoanReceableDayBook(userId, (int)branchId, LoanStages.Active, roleName);

            return View("~/Views/CustomerEMIs/View.cshtml", response);
        }

        [HttpPost]
        public ActionResult SaveSelectedItems(List<int> selectedIds, string paidBy)
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }
            string roleName = Session["RoleName"].ToString();

            var response =_customerLoanRepo.SaveEMIForMultipleUser(selectedIds, paidBy,branchId,roleName);

            return Json(response);
        }

    }
}