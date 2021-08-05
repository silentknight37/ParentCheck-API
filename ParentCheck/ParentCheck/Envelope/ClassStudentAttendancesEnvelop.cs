using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class ClassStudentAttendancesEnvelop
    {
        public ClassStudentAttendancesEnvelop(List<ClassStudentAttendancesDTO> classStudentAttendancesDTOs)
        {
            this.ClassStudentAttendances = classStudentAttendancesDTOs;
        }

        public List<ClassStudentAttendancesDTO> ClassStudentAttendances { get; set; }
    }
}
