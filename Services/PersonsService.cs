using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using System.Diagnostics;
using ServiceContracts.Enums;
using System.Transactions;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService
            _countriesService;

        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
            if (initialize)
            {
                //{}
                //{}
                //{}
                //{E1F0C660-B97A-497F-9F7B-3EA313063C3C}

                //sampel data
                // John Doe, john.doe @email.com,1985 - 07 - 15,Male,123 Main St, TRUE
                //Jane Smith, jane.smith @email.com,1990 - 03 - 22,Female,456 Oak Ave, FALSE
                //Bob Johnson, bob.johnson @email.com,1978 - 11 - 05,Male,789 Pine Rd, TRUE
                //Alice Brown, alice.brown @email.com,1982 - 09 - 18,Female,101 Cedar Ln, FALSE
                //Chris Wilson, chris.wilson @email.com,1995 - 04 - 30,Non - Binary,202 Elm Blvd, TRUE
                //sample data

                _persons.Add(new Person()
                {
                    Personid = Guid.Parse("C8CB6F80-9645-41D8-A6A5-A991EA1ACF06"),

                    PersonName = "John Doe",
                    Email = "john.doe @email.com",
                    DateOfbirth = DateTime.Parse("1985-07-15"),
                    Address = "123 Main St",
                    ReceiveNewsLetters = true,
                    Gender = "Male",
                    CountryId =
                    Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D")
                });

                _persons.Add(new Person()
                {
                    Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

                    PersonName = "Jane Smith",
                    Email = "jane.smith @email.com",
                    DateOfbirth = DateTime.Parse("1990-03-22"),
                    Address = "456 Oak Ave",
                    ReceiveNewsLetters = true,
                    Gender = "Female",
                    CountryId =
                    Guid.Parse("52BC74D6-9094-441F-A51D-C1604F26F376")

                });

                _persons.Add(new Person()
                {
                    Personid = Guid.Parse("2EFA7A85-7252-4408-A377-CDE1BD1510F3"),

                    PersonName = "Jane Smith",
                    Email = "jane.smith @email.com",
                    DateOfbirth = DateTime.Parse("1990-03-22"),
                    Address = "456 Oak Ave",
                    ReceiveNewsLetters = true,
                    Gender = "Female",
                    CountryId =
                    Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D")
                });

                _persons.Add(new Person()
                {
                    Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

                    PersonName = "Bob Johnson",
                    Email = "bob.johnson @email.com",
                    DateOfbirth = DateTime.Parse("1987-03-09"),
                    Address = "789 Pine Rd",
                    ReceiveNewsLetters = false,
                    Gender = "male",
                    CountryId =
                    Guid.Parse("E45C96A1-3D83-4EF9-99E4-2B6FF51B5399")
                });

                _persons.Add(new Person()
                {
                    Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

                    PersonName = "Alice Brown",
                    Email = "alice.brown @email.com",
                    DateOfbirth = DateTime.Parse("1982-09-18"),
                    Address = "101 Cedar Ln",
                    ReceiveNewsLetters = true,
                    Gender = "Female",
                    CountryId =
                   Guid.Parse("54DEAD56-5A92-4C41-AF81-F27DC37BC67D")
                });
            }
        }

        private PersonResponse ConvertPersonToPersonresponse(Person person)
        {
            PersonResponse personresponse = person.ToPersonResponse();
            personresponse.Country = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
            return personresponse;
        }


        public PersonResponse AddPerson(PersonAddRequest personAddRequest)
        {
            //CHWECK IF PEROSNREQUEST OBJEVCT IS NOT NULL
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }
            //if (string.IsNullOrEmpty(personAddRequest.PersonName))
            //{
            //    throw new ArgumentException("person name cannot be blank !");
            //}

            //mode validations:
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.Personid = Guid.NewGuid();
            _persons.Add(person);

            //convert perosn object into perosnrepsonse.
            return ConvertPersonToPersonresponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetpersonByPersonId(Guid? personid)
        {
            if (personid == null)
                return null;
            Person? person = _persons.FirstOrDefault(temp => temp.Personid == personid);
            if (person == null)
                return null;

            return person.ToPersonResponse();

        }

        public List<PersonResponse> GetFilteredpersons(string searchby, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;
            foreach (PersonResponse personResponse in matchingPersons)
            {
                personResponse.Country = _countriesService?.GetCountryByCountryId(personResponse.CountryId)?.CountryName;
            }
            if (string.IsNullOrEmpty(searchby) || string.IsNullOrEmpty(searchString))
                return matchingPersons;
            switch (searchby)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfbirth):
                    matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;


                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; break;

            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons //_ means deafult case.

            };  // switch expression
            return sortedPersons;

        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.Personid == personUpdateRequest.PersonId);

            if (matchingPerson == null)
                throw new ArgumentException("Given person id doesnt exist !");

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfbirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId == null)
                throw new ArgumentNullException(nameof(personId));
            Person person = _persons.FirstOrDefault(temp => temp.Personid == personId);
            if (person == null)
                return false;

            _persons.RemoveAll(temp => temp.Personid == personId);
            return true;
        }
    }
}