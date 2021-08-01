using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Common
{
    public enum EnumStatus
    {
        Completed=1,
        PartiallyCompleted=2,
        NotCompleted=3,
        Open=4,
        InProgress=5,
        Closed=6,
        InReview=7
    }

    public enum EnumSupportTicketType
    {
        Open,
        Closed,
        Review
    }

    public enum EnumReferenceType
    {
        Subject=1,
        Term=2
    }
}
