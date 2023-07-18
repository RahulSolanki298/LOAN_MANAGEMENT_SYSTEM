using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using website.Dto;
using website.Helpers;
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
            connection();
            con.Open();

            var result = SqlMapper.Query<CustomerLoanManagerDTO>(
                            con, $"select * from CustomerLoanManager where Id={id}").FirstOrDefault();

            if (result == null)
            {
                con.Execute($"Delete FROM AppUserRole WHERE Id={id}");
                con.Execute($"Delete FROM AddressMaster WHERE Id={id}");
                con.Execute($"Delete FROM AppUserPrivateDetails WHERE Id={id}");
                con.Execute($"Delete FROM AppUser WHERE Id={id}");
                return true;
            }

            con.Close();
            return false;

        }

        public DashboardDTO Dashboard()
        {
            connection();
            con.Open();

            DashboardDTO dashboardDTO = new DashboardDTO();

            dashboardDTO.ActiveClientNo = SqlMapper.Query<int>(
                              con, $"select count(*) as ActiveClientNo from ApplicationCustomer where IsActive='1'").FirstOrDefault();


            dashboardDTO.DeActiveClientNo = SqlMapper.Query<int>(
                              con, $"select count(*) as DeActiveClientNo from ApplicationCustomer where IsActive='0'").FirstOrDefault();

            dashboardDTO.TodayLoanApply = SqlMapper.Query<int>(
                              con, $"select count(*) from CustomerLoanManager cms" +
                              $" Inner join AppUserRole aur on cms.UserId = aur.UserId" +
                              $" Inner join LoanStages ls on aur.RoleId = ls.Id" +
                              $" where ls.Name='{LoanStages.Applied}'").FirstOrDefault();

            dashboardDTO.ComplateLoan = SqlMapper.Query<int>(
                              con, $"select count(*) from CustomerLoanCard where RepaymentDate='{DateTime.Now}'").FirstOrDefault();


            con.Close();

            return dashboardDTO;
        }

        #region Customer Primary Data

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

        public int SaveCustomer(CustomerRegistrationDTO customer)
        {
            connection();
            con.Open();
            var hmac = new HMACSHA512();

            if (customer.Id == 0)
            {
                customer.Password = customer.FirstName + "@" + DateTime.Now.Year.ToString();
            }
            else
            {
                customer.Password = "NO-CHANGE";
            }

            var param = new DynamicParameters();
            param.Add("@Id", customer.Id > 0 ? customer.Id : 0);
            param.Add("@UserName", customer.EmailId);
            param.Add("@FirstName", customer.FirstName);
            param.Add("@MiddleName", customer.MiddleName);
            param.Add("@LastName", customer.LastName);
            param.Add("@Gender", customer.Gender);
            param.Add("@DateOfBirth", customer.DateOfBirth);
            param.Add("@MobileNo", customer.MobileNo);
            param.Add("@WhatsAppNo", customer.WhatsAppNo);
            param.Add("@EmailId", customer.EmailId);
            param.Add("@BranchId", customer.BranchId);
            param.Add("@IsActive", customer.IsActive);
            param.Add("@PasswordHash", hmac.ComputeHash(Encoding.UTF8.GetBytes(customer.Password)));
            param.Add("@PasswordSalt", hmac.Key);
            param.Add("@LoanAppAccountNo", GenerateAccount((DateTime)customer.DateOfBirth));

            var response = con.ExecuteScalar("SP_CustomerCreation", param, commandType: CommandType.StoredProcedure);

            con.Close();

            int customerId = Convert.ToInt32(response);

            return customerId;
        }

        public string SaveMultipleCustomer(List<ApplicationUserDTO> customers)
        {
            try
            {
                connection();
                con.Open();
                ApplicationUserDTO dt = new ApplicationUserDTO();
                List<ApplicationUserDTO> InsertCustomers = new List<ApplicationUserDTO>();
                List<ApplicationUserDTO> UpdateCustomers = new List<ApplicationUserDTO>();

                foreach (var customer in customers)
                {
                    dt = SqlMapper.Query<ApplicationUserDTO>(
                              con, $"select * from AppUser where EmailId='{customer.EmailId}'").FirstOrDefault();

                    if (dt == null)
                    {
                        InsertCustomers.Add(customer);
                    }

                }

                var trans = con.BeginTransaction();

                var response = con.Execute(@"INSERT INTO AppUser (UserName, FirstName, MiddleName,LastName,Gender,DateOfBirth,MobileNo,WhatsAppNo,EmailId,CreatedById,CreatedDate,BranchId,IsActive,PasswordHash,PasswordSalt,LoanAppAccountNo)" +
                                "values(@UserName, @FirstName, @MiddleName,@LastName,@Gender,@DateOfBirth,@MobileNo,@WhatsAppNo,@EmailId,@CreatedById,@CreatedDate,@BranchId,@IsActive,@PasswordHash,@PasswordSalt,@LoanAppAccountNo)", InsertCustomers, transaction: trans);

                trans.Commit();
                con.Close();

                con.Open();

                foreach (var customer in InsertCustomers)
                {
                    dt = SqlMapper.Query<ApplicationUserDTO>(
                              con, $"select * from AppUser where EmailId='{customer.EmailId}'").FirstOrDefault();

                    var dt2 = SqlMapper.Query<int>(
                             con, $"select Id from AppRole where Name='{customer.RoleName}'").FirstOrDefault();

                    string sqlQuery1 = $"Insert into AppUserRole(UserId,RoleId)values({dt.Id},{dt2})";
                    con.Execute(sqlQuery1);
                }

                con.Close();
                return "Success";
            }
            catch (Exception ex)
            {

                return $"Fail with Ex: {ex.Message}";
            }

        }

        #endregion

        #region Customer Address

        public AddressMasterDto GetCustomerAddress(int userId)
        {
            connection();
            con.Open();

            var customerAddrs = SqlMapper.Query<AddressMasterDto>(
                              con, $"select * from AddressMaster where UserId={userId}").FirstOrDefault();

            con.Close();

            return customerAddrs;
        }

        public int SaveCustomerAddress(AddressMasterDto address)
        {
            connection();
            con.Open();

            var param = new DynamicParameters();
            param.Add("@Id", address.Id > 0 ? address.Id : 0);
            param.Add("@UserId", address.UserId);
            param.Add("@AddressLine1", address.AddressLine1);
            param.Add("@AddressLine2", address.AddressLine2);
            param.Add("@City", address.City);
            param.Add("@State", address.State);
            param.Add("@Country", address.Country);
            param.Add("@ZipCode", address.ZipCode);
            param.Add("@AddressFor", address.AddressFor);

            var response = con.ExecuteScalar("SP_SaveUserAddress", param, commandType: CommandType.StoredProcedure);

            con.Close();

            int addressId = Convert.ToInt32(response);

            return addressId;
        }

        #endregion


        #region Customer Documentation

        public DocumentationDTO GetDocumentation(int userId)
        {
            connection();
            con.Open();

            var documenation = SqlMapper.Query<DocumentationDTO>(
                              con, $"select * from AppUserPrivateDetails where UserId={userId}").FirstOrDefault();

            con.Close();

            return documenation;
        }

        public int SaveDocumentation(DocumentationDTO documenation)
        {
            connection();
            con.Open();

            var param = new DynamicParameters();
            param.Add("@Id", documenation.Id > 0 ? documenation.Id : 0);
            param.Add("@UserId", documenation.UserId);
            param.Add("@AadharCardNo", documenation.AadharCardNo);
            param.Add("@AadharCardImagePath", documenation.AadharCardImagePath);
            param.Add("@PancardNo", documenation.PancardNo);
            param.Add("@PancardImagePath", documenation.PancardImagePath);
            param.Add("@BankName", documenation.BankName);
            param.Add("@BankAccountNo", documenation.BankAccountNo);
            param.Add("@IFSCode", documenation.IFSCode);
            param.Add("@PassBookImgPath", documenation.PassBookImgPath);
            param.Add("@CheckBooKImgPath", documenation.CheckBooKImgPath);
            param.Add("@ProfileImgPath", documenation.ProfileImgPath);

            var response = con.ExecuteScalar("SP_DocumentationCreation", param, commandType: CommandType.StoredProcedure);

            con.Close();
            int documentId = Convert.ToInt32(response);
            return documentId;
        }

        #endregion

        private string GenerateAccount(DateTime date)
        {
            Random random = new Random();
            string accountNumber = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + random.Next(100000, 9999999).ToString();
            return accountNumber;
        }

    }
}