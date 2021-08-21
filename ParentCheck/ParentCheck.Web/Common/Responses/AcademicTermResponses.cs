using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AcademicTermResponses
    {
        public List<AcademicTerm> academicTerms { get; set; }

        public static AcademicTermResponses PopulateAcademicTermResponses(List<AcademicTermDTO> academicTermDTOs)
        {
            var academicTermResponses = new AcademicTermResponses();

            academicTermResponses.academicTerms = new List<AcademicTerm>();

            foreach (var academicTermDTO in academicTermDTOs)
            {
                var academicTerm = new AcademicTerm
                {
                    id = academicTermDTO.Id,
                    term = academicTermDTO.Term,
                    yearAcademic= academicTermDTO.YearAcademic,
                    yearAcademicId= academicTermDTO.YearAcademicId,
                    fromDate = academicTermDTO.FromDate,
                    fromDateFormated = academicTermDTO.FromDate.ToString("dd/MM/yyyy"),
                    toDate = academicTermDTO.ToDate,
                    toDateFormated = academicTermDTO.ToDate.ToString("dd/MM/yyyy"),
                    isActive = academicTermDTO.IsActive
                };

                academicTermResponses.academicTerms.Add(academicTerm);
            }

            return academicTermResponses;
        }
    }

    public class AcademicTerm
    {
        public long id { get; set; }
        public string term { get; set; }
        public int yearAcademic { get; set; }
        public long yearAcademicId { get; set; }
        public DateTime fromDate { get; set; }
        public string fromDateFormated { get; set; }
        public DateTime toDate { get; set; }
        public string toDateFormated { get; set; }
        public bool isActive { get; set; }
    }
}
