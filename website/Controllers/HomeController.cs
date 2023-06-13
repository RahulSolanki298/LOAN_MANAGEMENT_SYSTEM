using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
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

             var result=_customerRepository.Dashboard();


            con.Close();
            return View(result);
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