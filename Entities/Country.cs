namespace Entities
{
    /// <summary>
    /// Domain model for storing country details
    /// </summary>
    public class Country
    {
        public Guid CountryID { get; set; }// values are unlimited
        public string? CountryName { get; set; }
    }
}