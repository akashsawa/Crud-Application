using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// represnts business logic for manipulating country entity.
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// adds a country object to the list of countries.
        /// </summary>
        /// <param name="countryAddrequest">country object to add</param>
        /// <returns>the country object after adding it</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddrequest);

        /// <summary>
        /// returns all countries form the list
        /// </summary>
        /// <returns></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// returns a country based on given country id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>matching country as countryrepsponse</returns>
        Task<CountryResponse>? GetCountryByCountryId(Guid? countryId);

        Task<int> UploadCountriesFromExcelFil(IFormFile formFile) // for adding countries form excle file to database.
;
    }


}