using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        List<PersonResponse> GetAllPersons();

        PersonResponse? GetpersonByPersonId(Guid? personid);

        /// <summary>
        /// returns all person objects that matches with the given search field and search string.
        /// </summary>
        /// <param name="searchby"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        List<PersonResponse> GetFilteredpersons(string searchby, string? searchString);

        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        bool DeletePerson(Guid? personId);
    }
} 
