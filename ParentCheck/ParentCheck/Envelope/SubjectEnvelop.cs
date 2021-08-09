using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class SubjectEnvelop
    {
        public SubjectEnvelop(List<SubjectDTO> subjectDTOs)
        {
            this.subjects = subjectDTOs;
        }

        public List<SubjectDTO> subjects { get; set; }
    }
}
