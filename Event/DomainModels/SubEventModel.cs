using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventManagerPro.Model.DomainModels
{
    class SubEventModel
    {

        public static SubEvent create(int eventId, int venueId, DateTime start, DateTime end)
        {
            using (var context = new EventContainer())
            {
                SubEvent newSubEvent = new SubEvent
                {
                    EventId = eventId,
                    Start = start,
                    End = end,
                    VenueId = venueId,
                };
                context.SubEvents.Add(newSubEvent);
                context.SaveChanges();
                return newSubEvent;
            }
        }

    }
}
