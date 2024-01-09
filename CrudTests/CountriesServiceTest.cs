﻿using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CrudTests
{

    public class CountriesServiceTest
    {
        /// <summary>
        /// we should have reference of service project in test project.
        /// </summary>
        private readonly ICountriesService _countriesService;

        //constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false); // means this intilalization will not work.
        }

        #region AddCountry

        //when countryaddrequest is null, it should return argumentnullexception.
        [Fact]
        public void AddCountry_NullCountry()
        {
            //arrange
            CountryAddRequest? request = null;

            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _countriesService.AddCountry(request);
            });

            //act
            //_countriesService.AddCountry(request);
        }

        //when the countryname is null, it should throw argumenexception

        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request);
            });

            //act
            //_countriesService.AddCountry(request);
        }

        //when countryname is duplicate it should throw argumentexception
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });

            //act
        }

        //whe you supply countryname it should insert the country to the existing list of countries.
        [Fact]
        public void AddCountry_ProperCOuntryDetails()
        {
            //arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "Japan"
            };


            //act
            CountryResponse response = _countriesService.AddCountry(request1);

            List<CountryResponse> countries_From_GetAllCountries =  _countriesService.GetAllCountries();

            //assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_From_GetAllCountries); // this contains metghod actually calls the equeals method overrided in countryresponse class.
        }

        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //acts
            List<CountryResponse> actual_country_reponse_list = _countriesService.GetAllCountries();

            //assert
            Assert.Empty(actual_country_reponse_list); 
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest> { new CountryAddRequest() { CountryName = "USA" },
            new CountryAddRequest(){CountryName = "UK" }
            };

            //act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
            foreach (CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();
            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList); // this contains metghod actually calls the equeals method overrided in countryresponse class.
            } 
        }


        #endregion

        #region getCountryByCountryid
        [Fact]
        //if we supply null country id then it should return the matching null details as countryresponse object.
        public void GetCountryByCountryId_NullCountryid()
        {
            //arrange
            Guid? countryId = null;

            //act
            CountryResponse? country_response_from_get_method = _countriesService.GetCountryByCountryId(countryId);

            //assert
            Assert.Null(country_response_from_get_method);

        }

        ////if we supply null country id then it should return the matching country details as countryresponse object.
        [Fact]
        public void GetCountryByCountryId_ValidCountryId()
        {
            //arrange
            CountryAddRequest? country_add_request = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            //act
            CountryResponse? country_response_from_get =  _countriesService.GetCountryByCountryId(country_response_from_add.CountryID);

            //assert
            Assert.Equal(country_response_from_add,country_response_from_get);

        }

        #endregion
    }
}