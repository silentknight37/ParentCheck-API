using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class AcademicTermSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public AcademicTermSaveCommand(long id,string term, long yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            this.Id = id;
            this.Term = term;
            this.YearAcademic = yearAcademic;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public string Term { get; set; }
        public long YearAcademic { get; set; }
        public long YearAcademicId
        {
            get
            {
                return YearAcademic;
            }
        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
