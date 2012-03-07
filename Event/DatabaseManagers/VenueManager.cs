using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventManagerPro.Model
{
    class VenueManager
    {
        public static Venue create(string name)
        {
            using (var context = new EventContainer())
            {
                Venue newVenue = new Venue
                {
                    Name = name
                };
                context.Venues.Add(newVenue);
                context.SaveChanges();
                return newVenue;
            }
        }

        public static Dictionary<int,Venue> getAll()
        {
            using (var context = new EventContainer())
            {
                var venues = (from v in context.Venues
                                  select v).ToDictionary(k => k.Id);
                return venues;
            }
        }

    }
}
