using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AcademicTermEnvelop
    {
        public AcademicTermEnvelop(List<AcademicTermDTO> academicTermDTOs)
        {
            this.academicTerms = academicTermDTOs;
        }

        public List<AcademicTermDTO> academicTerms { get; set; }
    }
}
