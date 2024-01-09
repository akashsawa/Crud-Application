using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// actrs as dto for inserting a new person.
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "person name can't be blank !")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "email can't be blank !")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email !")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date Of Birth can't be blank !")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please select Gender !")]
        public GenderOptions? Gender { get; set; }

        [Required(ErrorMessage = "Please select a country !")]
        public Guid? CountryId { get; set; }

        [Required(ErrorMessage = "Please enter Address !")] public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        //convert current object of personaddrequest into a new object of person type
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfbirth = DateOfBirth,
                Gender = Gender.ToString(),
                Address = Address,
                CountryId = CountryId,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
