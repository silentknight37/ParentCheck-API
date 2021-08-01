using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class ClassRoomOverviewResponses
    {
        public List<ClassRoomOverview> classRoomOverviews { get; set; }

        public static ClassRoomOverviewResponses PopulateClassRoomOverviewResponses(List<ClassRoomOverviewDTO> classRoomOverviewDTOs)
        {
            var classRoomOverviewResponses = new ClassRoomOverviewResponses();
            classRoomOverviewResponses.classRoomOverviews = new List<ClassRoomOverview>();

            foreach (var classRoomOverviewDTO in classRoomOverviewDTOs)
            {
                var classRoomOverview = new ClassRoomOverview
                {
                    term = classRoomOverviewDTO.Term,
                    date= classRoomOverviewDTO.Date,
                    subject= classRoomOverviewDTO.Subject,
                    chapter= classRoomOverviewDTO.Chapter,
                    topic= classRoomOverviewDTO.Topic,
                    topicId= classRoomOverviewDTO.TopicId
                };

                classRoomOverviewResponses.classRoomOverviews.Add(classRoomOverview);
            }

            return classRoomOverviewResponses;
        }
    }

    public class ClassRoomOverview
    {
        public DateTime date { get; set; }
        public string term { get; set; }
        public string subject { get; set; }
        public string chapter { get; set; }
        public string topic { get; set; }
        public long topicId { get; set; }
    }
}
