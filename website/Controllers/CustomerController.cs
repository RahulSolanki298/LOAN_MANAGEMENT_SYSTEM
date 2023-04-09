using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using website.Dto;
using website.Helpers;
using website.Interface;
using website.Models;

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
            var response = _customerRepo.GetActiveCustomers();
            return View(response);
        }

        public ActionResult DeactivedCustomerList()
        {
            var response = _customerRepo.GetNonActiveCustomers();
            return View(response);
        }
    }
}