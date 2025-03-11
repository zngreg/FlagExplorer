namespace FlagExplorerApp.Services.Models
{
    public class CountryDetails : Country
    {
        public long Population { get; set; }
        public string Capital { get; set; }
    }
}