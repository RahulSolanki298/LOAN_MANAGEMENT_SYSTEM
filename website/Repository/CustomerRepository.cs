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

        public bool SaveCustomer(CustomerRegistrationDTO customer)
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
            param.Add("@LoanAppAccountNo", GenerateAccount());

            con.ExecuteScalar("SP_CustomerCreation", param, commandType: CommandType.StoredProcedure);

            con.Close();

            return true;
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

        public bool SaveCustomerAddress(AddressMasterDto address)
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

            con.ExecuteScalar("SP_SaveUserAddress", param, commandType: CommandType.StoredProcedure);

            con.Close();

            return true;
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

        public bool SaveDocumentation(DocumentationDTO documenation)
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

            con.ExecuteScalar("SP_DocumentationCreation", param, commandType: CommandType.StoredProcedure);

            con.Close();

            return true;
        }

        #endregion

        private string GenerateAccount()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            return $"LOAN_ACC_{DateTime.Now.Date + "" + randomNumber}";
        }

    }
}