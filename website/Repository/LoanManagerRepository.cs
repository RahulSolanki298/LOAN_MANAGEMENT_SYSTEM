using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using website.Dto;
using website.Helpers;
using website.Interface;
using website.Models;

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

        public List<CustomerLoanManagerDTO> GetCustomersLoan(int userId, int branchId, string loanStatus, string loginUserRole)
        {
            connection();
            con.Open();

            var customers = SqlMapper.Query<CustomerLoanManagerDTO>(
                             con, $"SP_CreateLoanManager {userId},{branchId},'{loanStatus}','{loginUserRole}'").ToList();

            con.Close();

            return customers;
        }

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

        public bool SaveCustomerLoanApply(CustomerLoanManagerDTO data)
        {
            connection();
            con.Open();
            var param = new DynamicParameters();
            param.Add("@Id", data.Id > 0 ? data.Id : 0);
            param.Add("@UserId", data.UserId);
            param.Add("@BranchId", data.BranchId);
            param.Add("@LoanApplyAmountDate", data.LoanApplyAmountDate);
            param.Add("@LoanApplyAmount", data.LoanApplyAmount);
            param.Add("@LoanNetAmount", data.LoanNetAmount);
            param.Add("@LoanEMI", data.LoanEMI);
            param.Add("@IsNoOfDays", data.IsNoOfDays);
            param.Add("@LoanIntrest", data.LoanIntrest);
            param.Add("@LoanAccNo", data.LoanAccNo);
            param.Add("@NoOfDays", data.NoOfDays);
            param.Add("@LoanNo", data.LoanNo);

            con.ExecuteScalar("SP_SaveLoanApply", param, commandType: CommandType.StoredProcedure);

            con.Close();
            return true;
        }

        public bool UpdateLoanStatus(int id, string status)
        {
            connection();
            con.Open();

            var StatusDT = $"select Id from LoanStages where Name='{status}'";
            var statusId = SqlMapper.Query<CustomerLoanStatus>(
                            con, StatusDT).FirstOrDefault();

            var LoanDetail = SqlMapper.Query<CustomerLoanManagerDTO>(
                            con, $"select * from CustomerLoanManager where Id = {id}").FirstOrDefault();

            LoanDetail.SectionAmount = LoanDetail.LoanApplyAmount;
            LoanDetail.SectionAmountDate = DateTime.Now;
            LoanDetail.LoanDate = DateTime.Now;
            //LoanDetail.UpdateById = string.Empty;
            LoanDetail.UpdatedDate = DateTime.Now;

            string sqlQuery1 = $"update CustomerLoanManager set SectionAmount=@SectionAmount,SectionAmountDate=@SectionAmountDate,UpdatedDate=@UpdatedDate,LoanDate=@LoanDate where Id={id}";

            int rowsAffected1 = con.Execute(sqlQuery1, LoanDetail);

            var PassLoanDT = SqlMapper.Query<CustomerLoanManagerStage>(
                            con, $"select * from CustomerLoanManagerStage where CustomerLoanManagerId = {id}").FirstOrDefault();

            PassLoanDT.LoanStagesId = statusId.Id;

            string sqlQuery = $"update CustomerLoanManagerStage set LoanStagesId=@LoanStagesId where CustomerLoanManagerId={id}";

            int rowsAffected = con.Execute(sqlQuery, PassLoanDT);

            if (status == LoanStages.Active)
            {
                var manageLoan = SqlMapper.Query<CustomerLoanManagerDTO>(
                            con, $"select * from CustomerLoanManager where Id = {id}").FirstOrDefault();

                var dataTable = new DataTable();
                dataTable.Columns.Add("LoanAccNo", typeof(string));
                dataTable.Columns.Add("LoanNo", typeof(string));
                dataTable.Columns.Add("RepaymentDate", typeof(DateTime));
                dataTable.Columns.Add("OS_pricipal", typeof(decimal));
                dataTable.Columns.Add("Inst_Amount", typeof(decimal));
                dataTable.Columns.Add("Principal", typeof(decimal));
                dataTable.Columns.Add("Intrest", typeof(decimal));

                var loanAmt = Convert.ToDecimal(manageLoan.LoanNetAmount);
                for (int i = 1; i <= int.Parse(manageLoan.NoOfDays); i++)
                {
                    manageLoan.LoanNetAmount = Convert.ToInt32(manageLoan.LoanNetAmount - manageLoan.LoanEMI);


                    dataTable.Rows.Add(
                        manageLoan.LoanAccNo,
                        manageLoan.LoanNo,
                        DateTime.Now.AddDays(i),
                        manageLoan.LoanNetAmount,
                        manageLoan.LoanEMI,
                        loanAmt,
                        manageLoan.LoanIntrest
                        );
                }
                var param = new DynamicParameters();
                param.Add("@LoanCard", dataTable);
                con.Execute("SP_LoanCardCreate", new { LoanCard = dataTable.AsTableValuedParameter("LoanCard") }, commandType: CommandType.StoredProcedure);
            }

            con.Close();
            return true;
        }

        public List<CustomerLoanCardDto> GetLoanReceableDayBook(int userId, int branchId, string loanStatus, string loginUserRole)
        {
            connection();
            con.Open();

            var loanData = SqlMapper.Query<CustomerLoanCardDto>(
                             con, $"getCustomerEMICard {branchId},'{loginUserRole}'").ToList();

            con.Close();

            return loanData;
        }

        public string SaveEMIForMultipleUser(List<int> selectedIds, string paidBy, int branchId, string loginUserRole)
        {
            try
            {
                connection();
                con.Open();

                var DataList = SqlMapper.Query<CustomerLoanCardDto>(
                            con, $"getCustomerEMICard {branchId},'{loginUserRole}'").ToList();


                for (int i = 0; i < selectedIds.Count; i++)
                {
                    var loanDT = DataList.Where(x=>x.Id == selectedIds[i]).FirstOrDefault();

                    string sqlQuery = $"update CustomerLoanCard set PaiderName='{loanDT.CustomerName}',AmountCollected='{loanDT.Inst_Amount}',PaidStatus='1',PaidBy='{paidBy}' where Id ={loanDT.Id}";
                    con.Execute(sqlQuery);
                }

                con.Close();

                return "Data saved successfully";
            }
            catch (Exception ex)
            {

                return $"Exeption:{ex.Message}";
            }
        }
    }
}