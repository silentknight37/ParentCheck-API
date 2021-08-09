using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class AcademicSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public AcademicSaveCommand(long id,int yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            this.Id = id;
            this.YearAcademic = yearAcademic;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public int YearAcademic { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
