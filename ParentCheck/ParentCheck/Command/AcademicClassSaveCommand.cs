using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class AcademicClassSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public AcademicClassSaveCommand(long id,string academicClass, long yearAcademic, long responsibleUserId, bool isActive, long userId)
        {
            this.Id = id;
            this.AcademicClass = academicClass;
            this.YearAcademic = yearAcademic;
            this.ResponsibleUserId = responsibleUserId;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public string AcademicClass { get; set; }
        public long YearAcademic { get; set; }
        public long ResponsibleUserId { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
