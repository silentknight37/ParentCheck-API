using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class ClassStudentEnvelop
    {
        public ClassStudentEnvelop(List<ClassStudentDTO> classStudentDTOs)
        {
            this.ClassStudents = classStudentDTOs;
        }

        public List<ClassStudentDTO> ClassStudents { get; set; }
    }
}
