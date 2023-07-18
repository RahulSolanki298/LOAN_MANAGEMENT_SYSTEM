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
using website.Models;

namespace website.Repository
{
    public class BranchRepository : IBranchRepository
    {
        public SqlConnection con;

        public bool DeleteBranchDetail(int id)
        {
            throw new System.NotImplementedException();
        }

        public CompanyBranchDetail GetBranchDetail(int id)
        {
            connection();
            con.Open();

            var branch = SqlMapper.Query<CompanyBranchDetail>(
                              con, $"select * from CompanyBranchDetails where Id={id}").FirstOrDefault();

            con.Close();

            return branch;
        }

        public CompanyBranchDetail GetBranchByName(string branchName)
        {
            connection();
            con.Open();

            var branch = SqlMapper.Query<CompanyBranchDetail>(
                              con, $"select * from CompanyBranchDetails where Title='{branchName}'").FirstOrDefault();

            con.Close();

            return branch;
        }

        public List<CompanyBranchDetail> GetBranchList()
        {
            connection();
            con.Open();

            var companyAndBranch = SqlMapper.Query<CompanyBranchDetail>(
                          con, "SP_CompanyAndBranchList").ToList(); //GetBranchAdminList

            con.Close();

            return companyAndBranch;
        }

        public bool SaveBranchDetail(CompanyBranchDetail branchDetail)
        {
            connection();
            con.Open();

            var param = new DynamicParameters();
            param.Add("@Id", branchDetail.Id > 0 ? branchDetail.Id : 0);
            param.Add("@Title", branchDetail.Title);
            param.Add("@SubTitle", branchDetail.SubTitle);
            param.Add("@Description", branchDetail.Description);
            param.Add("@Certificate", branchDetail.Certificate);
            param.Add("@CompanyLogo", branchDetail.CompanyLogo);
            param.Add("@CompanyRegisterDate", DateTime.Now);
            param.Add("@IsActivated", branchDetail.IsActivated);
            param.Add("@CompanyBranchType", branchDetail.CompanyBranchType);

            con.ExecuteScalar("SP_CompanyBranchCreation", param, commandType: CommandType.StoredProcedure);

            con.Close();

            return true;
        }

        public List<BranchAdminRegistrationDTO> getBranchAdminList()
        {
            connection();
            con.Open();

            var adminList = SqlMapper.Query<BranchAdminRegistrationDTO>(
                          con, "GetBranchAdminList").ToList(); //GetBranchAdminList

            con.Close();

            return adminList;

        }

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        public BranchAdminRegistrationDTO GetBranchAdmin(int id)
        {
            connection();
            con.Open();

            var adminList = SqlMapper.Query<BranchAdminRegistrationDTO>(
                          con, $"GetBranchAdminById {id}").FirstOrDefault(); //GetBranchAdminList

            con.Close();

            return adminList;
        }

        public bool SaveBranchAdmin(BranchAdminRegistrationDTO applicationUserDTO)
        {
            connection();
            con.Open();
            var hmac = new HMACSHA512();


            var existAdmin= SqlMapper.Query<BranchAdminRegistrationDTO>(
                              con, $"select * from AppUser where EmailId='{applicationUserDTO.EmailId}' or MobileNo='{applicationUserDTO.MobileNo}'").FirstOrDefault();

            if (existAdmin == null)
            {
                return false;
            }


            if (applicationUserDTO.Id == 0)
            {
                applicationUserDTO.Password = applicationUserDTO.FirstName + "@" + DateTime.Now.Year.ToString();
            }
            else
            {
                applicationUserDTO.Password = "NO-CHANGE";
            }

            var param = new DynamicParameters();
            param.Add("@Id", applicationUserDTO.Id > 0 ? applicationUserDTO.Id : 0);
            param.Add("@UserName", applicationUserDTO.EmailId);
            param.Add("@FirstName", applicationUserDTO.FirstName);
            param.Add("@MiddleName", applicationUserDTO.MiddleName);
            param.Add("@LastName", applicationUserDTO.LastName);
            param.Add("@Gender", applicationUserDTO.Gender);
            param.Add("@DateOfBirth", applicationUserDTO.DateOfBirth);
            param.Add("@MobileNo", applicationUserDTO.MobileNo);
            param.Add("@WhatsAppNo", applicationUserDTO.WhatsAppNo);
            param.Add("@EmailId", applicationUserDTO.EmailId);
            param.Add("@BranchId", applicationUserDTO.BranchId);
            param.Add("@IsActive", applicationUserDTO.IsActive);
            param.Add("@PasswordHash", hmac.ComputeHash(Encoding.UTF8.GetBytes(applicationUserDTO.Password)));
            param.Add("@PasswordSalt", hmac.Key);

            con.ExecuteScalar("SP_CompanyBranchAdminCreation", param, commandType: CommandType.StoredProcedure);

            con.Close();

            return true;
        }
    }
}