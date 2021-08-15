using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AssociateClassResponses
    {
        public List<AssociateClass> associateClasses { get; set; }

        public static AssociateClassResponses PopulateAssociateClassResponses(List<AssociateClassDTO> associateClassDTOs)
        {
            var associateClassResponses = new AssociateClassResponses();

            associateClassResponses.associateClasses = new List<AssociateClass>();

            foreach (var associateClassDTO in associateClassDTOs)
            {
                var associateClasse = new AssociateClass
                {
                    id = associateClassDTO.Id,
                    associateClass= associateClassDTO.AssociateClass,
                    subject= associateClassDTO.Subject
                };

                associateClassResponses.associateClasses.Add(associateClasse);
            }

            return associateClassResponses;
        }
    }

    public class AssociateClass
    {
        public long id { get; set; }
        public string associateClass { get; set; }
        public string subject { get; set; }
    }
}
