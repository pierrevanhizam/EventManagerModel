using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventManagerPro.Model;

namespace EventManagerApp
{
    /// <summary>
    /// Object to store permissions and event data in Upcoming Events grid.
    /// </summary>
    public class EventItem
    {
        public Event CurEvent { get; set; }
        public bool IsOwner { get; set; }
        public bool IsRegistered { get; set; }

        public bool CanRegister
        {
            get { return !this.IsOwner && !this.IsRegistered; }
        }

        // Constructor
        public EventItem(Event e, bool isOwner, bool isRegistered)
        {
            this.CurEvent = e;
            this.IsOwner = isOwner;
            this.IsRegistered = isRegistered;
        }
    }
}
