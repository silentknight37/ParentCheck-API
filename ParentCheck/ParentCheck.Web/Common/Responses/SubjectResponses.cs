using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class SubjectResponses
    {
        public string userClass { get; set; }
        public List<Subject> subjects { get; set; }

        public static SubjectResponses PopulateSubjectResponses(UserClassDTO userClass)
        {
            var subjectResponses = new SubjectResponses();
            subjectResponses.subjects = new List<Subject>();
            subjectResponses.userClass = userClass.UserClass;

            foreach (var userSubject in userClass.Subjects)
            {
                var subject = new Subject
                {
                    id= userSubject.InstituteClassSubjectId,
                    subjectText=userSubject.Subject,
                    descriptionText = userSubject.Description,
                    bgColor = userSubject.ColorBg,
                    fontColor= userSubject.ColorFont
                };

                subjectResponses.subjects.Add(subject);
            }

            return subjectResponses;
        }
    }

    public class Subject
    {
        public long id { get; set; }
        public string subjectText { get; set; }
        public string descriptionText { get; set; }
        public string bgColor { get; set; }
        public string fontColor { get; set; }
    }
}
