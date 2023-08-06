using System;
using System.Linq;
using System.Web.Mvc;
using website.Dto;
using website.Interface;
using website.Repository;

namespace website.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        public readonly ICustomerRepository _customerRepo;
        public readonly IAccountRepository _accountRepository;
        public CustomerController(ICustomerRepository customerRepo, IAccountRepository accountRepository)
        {
            _customerRepo = customerRepo;
            _accountRepository = accountRepository;
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

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View("~/Views/Account/ChangePassword.cshtml", new ChangePasswordDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"].ToString());
                model.UserId = userId;


                bool passwordChanged = _accountRepository.ChangePassword(model);

                if (passwordChanged)
                {
                    ViewBag.Message = "Password changed successfully.";
                }
                else
                {
                    ViewBag.Message = "Failed to change the password. Please try again.";
                }
            }

            return View("~/Views/Account/ChangePassword.cshtml", model);
        }
    }
}