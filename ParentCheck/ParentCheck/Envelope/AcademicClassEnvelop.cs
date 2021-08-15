using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AcademicClassEnvelop
    {
        public AcademicClassEnvelop(List<AcademicClassDTO> academicClassDTOs)
        {
            this.academicClasses = academicClassDTOs;
        }

        public List<AcademicClassDTO> academicClasses { get; set; }
    }
}
