using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AddressBook.Controllers
{
    public class LOC_StateController : Controller
    {

        public IConfiguration Configuration { get; set; }

        public LOC_StateController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            if (reader.HasRows)
            {
                dt.Load(reader);
            }
            return View(dt);
        }

        public IActionResult SelectByPK(int? StateId) 
        {
            FillCountryDDL();
            if(StateId == null)
            {
                StateId = -1;
            }
            SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_SelectByPK";
            cmd.Parameters.AddWithValue("@StateID",StateId);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            LOC_StateModel stateModel = new LOC_StateModel();
            foreach (DataRow data in dt.Rows)
            {
                stateModel.StateName = data["StateName"].ToString();
                stateModel.CountryID = Convert.ToInt32(data["CountryID"]);
                stateModel.StateCode = data["StateCode"].ToString();
            }
            conn.Close();
            return View("LOC_StateAddEdit", stateModel);
        }

        [HttpPost]
        public IActionResult Add(LOC_StateModel modelLOC_State, int StateID = 0)
        {
            SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if(StateID == 0)
            {
                cmd.CommandText = "PR_State_Insert";
            }
            else
            {
                cmd.CommandText = "PR_State_UpdateByPK";
                cmd.Parameters.AddWithValue("@StateId", modelLOC_State.StateId);
            }

            cmd.Parameters.AddWithValue("@StateName", modelLOC_State.StateName);
            cmd.Parameters.AddWithValue("@CountryID", modelLOC_State.CountryID);
            cmd.Parameters.AddWithValue("@StateCode", modelLOC_State.StateCode);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }

        public IActionResult AddEditState()
        {
            FillCountryDDL();
            return View("LOC_StateAddEdit");
        }

        public IActionResult DeleteByPK(int StateID)
        {
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_DeleteByPK";
            cmd.Parameters.AddWithValue("@StateID",StateID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }

        public void FillCountryDDL()
        {
            SqlConnection conn = new SqlConnection(this.Configuration.GetConnectionString("Default"));
            List<LOC_CountryDropDownModel> country = new List<LOC_CountryDropDownModel>();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_DropDown_CountryName";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LOC_CountryDropDownModel countryModel = new LOC_CountryDropDownModel()
                {
                    CountryID = Convert.ToInt32(reader["CountryID"]),
                    CountryName = reader["CountryName"].ToString()
                };
                country.Add(countryModel);
            }
            reader.Close();
            conn.Close();
            ViewBag.CountryList = country;
        }


    }
}
