using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using website.Models;

namespace website.APIControllers
{
    public class CompanyBranchController : ApiController
    {
        public SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        public List<CompanyBranchDetail> GetCompanyDetails()
        {
            connection();
            con.Open();

            List<CompanyBranchDetail> EmpList = SqlMapper.Query<CompanyBranchDetail>(
                              con, "SP_CompanyAndBranchList").ToList();

            con.Close();

            return EmpList;

        }
    }
}
