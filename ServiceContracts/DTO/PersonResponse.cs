using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse))
                return false;
            PersonResponse person =
(PersonResponse)obj;
            return this.PersonId == person.PersonId && this.PersonName == person.PersonName && Email == person.Email && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryId == person.CountryId && Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"personid : {PersonId} , personname: {PersonName}, email: {Email}, date of birth: {DateOfBirth?.ToString("dd MMM yyyy")}, gender: {Gender}, country id: {CountryId}, address: {Address}, receive news letters: {ReceiveNewsLetters}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                CountryId = CountryId
            };
        }
    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            //person => personresponse
            return new PersonResponse()
            {
                PersonId = person.Personid,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfbirth,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Address = person.Address,
                CountryId = person.CountryId,

                Gender = person.Gender,
                Age = (person.DateOfbirth != null) ? Math.Round((DateTime.Now - person.DateOfbirth.Value).TotalDays / 365.25) : null, 
                Country = person.Country?.CountryName
            };
        }

        private static PersonResponse ConvertPersonToPersonresponse(Person person)
        {
            PersonResponse personresponse = person.ToPersonResponse();
            personresponse.Country = person.Country?.CountryName;
            return personresponse;
        }
    }
}
