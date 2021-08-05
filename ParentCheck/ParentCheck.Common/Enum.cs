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
        Term=2,
        CommunicationGroup=3,
        UserClass=4,
        Institute=5,
        InvoiceType=6
    }

    public enum EnumCommunicationBoxType
    {
        Inbox = 1,
        Outbox = 2
    }

    public enum EnumContactType
    {
        Email = 1,
        Mobile = 2
    }

    public enum EnumCommunication
    {
        Email = 1,
        SMS = 2,
        Event = 3,
        Session = 4,
        Template = 5
    }
    public enum EnumEventType
    {
        Task = 1,
        Notification = 2,
        Event = 3,
        Timetable = 4,
        Sessions = 5,
        Assignment = 6
    }
}
