using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventManagerPro.Model
{
    public class EventManager
    {
        public static Event create(string matricId, string name, int venueId, DateTime start, DateTime end, int capacity, int budget = 0, string description="")
        {
            using (var context = new EventContainer())
            {
                Event newEvent = new Event
                {
                    StudentMatricId = matricId,
                    Name = name,
                    Description = description,
                    Start = start,
                    End = end,
                    Capacity = capacity,
                    Budget = budget,
                    VenueId = venueId,
                    TimeCreated = DateTime.Now
                };
                context.Events.Add(newEvent);
                context.SaveChanges();
                return newEvent;
            }
        }

        public static Boolean registerGuest(string matricId, int eventId)
        {
            using (var context = new EventContainer())
            {
                var runningEvent = (from e in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                        where e.Id == eventId
                                        select e).FirstOrDefault();
                var student = (from s in context.Students
                                   where s.MatricId == matricId
                                   select s).FirstOrDefault();
                if (runningEvent == null || student == null)
                {
                    return false;
                }
                else
                {
                    runningEvent.Guests.Add(student);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        public static Dictionary<int, Event> getAll()
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                                orderby s.TimeCreated descending
                                                select s;
                return events.ToDictionary(k => k.Id);
            }
        }

        public static List<Event> getByOwner(string matricId)
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events
                                            where s.StudentMatricId == matricId
                                            orderby s.TimeCreated descending
                                            select s;
                return events.ToList();
            }
        }
    }
}
