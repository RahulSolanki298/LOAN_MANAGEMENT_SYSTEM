using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using website.Dto;

namespace website.Helpers
{
    public class SeedData
    {
        public SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        public void checkIsSeed()
        {
            connection();
            con.Open();
            var hmac = new HMACSHA512();
            var userDT = SqlMapper.Query<CustomerLoanManagerDTO>(
                              con, "select * from AppUser").ToList();

            if (userDT.Count == 0)
            {
                var UserName = "singhfinance";
                var FirstName = "Rahul";
                var MiddleName = "";
                var LastName = "Singh";
                var Gender = "Male";
                var Password = "Rahul@2023";
                var EmailId = "singhmutualnidhilimited@gmail.com";

                var param = new DynamicParameters();
                param.Add("@UserName", UserName);
                param.Add("@FirstName", FirstName);
                param.Add("@MiddleName", MiddleName);
                param.Add("@LastName", LastName);
                param.Add("@Gender", Gender);
                param.Add("@PasswordHash", hmac.ComputeHash(Encoding.UTF8.GetBytes(Password)));
                param.Add("@PasswordSalt", hmac.Key);
                param.Add("@EmailId", EmailId);

                con.ExecuteScalar("SP_SeedData", param, commandType: CommandType.StoredProcedure);
            }

            con.Close();
        }
    }
}