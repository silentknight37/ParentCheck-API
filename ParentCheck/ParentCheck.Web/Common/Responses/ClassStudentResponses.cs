using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class ClassStudentResponses
    {
        public List<ClassStudent> classStudents { get; set; }

        public static ClassStudentResponses PopulateClassStudentResponses(List<ClassStudentDTO> classStudentDTOs)
        {
            var classStudentResponses = new ClassStudentResponses();
            classStudentResponses.classStudents = new List<ClassStudent>();

            foreach (var classStudentDTO in classStudentDTOs)
            {
                var classStudent = new ClassStudent
                {
                    id = classStudentDTO.Id,
                    instituteUserId= classStudentDTO.InstituteUserId,
                    userFirstName= classStudentDTO.UserFirstName,
                    userLastName= classStudentDTO.UserLastName
                };

                classStudentResponses.classStudents.Add(classStudent);
            }

            return classStudentResponses;
        }
    }

    public class ClassStudent
    {
        public long id { get; set; }
        public long instituteUserId { get; set; }
        public string userFirstName { get; set; }
        public string userLastName { get; set; }
    }
}
