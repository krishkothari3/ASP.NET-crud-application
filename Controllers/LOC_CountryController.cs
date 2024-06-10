using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace AddressBook.Controllers
{
    public class LOC_CountryController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public LOC_CountryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region SelectAll
        public IActionResult Index(/*string? CountryName*/)
        {
            //if (CountryName == null)
            //{
                //CountryName = "";
                SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Country_SelectAll";
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                return View("LOC_CountryList", dt);
            //}
            //else
            //{
            //    SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            //    conn.Open();
            //    SqlCommand cmd = conn.CreateCommand();
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "PR_Search_Country";
            //    cmd.Parameters.AddWithValue("@name", CountryName);
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    DataTable dt = new DataTable();
            //    dt.Load(reader);
            //    conn.Close();
            //    return View("LOC_CountryList", dt);
            //}

        }
        #endregion

        #region AddCountry
        public IActionResult AddEditCountry(LOC_CountryModel modelLOC_Country, int CountryID = 0)
        {
            if (string.IsNullOrEmpty(modelLOC_Country.CountryName))
            {
                ModelState.AddModelError("CountryName", "CountryName is required");
            }
            if (string.IsNullOrEmpty(modelLOC_Country.CountryCode))
            {
                ModelState.AddModelError("CountryCode", "CountryCode is required");
            }
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if (ModelState.IsValid)
            {
                if (CountryID == 0)
                {
                    cmd.CommandText = "PR_Country_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_Country_UpdateByPK";
                    cmd.Parameters.AddWithValue("@CountryID", modelLOC_Country.CountryId);
                }
                cmd.Parameters.AddWithValue("@CountryName", modelLOC_Country.CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", modelLOC_Country.CountryCode);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            conn.Close();
            return View("LOC_CountryAddEdit", modelLOC_Country);
        }
        public IActionResult LOC_CountryAddEdit()
        {
            return View();
        }
        #endregion

        #region EditCountry
        public IActionResult SelectByPK(int? CountryID)
        {
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            LOC_CountryModel model = new LOC_CountryModel();
            if (CountryID == null)
            {
                CountryID = -1;
            }
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_SelectByPK";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            foreach (DataRow data in dt.Rows)
            {
                model.CountryName = data["CountryName"].ToString();
                model.CountryCode = data["CountryCode"].ToString();
                model.CountryId = int.Parse(data["CountryID"].ToString());
            }
            conn.Close();
            return View("LOC_CountryAddEdit", model);
        }
        #endregion

        #region DeleteCountry
        public IActionResult DeleteByPK(int CountryID)
        {
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_DeleteByPK";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
