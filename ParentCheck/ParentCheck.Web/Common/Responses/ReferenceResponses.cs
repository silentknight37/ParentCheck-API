using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class ReferenceResponses
    {
        public List<Reference> references { get; set; }

        public static ReferenceResponses PopulateReferenceResponses(List<ReferenceDTO> referenceList)
        {
            var referenceResponses = new ReferenceResponses();
            referenceResponses.references = new List<Reference>();

            foreach (var reference in referenceList)
            {
                var refVal = new Reference
                {
                    id= reference.Id,
                    value= reference.ValueText
                };

                referenceResponses.references.Add(refVal);
            }

            return referenceResponses;
        }
    }

    public class Reference
    {
        public long id { get; set; }
        public string value { get; set; }
    }
}
