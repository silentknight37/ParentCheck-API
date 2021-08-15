using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
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
            List<CalenderEventDTO> calenderEventDTOs = new List<CalenderEventDTO>();
            var user = await (from u in _parentcheckContext.InstituteUser
                              where (u.ParentUserid == userId || u.Id == userId)
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();
            if (user != null)
            {
                var calenderEventList = await (from c in _parentcheckContext.CalenderEvent
                                               join e in _parentcheckContext.EventType on c.EventTypeId equals e.Id
                                               where c.FromDate.Month == eventRequestedDate.Month
                                                 && c.FromDate.Year == eventRequestedDate.Year
                                                 && c.ToDate.Month == eventRequestedDate.Month
                                                 && c.ToDate.Year == eventRequestedDate.Year
                                                 && e.IsActive == true
                                                 && (eventType == 0 || c.EventTypeId == eventType)
                                                 && c.InstituteUserId == userId
                                               select new
                                               {
                                                   Id = c.Id,
                                                   Subject = c.SubjectName,
                                                   Description = c.DescriptionText,
                                                   FromDate = c.FromDate,
                                                   ToDate = c.ToDate,
                                                   Type = c.EventTypeId,
                                                   ColorCode = e.ColorCode
                                               }).ToListAsync();
                foreach (var calenderEvent in calenderEventList)
                {
                    calenderEventDTOs.Add(new CalenderEventDTO
                    {
                        Id = calenderEvent.Id,
                        ColorCode = calenderEvent.ColorCode,
                        Description = calenderEvent.Description,
                        FromDate = calenderEvent.FromDate,
                        ToDate = calenderEvent.ToDate,
                        Subject = calenderEvent.Subject,
                        Type = calenderEvent.Type,
                    });
                }

                return calenderEventDTOs;
            }
            return calenderEventDTOs;
        }
        public async Task<List<CalenderEventDTO>> GetCalenderTodayEventsAsync(long userId)
        {
            List<CalenderEventDTO> calenderEventDTOs = new List<CalenderEventDTO>();
            var user = await (from u in _parentcheckContext.InstituteUser
                              where (u.ParentUserid == userId || u.Id == userId)
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();
            if (user != null)
            {
                var eventRequestedDate = DateTime.Now;
                var calenderEventList = await (from c in _parentcheckContext.CalenderEvent
                                               join e in _parentcheckContext.EventType on c.EventTypeId equals e.Id
                                               where c.FromDate.Date <= eventRequestedDate.Date
                                                 && c.ToDate.Date >= eventRequestedDate.Date
                                                 && e.IsActive == true
                                                 && (c.EventTypeId == (int)EnumEventType.Event || c.EventTypeId == (int)EnumEventType.Sessions || c.EventTypeId == (int)EnumEventType.Assignment || c.EventTypeId == (int)EnumEventType.Notification)
                                                 && c.InstituteUserId == user.Id
                                               select new
                                               {
                                                   Id = c.Id,
                                                   Subject = c.SubjectName,
                                                   Description = c.DescriptionText,
                                                   FromDate = c.FromDate,
                                                   ToDate = c.ToDate,
                                                   Type = c.EventTypeId,
                                                   ColorCode = e.ColorCode
                                               }).ToListAsync();
                foreach (var calenderEvent in calenderEventList)
                {
                    calenderEventDTOs.Add(new CalenderEventDTO
                    {
                        Id = calenderEvent.Id,
                        ColorCode = calenderEvent.ColorCode,
                        Description = calenderEvent.Description,
                        FromDate = calenderEvent.FromDate,
                        ToDate = calenderEvent.ToDate,
                        Subject = calenderEvent.Subject,
                        Type = calenderEvent.Type,
                    });
                }

                return calenderEventDTOs;
            }
            return calenderEventDTOs;
        }

        public async Task<List<CalenderEventDTO>> GetCalenderAllEventsAsync(long userId)
        {
            List<CalenderEventDTO> calenderEventDTOs = new List<CalenderEventDTO>();
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var calenderEventList = await (from c in _parentcheckContext.CalenderEvent
                                               join e in _parentcheckContext.EventType on c.EventTypeId equals e.Id
                                               where e.IsActive == true
                                               && c.InstituteId== user.InstituteId
                                                && c.EventTypeId == (int)EnumEventType.Event
                                               select new CalenderEventDTO
                                               {
                                                   Id = c.Id,
                                                   Subject = c.SubjectName,
                                                   Description = c.DescriptionText,
                                                   FromDate = c.FromDate,
                                                   ToDate = c.ToDate,
                                                   Type = c.EventTypeId,
                                                   ColorCode = e.ColorCode
                                               }).ToListAsync();

                calenderEventDTOs.AddRange(calenderEventList);
            }

            return calenderEventDTOs;
        }

        public async Task<bool> SaveCalenderEventAsync(DateTime fromDate, DateTime toDate, string subject, string description, int type, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                CalenderEvent calenderEvent = new CalenderEvent();
                calenderEvent.InstituteId = user.InstituteId;
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
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var calenderEvent = _parentcheckContext.CalenderEvent.Where(i => i.Id == id && i.InstituteId == user.InstituteId).FirstOrDefault();
                _parentcheckContext.CalenderEvent.Remove(calenderEvent);
                await _parentcheckContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
