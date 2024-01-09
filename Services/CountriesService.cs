using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Runtime.CompilerServices;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>() {
                new Country() { CountryID=Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D"), CountryName="USA" },

                new Country() { CountryID = Guid.Parse("52BC74D6-9094-441F-A51D-C1604F26F376"), CountryName = "INDIA" },

                new Country() { CountryID = Guid.Parse("54DEAD56-5A92-4C41-AF81-F27DC37BC67D"), CountryName = "JAPAN" },

                new Country() { CountryID = Guid.Parse("E45C96A1-3D83-4EF9-99E4-2B6FF51B5399"), CountryName = "MEXICO" }
            });
            }

        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddrequest)
        {
            //validation: countryaddrequest parameter cnat be null 
            if (countryAddrequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddrequest));
            }

            //validation : countryname cant be null
            if (countryAddrequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddrequest));
            }

            //validation : countryname cant be duplicate
            if (_countries.Where(temp => temp.CountryName == countryAddrequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Country name already exist !");
            }

            //convert object from countryaddreqeust to country type
            Country country = countryAddrequest.ToCountry();

            //generate countryid
            country.CountryID = Guid.NewGuid();

            //add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;
            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryId);

            if (country_response_from_list == null)
                return null;
            return country_response_from_list.ToCountryResponse();

        }
    }
}