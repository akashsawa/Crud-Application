using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain model for storing country details
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }// values are unlimited
        public string? CountryName { get; set; } // no '?' means its non null column

        //table ef relations:
        public virtual ICollection<Person>? Persons { get; set; }
    }
}