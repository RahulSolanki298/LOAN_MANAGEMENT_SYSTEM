using Dapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using System;
using System.Configuration;
using System.Data;
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

        public bool ChangePassword(ChangePasswordDTO data)
        {
            try
            {
                connection();
                con.Open();

                var user = SqlMapper.Query<ApplicationUserDTO>(
                                  con, "GetUserData").Where(x => x.Id == data.UserId).FirstOrDefault();

                if (user == null)
                {
                    return false;
                }
                var hmac = new HMACSHA512(user.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data.OldPassword));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i]) return false;
                }

                if (data.NewPassword != data.ConfirmPassword) return false;

                hmac = new HMACSHA512();

                var passVal = hmac.ComputeHash(Encoding.UTF8.GetBytes(data.NewPassword));
                var passKey = hmac.Key;

                var param = new DynamicParameters();
                param.Add("@Id", data.UserId);
                param.Add("@PasswordHash", passVal);
                param.Add("@PasswordSalt", passKey);

                var response = con.ExecuteScalar("SP_ChangePassword", param, commandType: CommandType.StoredProcedure);

                con.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}