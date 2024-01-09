using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;

namespace CrudTests
{
    public class PersonServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testoutputHelper;

        public PersonServiceTest(ITestOutputHelper testoutputHelper) //ITestOutputHelper is for able to see the output for every test case just like console.writeline()
        {
            _personsService = new PersonsService(false);
            _countriesService = new CountriesService(false);
            _testoutputHelper = testoutputHelper;
        }

        #region Addperson
        //when we supply null value as personaddrequest it should return argumentnullexcpetion

        [Fact]
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        //when we supply null value as personname it should throw argumentexcpetion
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }


        //when we supply proper person details  it should insert the person into person list and it should returnan object of personresponse, which inclused with the newly generated perosn id.
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Akash",
                Email = "akash@gmail.com",
                Address = "Bhandup",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1998-07-22"),
                ReceiveNewsLetters = true
            };

            //act
            PersonResponse person_respnse_from_add = _personsService.AddPerson(personAddRequest);

            List<PersonResponse> persons_list = _personsService.GetAllPersons();

            Assert.True(person_respnse_from_add.PersonId != Guid.Empty);

            Assert.Contains(person_respnse_from_add, persons_list);
        }

        #endregion

        #region Getperosnbyperosnid
        //if we supply null as perosnid , it should return null as perosnresponse.

        [Fact]
        public void GetpersonByPersonId_NullPersonId()
        {
            Guid? personId = null;
            PersonResponse? person_response_from_get = _personsService.GetpersonByPersonId(personId);
            Assert.Null(person_response_from_get);

        }

        //if we supply valid as perosnid , it should return valid persondetails as as perosnresponse object.

        [Fact]
        public void GetpersonByPersonId_WithPersonId()
        {
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryResponse country_response = _countriesService.AddCountry(country_request);
            PersonAddRequest person_request = new PersonAddRequest()
            {
                PersonName = "Perosn name...",
                Email = "email@gmail.com",
                Address = "address...",
                CountryId = country_response.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-02"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };
            PersonResponse person_response_form_add = _personsService.AddPerson(person_request);
            PersonResponse? person_response_from_get = _personsService.GetpersonByPersonId(person_response_form_add.PersonId);
            Assert.Equal(person_response_from_get, person_response_form_add);
        }
        #endregion

        #region getallpersons

        //the getallpersons() should return an empty list by default

        [Fact]
        public void GetAllPerosns_EmptyList()
        {
            //act
            List<PersonResponse> persons_from_get = _personsService.GetAllPersons();

            //assert
            Assert.Empty(persons_from_get);
        }

        [Fact]
        public void GetAllPerosns_AddFewPersons()
        {
            //act
            CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };
            CountryResponse country_response_1 = _countriesService.AddCountry(country_request1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request2);


            PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@gmail.com", Gender = GenderOptions.Male, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") };

            PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "john", Email = "john@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };



            //assert
            List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in perosn_requests)
            {
                PersonResponse person_response = _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //itestoutputhelper
            _testoutputHelper.WriteLine("expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            //

            List<PersonResponse> persons_list_from_get = _personsService.GetAllPersons();
            foreach (PersonResponse person_response_from_add in persons_list_from_get)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }

            //itestoutputhelper
            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //

        }

        #endregion

        #region GetFiletredpersons

        //if the search text is empty and search by is personname it should return all persons. 

        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //act
            CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };
            CountryResponse country_response_1 = _countriesService.AddCountry(country_request1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request2);


            PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@gmail.com", Gender = GenderOptions.Male, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") };

            PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "john", Email = "john@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };



            //assert
            List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in perosn_requests)
            {
                PersonResponse person_response = _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //itestoutputhelper
            _testoutputHelper.WriteLine("expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            //

            List<PersonResponse> persons_list_from_search = _personsService.GetFilteredpersons(nameof(Person.PersonName), "");
            foreach (PersonResponse person_response_from_add in persons_list_from_search)
            {
                Assert.Contains(person_response_from_add, persons_list_from_search);
            }

            //itestoutputhelper
            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //

        }

        //1first we will add few eprsons and then we will search based on eprosn name with some search string , it should return the matching perosns.
        [Fact]
        public void GetFilteredPersons_SearchByPersonsName()
        {
            //act
            CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };
            CountryResponse country_response_1 = _countriesService.AddCountry(country_request1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request2);


            PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@gmail.com", Gender = GenderOptions.Male, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") };

            PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "john", Email = "john@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };



            //assert
            List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in perosn_requests)
            {
                PersonResponse person_response = _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //itestoutputhelper
            _testoutputHelper.WriteLine("expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            //

            List<PersonResponse> persons_list_from_search = _personsService.GetFilteredpersons(nameof(Person.PersonName), "ma");
            foreach (PersonResponse person_response_from_add in persons_list_from_search)
            {
                if (person_response_from_add.PersonName != null)
                {
                    if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                        Assert.Contains(person_response_from_add, persons_list_from_search);
                }
            }

            //itestoutputhelper
            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //

        }


        #endregion

        //when we sort based on personname is desc, it shoudl return perosns in descending order .
        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons()
        {
            //act
            CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };
            CountryResponse country_response_1 = _countriesService.AddCountry(country_request1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request2);


            PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Akash", Email = "akash@gmail.com", Gender = GenderOptions.Female, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") };

            PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "Sagar", Email = "sagar@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };



            //assert
            List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in perosn_requests)
            {
                PersonResponse person_response = _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //itestoutputhelper
            _testoutputHelper.WriteLine("expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            //

            List<PersonResponse> allPersons = _personsService.GetAllPersons();
            List<PersonResponse> persons_list_from_sort = _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //itestoutputhelper
            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //
            person_response_list_from_add = person_response_list_from_add.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList();

            //assert
            for (int i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            }

        }
        #endregion

        #region UpdatePerson
        /// <summary>
        /// when we supply nulll as personupdaterequest it should throw arguiment null excpetion.
        /// </summary>
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest();
            personUpdateRequest = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });



        }

        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest() { PersonId = Guid.NewGuid() };

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);
            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_response_from_add.CountryID, Email = "john@gmail.com", Gender = GenderOptions.Male };
            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);
            PersonUpdateRequest personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        /// <summary>
        /// add new perosn and to update the same
        /// </summary>
        [Fact]
        public void UpdatePerson_PersonFullDetails()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);
            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_response_from_add.CountryID, Address = "ABC ROAD", Email = "abc@gmail.com", DateOfBirth = DateTime.Parse("2000-05-09"), Gender = GenderOptions.Male, ReceiveNewsLetters = true };
            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);
            PersonUpdateRequest personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "MAX";
            personUpdateRequest.Email = "max@gmail.com";


            PersonResponse person_response_from_update = _personsService.UpdatePerson(personUpdateRequest);

            PersonResponse person_response_from_get = _personsService.GetpersonByPersonId(person_response_from_update.PersonId);

            Assert.Equal(person_response_from_get, person_response_from_update);


        }

        #endregion

        #region Deleteperson
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse country_respinse_form_add = _countriesService.AddCountry(country_add_request);
            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_respinse_form_add.CountryID, Address = "ABC ROAD", Email = "abc@gmail.com", DateOfBirth = DateTime.Parse("2000-05-09"), Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

            bool isDeleted = _personsService.DeletePerson(person_response_from_add.PersonId);
            Assert.True(isDeleted);

        }

        [Fact]
        public void DeletePerson_InValidPersonID()
        {
            

            bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());
            Assert.False(isDeleted);

        }
        #endregion
    }
}
