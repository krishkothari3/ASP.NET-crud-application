using AddressBook.Areas.LOC_Branch.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AddressBook.Areas.LOC_Branch.Controllers
{
    [Area("LOC_Branch")]
    [Route("LOC_Branch/[controller]/[action]")]
    public class LOC_BranchController : Controller
    {
        public IConfiguration Configuration { get; set; }
        public LOC_BranchController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #region selectAllBranch
        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Branch_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();
            return View(dt);
        }
        #endregion

        #region navigate to add branch form page
        public IActionResult AddBranch() 
        {
            return View();
        }
        #endregion

        #region get branch on edit click
        public IActionResult SelectByPK(int? BranchID) 
        {
            SqlConnection conn = new SqlConnection((this.Configuration.GetConnectionString("Default")));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Branch_SelectByPK";
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            LOC_BranchModel model = new LOC_BranchModel();
            DataTable dt = new DataTable();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            foreach (DataRow item in dt.Rows)
            {
                model.BranchName = item["BranchName"].ToString();
                model.BranchCode = item["BranchCode"].ToString();
            }
            conn.Close();
            return View("AddBranch",model); 
        }
        #endregion

        #region save button of form page action
        public IActionResult Save(LOC_BranchModel modelLOC_Branch,int? BranchID)
        {
            #region add new brancg
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (BranchID == null)
            {
                cmd.CommandText = "PR_Branch_Insert";
            }
            #endregion
            #region update existing branch
            else
            {
                cmd.CommandText = "PR_Branch_UpdateByPK";
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
            }
            #endregion
            cmd.Parameters.AddWithValue("@BranchName", modelLOC_Branch.BranchName);
            cmd.Parameters.AddWithValue("@BranchCode", modelLOC_Branch.BranchCode);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        public IActionResult DeleteByPK(int? BranchID)
        {
            SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Branch_DeleteByPK";
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
    }
}
