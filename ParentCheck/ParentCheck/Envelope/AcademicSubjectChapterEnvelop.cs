using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AcademicSubjectChapterEnvelop
    {
        public AcademicSubjectChapterEnvelop(List<SubjectChapterDTO> subjectChapterDTOs)
        {
            this.subjectChapters = subjectChapterDTOs;
        }

        public List<SubjectChapterDTO> subjectChapters { get; set; }
    }
}
