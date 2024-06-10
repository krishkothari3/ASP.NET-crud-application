using AddressBook.Areas.LOC_City.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace AddressBook.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/[controller]/[action]")]
    public class LOC_CityController : Controller
    {

        public IConfiguration configuration { get; set; }
        public LOC_CityController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region select All City
        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            if (reader.HasRows)
            {
                dt.Load(reader);
            }
            return View("LOC_CityList", dt);
        }
        #endregion

        #region select Specific City To Update
        public IActionResult SelectByPK(int? CityID, int CountryID)
        {
            FillCountryDDL();
            FillStateDDL(CountryID);
            LOC_CityModel modelLOC_City = new LOC_CityModel();
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_SelectByPK";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            foreach (DataRow data in dt.Rows)
            {
                modelLOC_City.CityName = data["CityName"].ToString();
                modelLOC_City.CityCode = data["CityCode"].ToString();
                modelLOC_City.StateID = int.Parse(data["StateID"].ToString());
                modelLOC_City.CountryID = int.Parse(data["CountryID"].ToString());
            }
            return View("LOC_CityAddEdit", modelLOC_City);
        }
        #endregion

        #region addCity
        public IActionResult Add(LOC_CityModel modelLOC_City,Country countryModel, State stateModel, int? CityID)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (CityID == null)
            {
                cmd.CommandText = "PR_City_Insert";
            }
            else
            {
                cmd.CommandText = "PR_City_UpdateByPK";
                cmd.Parameters.AddWithValue("@CityID", modelLOC_City.CityID);
            }
            cmd.Parameters.AddWithValue("@StateID", modelLOC_City.StateID);
            cmd.Parameters.AddWithValue("@CountryID", modelLOC_City.CountryID);
            cmd.Parameters.AddWithValue("@CityName", modelLOC_City.CityName);
            cmd.Parameters.AddWithValue("@CityCode", modelLOC_City.CityCode);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }

        public IActionResult AddForm()
        {
            FillCountryDDL();
            FillStateDDL();
            return View("LOC_CityAddEdit");
        }
        #endregion

        #region deleteCity
        public IActionResult DeleteByPK(int CityID)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("Default"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_DeleteByPK";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        public JsonResult FillCountryDDL(int StateID = -1)

        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("Default"));
            List<Country> country = new List<Country>();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Cascade_State";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Country countryModel = new Country()
                {
                    CountryID = Convert.ToInt32(reader["CountryID"]),
                    CountryName = reader["CountryName"].ToString()
                };
                country.Add(countryModel);
            }
            ViewBag.Countries = new SelectList(country, "CountryID", "CountryName");
            reader.Close();
            conn.Close();
            return Json(country);
            //ViewBag.CountryList = country;
        }

        public JsonResult FillStateDDL(int CountryId = -1)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("Default"));
            List<State> states = new List<State>();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_GetStateFromCountryName";
            cmd.Parameters.AddWithValue("@CountryId", CountryId);
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                State stateModel = new State()
                {
                    StateID = Convert.ToInt32(reader["StateID"]),
                    StateName = reader["StateName"].ToString()
                };
                states.Add(stateModel);
            }
            ViewBag.StateList = new SelectList(states, "StateID", "StateName");
            conn.Close();
            return Json(states);
        }

        //public void FillStateDDL(string selectedCountry = "India")
        //{
        //    SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("Default"));
        //    List<LOC_StateDropDownModel> state = new List<LOC_StateDropDownModel>();
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "PR_Country_GetStateFromCountryName";
        //    cmd.Parameters.AddWithValue("@CountryName", selectedCountry);
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        LOC_StateDropDownModel stateModel = new LOC_StateDropDownModel()
        //        {
        //            StateID = Convert.ToInt32(reader["StateID"]),
        //            StateName = reader["StateName"].ToString(),
        //        };
        //        state.Add(stateModel);
        //    }
        //    reader.Close();
        //    conn.Close();
        //    ViewBag.States = new SelectList(state, "StateID", "StateName");
        //}
    }
}
