using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class ClassRoomOverviewEnvelop
    {
        public ClassRoomOverviewEnvelop(List<ClassRoomOverviewDTO> classRoomOverviews)
        {
            this.ClassRoomOverviews = classRoomOverviews;
        }

        public List<ClassRoomOverviewDTO> ClassRoomOverviews { get; set; }
    }
}
