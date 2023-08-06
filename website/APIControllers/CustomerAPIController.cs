using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using website.Dto;
using website.Models;

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
        [Route("api/CustomerAPI/active-customers/{branchId}")]
        [HttpGet]
        public List<CustomerRegistrationDTO> ActiveCustomers(int? branchId = 0)
        {
            connection();
            con.Open();

            var customers = new List<CustomerRegistrationDTO>();

            if (branchId == 0)
            {
                return customers;
            }

            customers = SqlMapper.Query<CustomerRegistrationDTO>(
                              con, $"select * from ApplicationCustomer where IsActive=1 and branchId={branchId} order by Id desc").ToList();

            con.Close();

            return customers;
        }

        [Route("api/CustomerAPI/SaveCustomer")]
        [HttpPost]
        public string SaveCustomer([FromBody] CustomerRegistrationDTO customer)
        {
            try
            {
                connection();
                con.Open();
                var hmac = new HMACSHA512();

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

                return "Customer Registration Successfully.";
            }
            catch (Exception ex)
            {

                return $"Exception : {ex.Message}";
            }
        }

        [Route("api/CustomerAPI/get-customer-address/{userId}")]
        [HttpGet]
        public AddressMasterDto GetCustomerAddress(int userId)
        {
            connection();
            con.Open();

            var customerAddrs = SqlMapper.Query<AddressMasterDto>(
                              con, $"select * from AddressMaster where UserId={userId}").FirstOrDefault();

            con.Close();

            return customerAddrs;
        }

        [Route("api/CustomerAPI/SaveAddress")]
        [HttpPost]
        public string SaveCustomerAddress([FromBody] AddressMasterDto address)
        {
            try
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


                return "Address succesully saved.";
            }
            catch (Exception ex)
            {

                return $"Exception : {ex.Message}";
            }
        }

        [Route("api/CustomerAPI/GetDocumentation/{userId}")]
        [HttpGet]
        public DocumentationDTO GetDocumentation(int userId)
        {
            connection();
            con.Open();

            var documenation = SqlMapper.Query<DocumentationDTO>(
                              con, $"select * from AppUserPrivateDetails where UserId={userId}").FirstOrDefault();

            con.Close();

            return documenation;
        }

        [Route("api/CustomerAPI/SaveDocumentation")]
        [HttpPost]
        public string SaveDocumentation([FromBody] DocumentationDTO documenation)
        {
            try
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
                return "";
            }
            catch (Exception ex)
            {

                return $"Exception : {ex.Message}";
            }
        }


        #region Loan Details

        [Route("api/CustomerAPI/GetCustomersLoan/{userId}/{branchId}/{loanStatus}/{loginUserRole}")]
        [HttpGet]
        public List<CustomerLoanManagerDTO> GetCustomersLoan(int userId, int branchId, string loanStatus, string loginUserRole)
        {
            connection();
            con.Open();

            var customers = SqlMapper.Query<CustomerLoanManagerDTO>(
                             con, $"SP_CreateLoanManager {userId},{branchId},'{loanStatus}','{loginUserRole}'").ToList();

            con.Close();

            return customers;
        }

        [Route("api/CustomerAPI/GetCustomerLoanData/{id}")]
        [HttpGet]
        public CustomerLoanManagerDTO GetCustomerLoanData(int id)
        {
            CustomerLoanManagerDTO slData = new CustomerLoanManagerDTO();
            connection();
            con.Open();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", id);

            slData = con.QuerySingleOrDefault<CustomerLoanManagerDTO>("Get_CustomerLoanManager", parameters, commandType: CommandType.StoredProcedure);

            return slData;
        }

        [Route("api/CustomerAPI/GetCustomerLoan/{id}")]
        [HttpGet]
        public SectionLatterDTO GetCustomerLoan(int id)
        {
            SectionLatterDTO slData = new SectionLatterDTO();
            connection();
            con.Open();

            slData.loanData = SqlMapper.Query<CustomerLoanManagerDTO>(
                            con, $"select *,clm.Id,ac.FirstName +''+ac.LastName as CustomerName,cbd.Title from CustomerLoanManager clm " +
                            $"inner join ApplicationCustomer ac on clm.UserId =ac.Id " +
                            $"inner join CompanyBranchDetails cbd on clm.BranchId =cbd.Id " +
                            $"where ac.IsActive=1 and clm.Id={id}").FirstOrDefault();

            slData.customerAddress = SqlMapper.Query<AddressMasterDto>(
                            con, $"select * from AddressMaster where UserId={id}").FirstOrDefault();

            slData.customerData = SqlMapper.Query<ApplicationCustomer>(
                            con, $"select * from ApplicationCustomer " +
                            $"where IsActive=1 and Id={slData.loanData.UserId}").FirstOrDefault();

            slData.branchData = SqlMapper.Query<CompanyBranchDetail>(
                            con, $"select * from CompanyBranchDetails " +
                            $"where IsActivated=1 and Id={slData.loanData.BranchId}").FirstOrDefault();

            slData.branchAddress = SqlMapper.Query<AddressMasterDto>(
                            con, $"select * from AddressMaster where CompanyDetailId={slData.loanData.BranchId}").FirstOrDefault();

            return slData;
        }

        [Route("api/CustomerAPI/GetLoanReceableDayBook/{branchId}/{loginUserRole}")]
        [HttpGet]
        public List<CustomerLoanCardDto> GetLoanReceableDayBook(int branchId, string loginUserRole)
        {
            try
            {
                connection();
                con.Open();

                var loanData = SqlMapper.Query<CustomerLoanCardDto>(
                                 con, $"getCustomerEMICard {branchId},'{loginUserRole}'").ToList();

                con.Close();

                return loanData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Route("api/CustomerAPI/SaveEMIForMultipleUser")]
        [HttpPost]
        public string SaveEMIForMultipleUser([FromBody] MultipleEMIPayment data)
        {
            try
            {
                connection();
                con.Open();

                var DataList = SqlMapper.Query<CustomerLoanCardDto>(
                            con, $"getCustomerEMICard {data.branchId},'{data.loginUserRole}'").ToList();


                for (int i = 0; i < data.selectedIds.Count; i++)
                {
                    var loanDT = DataList.Where(x => x.Id == data.selectedIds[i]).FirstOrDefault();

                    string sqlQuery = $"update CustomerLoanCard set PaiderName='{loanDT.CustomerName}',AmountCollected='{loanDT.Inst_Amount}',PaidStatus='1',PaidBy='{data.paidBy}',EntryBy='{data.userId}',CreatedBy='{data.userId}',EntryDate='{DateTime.Now}',CreatedDate='{DateTime.Now}' where Id ={loanDT.Id}";
                    con.Execute(sqlQuery);

                    var TotalReceivedAmt = SqlMapper.Query<CustomerLoanManagerDTO>(con, $"select * from CustomerLoanManager where LoanNo={loanDT.LoanNo}").FirstOrDefault();

                    if (TotalReceivedAmt != null)
                    {
                        string sqlQuery2 = $"update CustomerLoanManager set ReceivedAmount='{TotalReceivedAmt.ReceivedAmount + loanDT.Inst_Amount}' where LoanNo={loanDT.LoanNo}";
                        con.Execute(sqlQuery2);
                    }

                }

                con.Close();

                return "Data saved successfully";
            }
            catch (Exception ex)
            {

                return $"Exeption:{ex.Message}";
            }
        }

        [Route("api/CustomerAPI/GetCardWithLoanNo/{loanNo}/{loanAccountNo}")]
        [HttpGet]
        public LoanCardDetails GetCardWithLoanNo(string loanNo, string loanAccountNo)
        {
            connection();
            con.Open();

            LoanCardDetails loanCardDetails = new LoanCardDetails();

            var parameters = new { LoanNo = loanNo, LoanAccNo = loanAccountNo };

            using (var multi = con.QueryMultiple("getCustomerEMICardWithLoanNo", parameters, commandType: CommandType.StoredProcedure))
            {
                loanCardDetails.CustomerCardInfo = multi.Read<CustomerCardInfo>().FirstOrDefault();
                loanCardDetails.AddressMaster = multi.Read<AddressMasterDto>().FirstOrDefault();
                loanCardDetails.LoanCardEMIs = multi.Read<CustomerLoanCardDto>().OrderBy(x => x.RepaymentDate).ToList();

                return loanCardDetails;
            }
        }

        [Route("api/CustomerAPI/getLoanDaywise/{id}")]
        [HttpGet]
        public CustomerLoanCardDto getLoanDaywise(int id)
        {
            connection();

            con.Open();
            var loanData = SqlMapper.Query<CustomerLoanCardDto>(
                             con, $"select clm.*,ac.FirstName +' '+ac.LastName as CustomerName from CustomerLoanCard clm " +
                            $"inner join ApplicationCustomer ac on clm.LoanAccNo =ac.LoanAppAccountNo " +
                            $"where clm.Id={id}").FirstOrDefault();

            con.Close();

            return loanData;

        }

        [Route("api/CustomerAPI/CustomEMIPaid")]
        [HttpPost]
        public string CustomEMIPaid([FromBody] CustomerLoanCardDto loanData)
        {
            try
            {
                connection();
                con.Open();

                string sqlQuery = $"update CustomerLoanCard set RepaymentDate='{loanData.RepaymentDate}',PaiderName='{loanData.PaiderName}',AmountCollected='{loanData.AmountCollected}',PaidStatus='1',PaidBy='{loanData.PaidBy}',EntryBy='{loanData.EntryBy}',EntryDate='{loanData.EntryDate}' where Id ={loanData.Id}";
                con.Execute(sqlQuery);

                var manageLoan = SqlMapper.Query<CustomerLoanManagerDTO>(
                            con, $"select * from CustomerLoanManager where LoanNo ='{loanData.LoanNo}'").FirstOrDefault();

                var RecAmt = manageLoan.ReceivedAmount + loanData.AmountCollected;

                sqlQuery = $"update CustomerLoanManager set ReceivedAmount='{RecAmt}' where LoanNo ='{loanData.LoanNo}'";
                con.Execute(sqlQuery);

                con.Close();

                return "EMI amount saved successfully.";
            }
            catch (Exception ex)
            {
                return $"Exception : {ex.Message}";
            }
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