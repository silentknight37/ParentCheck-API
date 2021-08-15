using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class ChapterTopicsDTO
    {
        public long InstituteChapterTopicId { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string SubmitDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssignmentAssigned { get; set; }
        public long? InstituteAssignmentId { get; set; }
    }
}
