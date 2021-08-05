using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class ClassStudentAttendancesResponses
    {
        public List<ClassStudentAttendances> studentAttendances { get; set; }

        public static ClassStudentAttendancesResponses PopulateClassStudentAttendancesResponses(List<ClassStudentAttendancesDTO> classStudentAttendancesDTOs)
        {
            var classStudentAttendancesResponses = new ClassStudentAttendancesResponses();
            classStudentAttendancesResponses.studentAttendances = new List<ClassStudentAttendances>();

            foreach (var classStudentAttendancesDTO in classStudentAttendancesDTOs)
            {
                var classStudentAttendances = new ClassStudentAttendances
                {
                    instituteUserId = classStudentAttendancesDTO.InstituteUserId,
                    instituteClassId = classStudentAttendancesDTO.InstituteClassId,
                    isAttendance = classStudentAttendancesDTO.IsAttendance,
                    isMarked = classStudentAttendancesDTO.IsMarked,
                    recordDate = classStudentAttendancesDTO.RecordDate,
                    className= classStudentAttendancesDTO.UserClassName,
                    studentUserName = $"{classStudentAttendancesDTO.UserFirstName} {classStudentAttendancesDTO.UserLastName}",
                    responsibleUserName = $"{classStudentAttendancesDTO.ResponsibleUserFirstName} {classStudentAttendancesDTO.ResponsibleUserLastName}",
                };

                classStudentAttendancesResponses.studentAttendances.Add(classStudentAttendances);
            }

            return classStudentAttendancesResponses;
        }
    }

    public class ClassStudentAttendances
    {
        public long instituteUserId { get; set; }
        public long instituteClassId { get; set; }
        public DateTime recordDate { get; set; }
        public bool isAttendance { get; set; }
        public bool isMarked { get; set; }
        public string className { get; set; }
        public string studentUserName { get; set; }
        public string responsibleUserName { get; set; }
    }
}
