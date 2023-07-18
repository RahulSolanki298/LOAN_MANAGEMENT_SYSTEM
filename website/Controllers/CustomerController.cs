using System;
using System.Linq;
using System.Web.Mvc;
using website.Dto;
using website.Interface;

namespace website.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        public readonly ICustomerRepository _customerRepo;
        public CustomerController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CustomerRegistration(int id = 0)
        {
            if (id > 0)
            {
                var response = _customerRepo.GetCustomerPrimaryData(id);
                return View(response);
            }
            return View(new CustomerRegistrationDTO());
        }

        public ActionResult ActivatedCustomerList()
        {
            var branchId = 0;
            var response = _customerRepo.GetActiveCustomers();

            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }
            if (branchId > 0)
            {
                var list = response.Where(x => x.BranchId == branchId).ToList();
                return View(list);
            }
            return View(response);
        }

        public ActionResult DeactivedCustomerList()
        {
            var branchId = 0;
            var response = _customerRepo.GetNonActiveCustomers();

            if (Session["BranchId"] != null)
            {
                branchId = Convert.ToInt32(Session["BranchId"].ToString());
            }
            if (branchId > 0)
            {
                var list = response.Where(x => x.BranchId == branchId).ToList();
                return View(list);
            }
            return View(response);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int id)
        {

            try
            {
                var resp = _customerRepo.DeleteCustomer(id);
                return Json("deleted");
            }
            catch (Exception ex)
            {

                return Json($"Exp:{ex.Message}");
            }

        }
    }
}