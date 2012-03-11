using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EventManagerPro.Model.DomainModels
{
    public class EventModel
    {
        public static Event create(string matricId, string name, int venueId, DateTime start, DateTime end, int capacity, int budget = 0, string description = "", short viewAtLoginPage = 1)
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
                    TimeCreated = DateTime.Now,
                    ViewAtLoginPage = viewAtLoginPage,
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

        public static Event update(int id, string matricId, string name, int venueId, DateTime start, DateTime end, int capacity, int budget = 0, string description = "", short viewAtLoginPage = 1)
        {
            using (var context = new EventContainer())
            {
                Event updatedEvent = new Event
                {
                    Id = id,
                    StudentMatricId = matricId,
                    Name = name,
                    Description = description,
                    Start = start,
                    End = end,
                    Capacity = capacity,
                    Budget = budget,
                    VenueId = venueId,
                    TimeCreated = DateTime.Now,
                    ViewAtLoginPage = viewAtLoginPage,
                };
                context.Events.Attach(updatedEvent);
                context.Entry(updatedEvent).State = EntityState.Modified;
                context.SaveChanges();
                return updatedEvent;
            }
        }

        public static void deleteById(int id)
        {

            Console.WriteLine(id);
            using (var context = new EventContainer())
            {
                //var tb_delete_event = new Event() { Id = id };
                //context.Events.Attach(tb_delete_event);
                var tb_delete_event = context.Events.Find(new List<System32.Int32>() { id });
                Console.WriteLine(tb_delete_event.Guests.ToList().FirstOrDefault());
                    //context.Events.Remove(tb_delete_event);
                //context.SaveChanges();
            }
        }

        public static List<Event> getAll()
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                            orderby s.TimeCreated descending
                                            select s;
                return events.ToList();
            }
        }

        public static List<Event> getAllForLoginPage()
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                            where s.ViewAtLoginPage == 1
                                            orderby s.TimeCreated descending
                                            select s;
                return events.ToList();
            }
        }

        public static List<Event> getAllByMonth(int month)
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                            where s.ViewAtLoginPage == 1 && s.Start.Month == month
                                            orderby s.TimeCreated descending
                                            select s;
                return events.ToList();
            }
        }

        public static List<Event> getAllByYearMonth(DateTime date)
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                            where s.ViewAtLoginPage == 1 && s.Start.Year == date.Year && s.Start.Month == date.Month
                                            orderby s.TimeCreated descending
                                            select s;
                return events.ToList();
            }
        }

        public static List<Event> getNotByOwner(string matricId)
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                            where s.StudentMatricId != matricId
                                            orderby s.TimeCreated descending
                                            select s;
                return events.ToList();
            }
        }

        public static List<Event> getByOwner(string matricId)
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Event> events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                                            where s.StudentMatricId == matricId
                                            orderby s.TimeCreated descending
                                            select s;
                return events.ToList();
            }
        }

        public static Event getByID(int id)
        {
            using (var context = new EventContainer())
            {
                var events = from s in context.Events.Include("Venue").Include("Owner").Include("Guests")
                             where s.Id == id
                             select s;
                return events.FirstOrDefault();
            }
        }
    }
}
