using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using model = EventManagerPro.Model;

namespace EventManagerApp
{
    /// <summary>
    /// Interaction logic for EventsScreen.xaml
    /// </summary>
    public partial class EventsScreen : Page
    {

        private List<model.Event> _upcomingEventsList;
        private List<model.Event> _createdEventsList;

        public EventsScreen()
        {
            InitializeComponent();
            this._createdEventsList = model.DomainModels.EventModel.getByOwner("U096988R");
            this._upcomingEventsList = model.DomainModels.EventModel.getAll();
            this.upcomingEventsListGrid.ItemsSource = this._upcomingEventsList;
            this.createdEventsListGrid.ItemsSource = this._upcomingEventsList;
        }

        private void createEventBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("EditEventScreen.xaml", UriKind.Relative));
        }
    }
}
