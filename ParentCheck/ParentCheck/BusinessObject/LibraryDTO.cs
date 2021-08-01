using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class LibraryDTO
    {
        public long Id { get; set; }
        public long? InstituteId { get; set; }
        public long? InstituteClassSubjectId { get; set; }
        public string FileName { get; set; }
        public string LibraryDescription { get; set; }
        public string ContentURL { get; set; }
        public int ContentTypeId { get; set; }
        public bool IsInstituteLevelAccess { get; set; }
        public bool IsGlobal { get; set; }
    }
}
