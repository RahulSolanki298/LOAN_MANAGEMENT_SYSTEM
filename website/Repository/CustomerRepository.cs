using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using website.Dto;
using website.Interface;

namespace website.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        public bool DeleteCustomer(int id)
        {
            throw new System.NotImplementedException();
        }

        public CustomerRegistrationDTO GetCustomerPrimaryData(int id)
        {
            connection();
            con.Open();

            var customer = SqlMapper.Query<CustomerRegistrationDTO>(
                              con, $"select * from ApplicationCustomer where Id={id}").FirstOrDefault();

            con.Close();

            return customer;
        }

        public List<CustomerRegistrationDTO> GetActiveCustomers()
        {
            connection();
            con.Open();

            var customers = SqlMapper.Query<CustomerRegistrationDTO>(
                              con, "select * from ApplicationCustomer where IsActive=1 order by Id desc").ToList();

            con.Close();

            return customers;
        }

        public List<CustomerRegistrationDTO> GetNonActiveCustomers()
        {
            connection();
            con.Open();

            var customers = SqlMapper.Query<CustomerRegistrationDTO>(
                              con, "select * from ApplicationCustomer where IsActive=0 order by Id desc").ToList();

            con.Close();

            return customers;
        }

        public string SaveCustomer()
        {
            throw new System.NotImplementedException();
        }
    }
}