using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using website.Dto;
using website.Interface;

namespace website.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }
        public ApplicationUserDTO LoginProcess(LoginDTO loginDT)
        {
            connection();
            con.Open();

            var user = SqlMapper.Query<ApplicationUserDTO>(
                              con, "GetUserData").Where(x => x.UserName == loginDT.UserName).FirstOrDefault();

            con.Close();

            if (user == null)
            {
                return new ApplicationUserDTO();
            }
            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDT.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return new ApplicationUserDTO();
            }

            return user;
        }

    }
}