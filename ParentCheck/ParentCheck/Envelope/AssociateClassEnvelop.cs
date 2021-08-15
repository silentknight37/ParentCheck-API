using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AssociateClassEnvelop
    {
        public AssociateClassEnvelop(List<AssociateClassDTO>associateClassDTOs)
        {
            this.associateClasses = associateClassDTOs;
        }

        public List<AssociateClassDTO> associateClasses { get; set; }
    }
}
