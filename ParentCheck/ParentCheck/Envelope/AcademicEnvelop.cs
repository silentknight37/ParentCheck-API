using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AcademicEnvelop
    {
        public AcademicEnvelop(List<AcademicDTO> academicDTOs)
        {
            this.academics = academicDTOs;
        }

        public List<AcademicDTO> academics { get; set; }
    }
}
