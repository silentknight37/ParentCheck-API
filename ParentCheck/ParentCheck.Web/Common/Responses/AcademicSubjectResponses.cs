using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AcademicSubjectResponses
    {
        public List<AcademicSubject> subjects { get; set; }

        public static AcademicSubjectResponses PopulateAcademicSubjectResponses(List<SubjectDTO> subjectDTOs)
        {
            var academicSubjectResponses = new AcademicSubjectResponses();

            academicSubjectResponses.subjects = new List<AcademicSubject>();

            foreach (var subjectDTO in subjectDTOs)
            {
                var subject = new AcademicSubject
                {
                    id = subjectDTO.Id,
                    subject = subjectDTO.Subject,
                    descriptionText= subjectDTO.DescriptionText,
                    isActive = subjectDTO.IsActive
                };

                academicSubjectResponses.subjects.Add(subject);
            }

            return academicSubjectResponses;
        }
    }

    public class AcademicSubject
    {
        public long id { get; set; }
        public string subject { get; set; }
        public string descriptionText { get; set; }
        public bool isActive { get; set; }
    }
}
