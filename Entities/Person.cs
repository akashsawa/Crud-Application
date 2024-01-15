using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid Personid { get; set; }

        [StringLength(40)] // nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfbirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        //unique identifier
        public Guid? CountryId { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        //bit
        public bool ReceiveNewsLetters { get; set; }

        public string? TIN { get; set; }

        //table realtions:
        [ForeignKey("CountryID")]
        public virtual Country? Country { get; set; }
    }
}
