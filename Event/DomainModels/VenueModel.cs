using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EventManagerPro.Model.DomainModels
{
    public class VenueModel
    {
        public static Venue create(string name, int capacity)
        {
            using (var context = new EventContainer())
            {
                Venue newVenue = new Venue
                {
                    Name = name,
                    Capacity = capacity,
                };
                context.Venues.Add(newVenue);
                context.SaveChanges();
                return newVenue;
            }
        }

        public static Venue update(string name, int capacity)
        {
            using (var context = new EventContainer())
            {
                Venue venue = new Venue
                {
                    Name = name,
                    Capacity = capacity
                };
                context.Venues.Attach(venue);
                context.Entry(venue).State = EntityState.Modified;
                context.SaveChanges();
                return venue;
            }
        }

        public static Venue createObj(Venue e)
        {
            using (var context = new EventContainer())
            {
                context.Venues.Add(e);
                context.SaveChanges();
                return e;
            }
        }

        public static Venue updateObj(Venue e)
        {
            using (var context = new EventContainer())
            {
                context.Entry(e).State = EntityState.Modified;
                context.SaveChanges();

                return e;
            }
        }

        public static Venue getByID(int id)
        {
            using (var context = new EventContainer())
            {
                var venues = from v in context.Venues
                             where v.Id == id
                             select v;

                return venues.FirstOrDefault();
            }  
        }

        public static List<Venue> getAll()
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Venue> venues = from v in context.Venues
                                     select v;
                return venues.ToList();
            }
        }

    }
}
