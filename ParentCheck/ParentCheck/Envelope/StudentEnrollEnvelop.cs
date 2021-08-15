using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class StudentEnrollEnvelop
    {
        public StudentEnrollEnvelop(List<StudentEnrollDTO> studentEnrollDTOs)
        {
            this.studentEnrolls = studentEnrollDTOs;
        }

        public List<StudentEnrollDTO> studentEnrolls { get; set; }
    }
}
