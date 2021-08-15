using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AcademicClassSubjectEnvelop
    {
        public AcademicClassSubjectEnvelop(List<AcademicClassSubjectDTO> academicClassSubjectDTOs)
        {
            this.academicClassSubjects = academicClassSubjectDTOs;
        }

        public List<AcademicClassSubjectDTO> academicClassSubjects { get; set; }
    }
}
