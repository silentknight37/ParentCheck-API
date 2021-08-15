using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AssociateSubjectChapterResponses
    {
        public List<AssociateSubjectChapter> associateSubjectChapters { get; set; }

        public static AssociateSubjectChapterResponses PopulateAssociateSubjectChapterResponses(List<SubjectChapterDTO> subjectChapterDTOs)
        {
            var associateSubjectChapterResponses = new AssociateSubjectChapterResponses();

            associateSubjectChapterResponses.associateSubjectChapters = new List<AssociateSubjectChapter>();

            foreach (var subjectChapterDTO in subjectChapterDTOs)
            {
                var associateSubjectChapter = new AssociateSubjectChapter
                {
                    id = subjectChapterDTO.InstituteSubjectChapterId,
                    assignmentId= subjectChapterDTO.InstituteAssignmentId,
                    chapter= subjectChapterDTO.Chapter,
                    isAssignmentAssigned= subjectChapterDTO.IsAssignmentAssigned,
                    isActive = subjectChapterDTO.IsActive
                };

                associateSubjectChapterResponses.associateSubjectChapters.Add(associateSubjectChapter);
            }

            return associateSubjectChapterResponses;
        }
    }

    public class AssociateSubjectChapter
    {
        public long id { get; set; }
        public string chapter { get; set; }
        public bool isActive { get; set; }
        public bool isAssignmentAssigned { get; set; }
        public long? assignmentId { get; set; }
    }
}
