using System;
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

        


    }
}