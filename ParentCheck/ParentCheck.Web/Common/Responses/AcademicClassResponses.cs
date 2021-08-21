using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AcademicClassResponses
    {
        public List<AcademicClass>academicClasses { get; set; }

        public static AcademicClassResponses PopulateAcademicClassResponses(List<AcademicClassDTO> academicClassDTOs)
        {
            var academicClassResponses = new AcademicClassResponses();

            academicClassResponses.academicClasses = new List<AcademicClass>();

            foreach (var academicClassDTO in academicClassDTOs)
            {
                var academicClass = new AcademicClass
                {
                    id = academicClassDTO.Id,
                    className = academicClassDTO.Class,
                    yearAcademic= academicClassDTO.YearAcademic,
                    yearAcademicId= academicClassDTO.YearAcademicId,
                    yearAcademicDetail = academicClassDTO.YearAcademicDetail,
                    responsibleUser = academicClassDTO.ResponsibleUser,
                    responsibleUserId = academicClassDTO.ResponsibleUserId,
                    isActive = academicClassDTO.IsActive
                };

                academicClassResponses.academicClasses.Add(academicClass);
            }

            return academicClassResponses;
        }
    }

    public class AcademicClass
    {
        public long id { get; set; }
        public string className { get; set; }
        public long yearAcademicId { get; set; }
        public int yearAcademic { get; set; }
        public string yearAcademicDetail { get; set; }
        public string responsibleUser { get; set; }
        public long responsibleUserId { get; set; }
        public bool isActive { get; set; }
    }
}
