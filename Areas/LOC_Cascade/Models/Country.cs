namespace AddressBook.Areas.LOC_Cascade.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<State> States { get; set; }
    }
}
