using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using website.Dto;
using website.Interface;

namespace website.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public SqlConnection con;
        public readonly ICustomerRepository _customerRepository;
        public string UserRole = string.Empty;
        public HomeController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        public ActionResult Index()
        {
            connection();
            con.Open();

            var result = _customerRepository.Dashboard();


            con.Close();
            return View(result);
        }

        public ActionResult DaybookData()
        {
            connection();
            con.Open();

            var CustomerLoanList = SqlMapper.Query<CustomerLoanCardDto>(
                              con, $"select usr.FirstName +' '+usr.LastName as CustomerName,clc.* from CustomerLoanCard clc inner join AppUser usr on clc.LoanAccNo = usr.LoanAppAccountNo where RepaymentDate='{DateTime.Now}'").ToList();

            con.Close();
            return View(CustomerLoanList);
        }

        public ActionResult customerList()
        {
            var result = _customerRepository.GetActiveCustomers();

            return View(result);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}