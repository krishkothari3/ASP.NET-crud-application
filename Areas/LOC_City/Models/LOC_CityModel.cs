using AddressBook.Models;

namespace AddressBook.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }
        public string CityName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CityCode { get; set; }
    }

    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int StateID { get; set; }
        public ICollection<State> States { get; set; }
    }

    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public Country country { get; set; }

    }
}
