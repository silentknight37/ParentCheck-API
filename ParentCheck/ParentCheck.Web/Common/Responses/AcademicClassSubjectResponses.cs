using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AcademicClassSubjectResponses
    {
        public List<AcademicClassSubject> academicClassSubjects { get; set; }

        public static AcademicClassSubjectResponses PopulateAcademicClassSubjectResponses(List<AcademicClassSubjectDTO>  academicClassSubjectDTOs)
        {
            var academicClassSubjectResponses = new AcademicClassSubjectResponses();

            academicClassSubjectResponses.academicClassSubjects = new List<AcademicClassSubject>();

            foreach (var academicClassSubjectDTO in academicClassSubjectDTOs)
            {
                var academicClassSubject = new AcademicClassSubject
                {
                    id = academicClassSubjectDTO.Id,
                    subject = academicClassSubjectDTO.Subject,
                    subjectId = academicClassSubjectDTO.SubjectId,
                    responsibleUser = academicClassSubjectDTO.ResponsibleUser,
                    responsibleUserId = academicClassSubjectDTO.ResponsibleUserId,
                    isActive = academicClassSubjectDTO.IsActive
                };

                academicClassSubjectResponses.academicClassSubjects.Add(academicClassSubject);
            }

            return academicClassSubjectResponses;
        }
    }

    public class AcademicClassSubject
    {
        public long id { get; set; }
        public string subject { get; set; }
        public long subjectId { get; set; }
        public string responsibleUser { get; set; }
        public long responsibleUserId { get; set; }
        public bool isActive { get; set; }
    }
}
