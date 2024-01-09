using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// represents DRO class for update person.
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "person ID can't be blank !")]
        public Guid PersonId { get; set; }
        [Required(ErrorMessage = "person name can't be blank !")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "email can't be blank !")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email !")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }


        //convert current object of personaddrequest into a new object of person type
        public Person ToPerson()
        {
            return new Person()
            {
                Personid = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfbirth = DateOfBirth,
                Gender = Gender.ToString(),

                CountryId = CountryId,

            };
        }
    }
}
