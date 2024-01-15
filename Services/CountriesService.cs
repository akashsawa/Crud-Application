using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Runtime.CompilerServices;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonDBContext _db;

        //public CountriesService(PersonDBContext personDBContext,bool initialize = true)
        public CountriesService(PersonDBContext personDBContext)
        {
            _db = personDBContext;
            //if (initialize)
            //{
            //    _countries.AddRange(new List<Country>() {
            //    new Country() { CountryID=Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D"), CountryName="USA" },

            //    new Country() { CountryID = Guid.Parse("52BC74D6-9094-441F-A51D-C1604F26F376"), CountryName = "INDIA" },

            //    new Country() { CountryID = Guid.Parse("54DEAD56-5A92-4C41-AF81-F27DC37BC67D"), CountryName = "JAPAN" },

            //    new Country() { CountryID = Guid.Parse("E45C96A1-3D83-4EF9-99E4-2B6FF51B5399"), CountryName = "MEXICO" }
            //});
            //}

        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddrequest)
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
            if (await _db.Countries.CountAsync(temp => temp.CountryName == countryAddrequest.CountryName) > 0)
            {
                throw new ArgumentException("Country name already exist !");
            }

            //convert object from countryaddreqeust to country type
            Country country = countryAddrequest.ToCountry();

            //generate countryid
            country.CountryID = Guid.NewGuid();

            //add country object into _countries
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse>? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;
            Country? country_response_from_list = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryId);

            if (country_response_from_list == null)
                return null;
            return country_response_from_list.ToCountryResponse();

        }



        public async Task<int> UploadCountriesFromExcelFil(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Countries"];
                int rowCount = excelWorksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = excelWorksheet.Cells[row, 1].Value.ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;
                        if (_db.Countries.Where(temp => temp.CountryName == cellValue).Count() == 0)

                        {
                            Country country = new Country()
                            {
                                CountryName = countryName
                            };
                            _db.Countries.Add(country);
                            await _db.SaveChangesAsync
                                ();
                            countriesInserted++;
                        }
                    }
                }
            }
            return countriesInserted;
        }
    }
}