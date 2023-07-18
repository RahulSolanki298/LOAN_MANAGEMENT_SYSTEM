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
    public class EmployeeRepository : IEmployeeRepository
    {
        public SqlConnection con;

        public List<EmployeeRegistrationDTO> GetActiveEmployees()
        {
            connection();
            con.Open();

            var employees = SqlMapper.Query<EmployeeRegistrationDTO>(
                              con, "select * from ApplicationEmployee where IsActive=1 order by Id desc").ToList();

            con.Close();

            return employees;
        }

        public DocumentationDTO GetDocumentation(int userId)
        {
            connection();
            con.Open();

            var documenation = SqlMapper.Query<DocumentationDTO>(
                              con, $"select * from AppUserPrivateDetails where UserId={userId}").FirstOrDefault();

            con.Close();

            return documenation;
        }

        public AddressMasterDto GetEmployeeAddress(int userId)
        {
            connection();
            con.Open();

            var empAddrs = SqlMapper.Query<AddressMasterDto>(
                              con, $"select * from AddressMaster where UserId={userId}").FirstOrDefault();

            con.Close();

            return empAddrs;
        }

        public EmployeeRegistrationDTO GetEmployeePrimaryData(int id)
        {
            connection();
            con.Open();

            var employee = SqlMapper.Query<EmployeeRegistrationDTO>(
                              con, $"select * from ApplicationEmployee where Id={id}").FirstOrDefault();

            con.Close();

            return employee;
        }

        public List<EmployeeRegistrationDTO> GetNonActiveEmployees()
        {
            connection();
            con.Open();

            var employees = SqlMapper.Query<EmployeeRegistrationDTO>(
                              con, "select * from ApplicationEmployee where IsActive=0 order by Id desc").ToList();

            con.Close();

            return employees;
        }

        public int SaveEmployees(EmployeeRegistrationDTO employee)
        {
            connection();
            con.Open(); 
            var hmac = new HMACSHA512();

            var response = SqlMapper.Query<EmployeeRegistrationDTO>(
                              con, $"select * from ApplicationEmployee where EmailId='{employee.EmailId}' or MobileNo='{employee.MobileNo}'").ToList();

            if (response.Count >0 && employee.Id == 0)
            {
                return 0;
            }

            if (employee.Id == 0)
            {
                employee.Password = employee.FirstName + "@" + DateTime.Now.Year.ToString();
            }
            else
            {
                employee.Password = "NO-CHANGE";
            }
            var param = new DynamicParameters();
            param.Add("@Id", employee.Id > 0 ? employee.Id : 0);
            param.Add("@UserName", employee.EmailId);
            param.Add("@FirstName", employee.FirstName);
            param.Add("@MiddleName", employee.MiddleName);
            param.Add("@LastName", employee.LastName);
            param.Add("@Gender", employee.Gender);  
            param.Add("@DateOfBirth", employee.DateOfBirth);
            param.Add("@MobileNo", employee.MobileNo);
            param.Add("@WhatsAppNo", employee.WhatsAppNo);
            param.Add("@EmailId", employee.EmailId);
            param.Add("@BranchId", employee.BranchId);
            param.Add("@IsActive", employee.IsActive);
            param.Add("@PasswordHash", hmac.ComputeHash(Encoding.UTF8.GetBytes(employee.Password)));
            param.Add("@PasswordSalt", hmac.Key);
            param.Add("@EmployeeTitle", employee.EmployeeTitle);
            param.Add("@EmployeeSalary", employee.EmployeeSalary);

            var EmployeeId = con.ExecuteScalar("SP_EmployeeCreation", param, commandType: CommandType.StoredProcedure);

            con.Close();

            int customerId = Convert.ToInt32(EmployeeId);

            return customerId;
        }

        public int SaveEmployeesAddress(AddressMasterDto address)
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

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        
    }
}