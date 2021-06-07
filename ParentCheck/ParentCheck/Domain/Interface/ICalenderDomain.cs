using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface ICalenderDomain
    {
        Task<List<CalenderEventDTO>> GetCalenderEventsAsync(DateTime eventRequestedDate,int eventType, long userId);
        Task<bool> SaveCalenderEventAsync(DateTime fromDate, DateTime toDate, string subject, string description, int type, long userId);
        Task<bool> RemoveCalenderEventAsync(long id, long userId);
    }
}
