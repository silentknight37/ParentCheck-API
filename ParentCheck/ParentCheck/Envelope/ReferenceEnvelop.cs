using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class ReferenceEnvelop
    {
        public ReferenceEnvelop(List<ReferenceDTO> references)
        {
            this.References = references;
        }

        public List<ReferenceDTO> References { get; set; }
    }
}
