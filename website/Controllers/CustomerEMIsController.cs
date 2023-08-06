using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using website.Dto;
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

        public ActionResult GetDaybookData(int? branchId = 0)
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            string roleName = Session["RoleName"].ToString();
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            var response = _customerLoanRepo.GetLoanReceableDayBook(userId, (int)branchId, LoanStages.Active, roleName);

            var result = new List<CustomerLoanCardDto>();
            if (branchId > 0)
            {
                result = response.Where(x => x.BranchId == branchId).ToList();
                return View("~/Views/CustomerEMIs/View.cshtml", result);
            }

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

            var response = _customerLoanRepo.SaveEMIForMultipleUser(selectedIds, paidBy, branchId, roleName, userId);

            return Json(response);
        }

        [HttpGet]
        public ActionResult OtherWayForPayment(int id)
        {
            var result = _customerLoanRepo.getLoanDaywise(id);

            return View("~/Views/CustomerEMIs/CustomerPayment.cshtml", result);
        }

        [HttpPost]
        public ActionResult SubmitCustomPayment(CustomerLoanCardDto loanData)
        {
            var response = "";
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            loanData.EntryBy = userId;
            loanData.EntryDate = loanData.RepaymentDate;
            var result = _customerLoanRepo.CustomEMIPaid(loanData);
            if (result == true)
            {
                response = "EMI added successfully.";
            }
            else
            {
                response = "Something is wrong.";
            }

            return Json(response);
        }

    }
}