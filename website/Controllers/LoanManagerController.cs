using System.Web.Mvc;
using website.Interface;

namespace website.Controllers
{
    [Authorize]
    public class LoanManagerController : Controller
    {
        public readonly ILoanManagerRepository _customerLoanRepo;

        public LoanManagerController(ILoanManagerRepository customerLoanRepo)
        {
            _customerLoanRepo = customerLoanRepo;
        }

        // GET: LoanManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoanAppliedList()
        {
            var response = _customerLoanRepo.GetCustomerLoan();

            return View(response);
        }
    }
}