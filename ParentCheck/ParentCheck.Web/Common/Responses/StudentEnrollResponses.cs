using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class StudentEnrollResponses
    {
        public List<StudentEnroll> studentEnrolls { get; set; }

        public static StudentEnrollResponses PopulateStudentEnrollResponses(List<StudentEnrollDTO> studentEnrollDTOs)
        {
            var studentEnrollResponses = new StudentEnrollResponses();

            studentEnrollResponses.studentEnrolls = new List<StudentEnroll>();

            foreach (var studentEnrollDTO in studentEnrollDTOs)
            {
                var academic = new StudentEnroll
                {
                    id = studentEnrollDTO.Id,
                    studentUserName = studentEnrollDTO.StudentUserName,
                    studentUserId= studentEnrollDTO.StudentUserId,
                    classId= studentEnrollDTO.ClassId,
                    className= studentEnrollDTO.ClassName,
                    academicYear = studentEnrollDTO.AcademicYearId,
                    isActive = studentEnrollDTO.IsActive
                };

                studentEnrollResponses.studentEnrolls.Add(academic);
            }

            return studentEnrollResponses;
        }
    }

    public class StudentEnroll
    {
        public long id { get; set; }
        public string studentUserName { get; set; }
        public long studentUserId { get; set; }
        public string className { get; set; }
        public long classId { get; set; }
        public long academicYear { get; set; }
        public bool isActive { get; set; }
    }
}
