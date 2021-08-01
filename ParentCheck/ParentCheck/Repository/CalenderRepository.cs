using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository
{
    public class CalenderRepository : ICalenderRepository
    {
        private ParentCheckContext _parentcheckContext;

        public CalenderRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<List<CalenderEventDTO>> GetCalenderEventsAsync(DateTime eventRequestedDate,int eventType, long userId)
        {
            var calenderEventList = await (from c in _parentcheckContext.CalenderEvent
                                           join e in _parentcheckContext.EventType on c.EventTypeId equals e.Id
                                           where c.FromDate.Month == eventRequestedDate.Month
                                             && c.FromDate.Year == eventRequestedDate.Year
                                             && c.ToDate.Month == eventRequestedDate.Month
                                             && c.ToDate.Year == eventRequestedDate.Year
                                             && e.IsActive == true
                                             && (eventType==0 || c.EventTypeId == eventType)
                                             && c.InstituteUserId== userId
                                           select new CalenderEventDTO
                                           {
                                               Id=c.Id,
                                               Subject = c.SubjectName,
                                               Description = c.DescriptionText,
                                               FromDate = c.FromDate,
                                               ToDate = c.ToDate,
                                               Type = c.EventTypeId,
                                               ColorCode = e.ColorCode
                                           }).ToListAsync();

            return calenderEventList;
        }

        public async Task<bool> SaveCalenderEventAsync(DateTime fromDate, DateTime toDate, string subject, string description, int type, long userId)
        {
            var user = (from u in _parentcheckContext.User
                        join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                        where iu.Id == userId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            iu.Id,
                            iu.InstituteId,
                            uId = u.Id
                        }).FirstOrDefault();

            if (user != null)
            {
                CalenderEvent calenderEvent = new CalenderEvent();
                calenderEvent.FromDate = fromDate;
                calenderEvent.ToDate = toDate;
                calenderEvent.SubjectName = subject;
                calenderEvent.DescriptionText = description;
                calenderEvent.EventTypeId = type;
                calenderEvent.InstituteUserId = user.Id;
                calenderEvent.CreatedOn = DateTime.UtcNow;
                calenderEvent.CreatedBy = $"{user.FirstName} {user.LastName}";
                calenderEvent.UpdateOn = DateTime.UtcNow;
                calenderEvent.UpdatedBy = $"{user.FirstName} {user.LastName}";

                _parentcheckContext.CalenderEvent.Add(calenderEvent);
                await _parentcheckContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> RemoveCalenderEventAsync(long id, long userId)
        {
            var user = (from u in _parentcheckContext.User
                        join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                        where iu.Id == userId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            iu.Id,
                            iu.InstituteId,
                            uId = u.Id
                        }).FirstOrDefault();

            if (user != null)
            {
                var calenderEvent = _parentcheckContext.CalenderEvent.Where(i => i.Id == id && i.InstituteUserId == userId).FirstOrDefault();
                _parentcheckContext.CalenderEvent.Remove(calenderEvent);
                await _parentcheckContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
