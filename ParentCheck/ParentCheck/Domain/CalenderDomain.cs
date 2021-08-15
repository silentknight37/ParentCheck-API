using ParentCheck.BusinessObject;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class CalenderDomain : ICalenderDomain
    {
        private readonly ICalenderRepository _calenderRepository;

        public CalenderDomain(ICalenderRepository calenderRepository)
        {
            _calenderRepository = calenderRepository;
        }

        public async Task<List<CalenderEventDTO>> GetCalenderEventsAsync(DateTime eventRequestedDate,int eventType, long userId)
        {
            return await _calenderRepository.GetCalenderEventsAsync(eventRequestedDate, eventType, userId);
        }
        public async Task<List<CalenderEventDTO>> GetCalenderTodayEventsAsync(long userId)
        {
            return await _calenderRepository.GetCalenderTodayEventsAsync(userId);
        }

        public async Task<List<CalenderEventDTO>> GetCalenderAllEventsAsync(long userId)
        {
            return await _calenderRepository.GetCalenderAllEventsAsync(userId);
        }

        public async Task<bool> SaveCalenderEventAsync(DateTime fromDate, DateTime toDate, string subject, string description, int type, long userId)
        {
            return await _calenderRepository.SaveCalenderEventAsync(fromDate, toDate, subject, description, type, userId);
        }

        public async Task<bool> RemoveCalenderEventAsync(long id, long userId)
        {
            return await _calenderRepository.RemoveCalenderEventAsync(id, userId);
        }
    }
}
