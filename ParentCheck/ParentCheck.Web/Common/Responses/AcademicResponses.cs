using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AcademicResponses
    {
        public List<Academic> academics { get; set; }

        public static AcademicResponses PopulateAcademicResponses(List<AcademicDTO> academicDTOs)
        {
            var academicResponses = new AcademicResponses();

            academicResponses.academics = new List<Academic>();

            foreach (var academicDTO in academicDTOs)
            {
                var academic = new Academic
                {
                    id = academicDTO.Id,
                    yearAcademic= academicDTO.YearAcademic,
                    fromDate= academicDTO.FromDate,
                    toDate= academicDTO.ToDate,
                    isActive= academicDTO.IsActive
                };

                academicResponses.academics.Add(academic);
            }

            return academicResponses;
        }
    }

    public class Academic
    {
        public long id { get; set; }
        public int yearAcademic { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public bool isActive { get; set; }
    }
}
