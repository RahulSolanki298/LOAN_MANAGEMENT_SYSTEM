using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using website.Dto;
using website.Interface;

namespace website.Repository
{
    public class LoanManagerRepository : ILoanManagerRepository
    {
        public SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        public List<CustomerLoanManagerDTO> GetCustomerLoan()
        {
            connection();
            con.Open();

            var customers = SqlMapper.Query<CustomerLoanManagerDTO>(
                              con, "select * from CustomerLoanManager order by Id desc").ToList();

            con.Close();

            return customers;
        }
    }
}