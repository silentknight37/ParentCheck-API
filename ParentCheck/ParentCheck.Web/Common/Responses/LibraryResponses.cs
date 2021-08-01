using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class LibraryResponses
    {
        public List<Library> libraries { get; set; }

        public static LibraryResponses PopulateLibraryResponses(List<LibraryDTO> libraryDTOs)
        {
            var libraryResponses = new LibraryResponses();
            libraryResponses.libraries = new List<Library>();

            foreach (var libraryDTO in libraryDTOs)
            {
                var Library = new Library
                {
                    id= libraryDTO.Id,
                    classSubjectId= libraryDTO.InstituteClassSubjectId,
                    instituteId= libraryDTO.InstituteId,
                    fileName= libraryDTO.FileName,
                    fileUrl= libraryDTO.ContentURL,
                    description= libraryDTO.LibraryDescription,
                    contentTypeId= libraryDTO.ContentTypeId,
                    isGlobal= libraryDTO.IsGlobal,
                    isInstituteLevelAccess= libraryDTO.IsInstituteLevelAccess
                };

                libraryResponses.libraries.Add(Library);
            }

            return libraryResponses;
        }
    }

    public class Library
    {
        public long id { get; set; }
        public long? instituteId { get; set; }
        public long? classSubjectId { get; set; }
        public string fileName { get; set; }
        public string description { get; set; }
        public string fileUrl { get; set; }
        public int contentTypeId { get; set; }
        public bool isInstituteLevelAccess { get; set; }
        public bool isGlobal { get; set; }
    }
}
