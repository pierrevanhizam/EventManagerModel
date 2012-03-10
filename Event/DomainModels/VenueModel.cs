using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventManagerPro.Model.DomainModels
{
    public class VenueModel
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
