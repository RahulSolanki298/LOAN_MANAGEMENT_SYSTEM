using System;
using System.Linq;
using System.Web.Mvc;
using website.Dto;
using website.Helpers;
using website.Interface;

namespace website.Controllers
{
    [Authorize]
    public class LoanManagerController : Controller
    {
        public readonly ILoanManagerRepository _customerLoanRepo;
        public readonly IBranchRepository _branchRepo;
        public readonly ICustomerRepository _customerRepo;

        public LoanManagerController(ILoanManagerRepository customerLoanRepo,
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
            return View();
        }

        #region Loan Status wise List
        /// <summary>
        /// List Management
        /// </summary>
        /// <returns></returns>

        public ActionResult LoanAppliedList()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            string roleName = Session["RoleName"].ToString();

            var response = _customerLoanRepo.GetCustomersLoan(userId, branchId, LoanStages.Applied, roleName);

            return View(response);
        }

        public ActionResult LoanPassedList()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            string roleName = Session["RoleName"].ToString();

            var response = _customerLoanRepo.GetCustomersLoan(userId, branchId, LoanStages.Passed, roleName);

            return View(response);
        }

        public ActionResult CustomerFailed()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            string roleName = Session["RoleName"].ToString();

            var response = _customerLoanRepo.GetCustomersLoan(userId, branchId, LoanStages.Failed, roleName);

            return View(response);
        }

        public ActionResult CustomerActive()
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

        public ActionResult CustomerCancel()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            string roleName = Session["RoleName"].ToString();

            var response = _customerLoanRepo.GetCustomersLoan(userId, branchId, LoanStages.Cancel, roleName);

            return View(response);
        }

        public ActionResult CustomerComplated()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            string roleName = Session["RoleName"].ToString();

            var response = _customerLoanRepo.GetCustomersLoan(userId, branchId, LoanStages.Complated, roleName);

            return View(response);
        }

        #endregion

        public ActionResult LoanApply(int? id = 0, int? userId = 0)
        {
            ViewBag.BranchList = _branchRepo.GetBranchList();
            CustomerLoanManagerDTO data = new CustomerLoanManagerDTO();
            ViewBag.Title = "Loan Apply";
            if (userId != 0)
            {
                data.UserId = userId;
                data.LoanAccNo = _customerRepo.GetCustomerPrimaryData(Convert.ToInt32(userId)).LoanAppAccountNo;
            }

            if (id > 0)
            {
                ViewBag.Title = "Modify Loan Apply";
                data = _customerLoanRepo.GetCustomerLoanData((int)id);
            }

            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }



            return View(data);
        }

        [HttpPost]
        public ActionResult SubmitLoanApply(LoanApplyDTO loanApplyDTO)
        {
            try
            {
                ViewBag.BranchList = _branchRepo.GetBranchList().Select(x => new { x.Id, x.Title }).ToList();
                CustomerLoanManagerDTO data = new CustomerLoanManagerDTO(); ;
                data.Id = loanApplyDTO.Id ?? 0;
                data.NoOfDays = loanApplyDTO.NoOfDays;
                data.LoanEMI = loanApplyDTO.LoanEMI;
                data.UserId = loanApplyDTO.UserId;
                data.LoanAccNo = loanApplyDTO.LoanAccNo;
                data.LoanApplyAmount = loanApplyDTO.LoanApplyAmount;
                data.LoanNetAmount = loanApplyDTO.LoanNetAmount;
                data.LoanIntrest = loanApplyDTO.LoanIntrest;
                data.BranchId = loanApplyDTO.BranchId;
                data.LoanApplyAmountDate = DateTime.Now;
                data.LoanNo = GenerateLoanNo();
                var response = _customerLoanRepo.SaveCustomerLoanApply(data);

                TempData["Success"] = "Loan has been applied..";
                return Json("Index");

            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Exception : {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        public ActionResult PassLoanApplication(int id, string status)
        {
            var result = _customerLoanRepo.UpdateLoanStatus(id, status);

            return Json($"Customer({result}) loan has been passed.");
        }

        [HttpGet]
        public ActionResult SectionLetter(int id)
        {
            int branchId = 0;
            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }

            var response = _customerLoanRepo.GetCustomerLoan(id);

            return View("~/Views/LoanManager/section.cshtml", response);
        }

        [HttpGet]
        public ActionResult SubmitSectionLetter(int id, string status)
        {
            var result = _customerLoanRepo.UpdateLoanStatus(id, status);

            return Json($"Customer({result}) loan has been passed.");
        }

        [HttpGet]
        public ActionResult GetEMIReportByLoanNo(string loanNo,string loanAccountNo) 
        {
            LoanCardDetails loanCardDetails = new LoanCardDetails();

            loanCardDetails = _customerLoanRepo.GetCardWithLoanNo(loanNo,loanAccountNo);

            return View(loanCardDetails);
        }

        private string GenerateLoanNo()
        {
            var random = new Random();
            var loanNo = DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + random.Next(1000, 9999).ToString();
            return loanNo;
        }
    }

}