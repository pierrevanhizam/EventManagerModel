using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model = EventManagerPro.Model;

namespace EventManagerApp
{
    /// <summary>
    /// Stores data to be passed between pages.
    /// </summary>
    public class NavigationData
    {
        public string statusMessage { get; set; }
        public model.Student loggedInUser { get; set; }
        public int statusCode { get; set; }
        public int eventID { get; set; }

        public NavigationData()
        {
            this.statusCode = NavigationData.STATUS_NONE;
            this.eventID = -1;
        }

        public const int STATUS_NONE = 0;
        public const int STATUS_NOTICE = 1;
        public const int STATUS_ERROR = 2;
    }
}
