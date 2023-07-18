using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using website.Dto;

namespace website.APIControllers
{
    public class CustomerAPIController : ApiController
    {
        public SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        // GET: CustomerAPI
        [Route("api/CustomerAPI/active-customers")]
        [HttpGet]
        public List<CustomerRegistrationDTO> ActiveCustomers()
        {
            connection();
            con.Open();

            var customers = SqlMapper.Query<CustomerRegistrationDTO>(
                              con, "select * from ApplicationCustomer where IsActive=1 order by Id desc").ToList();

            con.Close();

            return customers;
        }
    }
}