// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace ParentCheck.Data
{
    public partial class InstituteExemptDependUser
    {
        public long Id { get; set; }
        public long? ExemptInstituteUserId { get; set; }
        public long? DependInstituteUserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateOn { get; set; }

        public virtual InstituteUser DependInstituteUser { get; set; }
        public virtual InstituteUser ExemptInstituteUser { get; set; }
    }
}