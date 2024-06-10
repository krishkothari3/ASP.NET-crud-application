namespace AddressBook.Models
{
    public class LOC_StateModel
    {
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
        public string StateCode { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }

        
    }

    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

    }
}
