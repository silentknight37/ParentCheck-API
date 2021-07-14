using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class ClassRoomOverviewQuery : IRequest<ClassRoomOverviewEnvelop>
    {
        public ClassRoomOverviewQuery(DateTime? fromDate, DateTime? toDate,string subjectId,string instituteTermsId, long userId)
        {
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.SubjectIdString = subjectId;
            this.InstituteTermsIdString = instituteTermsId;
            this.UserId = userId;
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SubjectIdString { get; set; }
        public string InstituteTermsIdString { get; set; }
        public long UserId { get; set; }

        public long? SubjectId
        {
            get
            {
                if (string.IsNullOrEmpty(SubjectIdString))
                {
                    return null;
                }

                return long.Parse(SubjectIdString);
            }
        }

        public long? InstituteTermsId
        {
            get
            {
                if (string.IsNullOrEmpty(InstituteTermsIdString))
                {
                    return null;
                }

                return long.Parse(InstituteTermsIdString);
            }
        }
    }
}
