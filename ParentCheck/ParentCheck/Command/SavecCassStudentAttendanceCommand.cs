using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class SaveClassStudentAttendanceCommand : IRequest<RequestSaveEnvelop>
    {
        public SaveClassStudentAttendanceCommand(long instituteUserId, long instituteClassId, DateTime recordDate, bool isAttendance, bool isReset, long userId)
        {
            this.InstituteUserId = instituteUserId;
            this.InstituteClassId = instituteClassId;
            this.RecordDate = recordDate;
            this.IsAttendance = isAttendance;
            this.IsReset = isReset;
            this.UserId = userId;
        }

        public long InstituteUserId { get; set; }
        public long InstituteClassId { get; set; }
        public DateTime RecordDate { get; set; }
        public bool IsAttendance { get; set; }
        public bool IsReset { get; set; }
        public long UserId { get; set; }
    }
}
