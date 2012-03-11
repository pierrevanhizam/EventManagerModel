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
using System.Windows.Media.Animation;

namespace EventManagerApp
{
    /// <summary>
    /// Interaction logic for EventsScreen.xaml
    /// </summary>
    public partial class EventsScreen : Page
    {

        private List<model.Event> _upcomingEventsList;
        private List<model.Event> _createdEventsList;
        private model.Student _loggedInUser;

        public EventsScreen()
        {
            InitializeComponent();

            /*
            // Use this for debugging purposes only.
            this._createdEventsList = model.DomainModels.EventModel.getByOwner("U096988R");
            this._upcomingEventsList = model.DomainModels.EventModel.getAll();

            this.upcomingEventsListGrid.ItemsSource = this._upcomingEventsList;
            this.createdEventsListGrid.ItemsSource = this._upcomingEventsList;
            */
        }

        public void SetupNavigationHandler(NavigationService ns)
        {
            ns.LoadCompleted += new LoadCompletedEventHandler(NavigationService_LoadCompleted);
        }

        private void createEventBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationData navData = new NavigationData();
            navData.loggedInUser = _loggedInUser;

            EditEventScreen editEventScreen = new EditEventScreen();
            editEventScreen.SetupNavigationHandler(this.NavigationService);
            this.NavigationService.Navigate(editEventScreen, navData);
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out now?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new Uri("LoginScreen.xaml", UriKind.Relative));
            } 
        }

        private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            this.NavigationService.LoadCompleted -= NavigationService_LoadCompleted;

            NavigationData navData = (NavigationData)e.ExtraData;
            if (navData == null || navData.loggedInUser == null)
            {
                this.NavigationService.Navigate(new Uri("LoginScreen.xaml", UriKind.Relative));
            }
            else
            {
                this._loggedInUser = navData.loggedInUser;
                this.loggedInUserLabel.Content = this._loggedInUser.Name;
                this.updateScreen();
            }

            if (navData.statusCode != NavigationData.STATUS_NONE)
            {
                this.statusMessage.Content = navData.statusMessage;
                Storyboard sb = (Storyboard)this.FindResource("StatusMessageFadeIn");
                this.statusMessage.BeginStoryboard(sb);
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            // Get event ID that user has requested to edit.
            Button b = (Button)e.Source;

            // Redirect user to EditEventScreen to edit their event.
            NavigationData navData = new NavigationData();
            navData.loggedInUser = _loggedInUser;
            navData.eventID = (int)b.Tag;

            EditEventScreen editEventScreen = new EditEventScreen();
            editEventScreen.SetupNavigationHandler(this.NavigationService);
            this.NavigationService.Navigate(editEventScreen, navData);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {        
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this event? THIS CANNOT BE UNDONE!", "Delete Event", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {   
                // Get event ID that user has requested to delete.
                Button b = (Button)e.Source;
                model.DomainModels.EventModel.deleteById(Convert.ToInt32(b.Tag));
                this.updateScreen();
            } 
        }

        protected void updateScreen()
        {
            // Load events based on logged in user (created events for now, registered events in the future).
            this._createdEventsList = model.DomainModels.EventModel.getByOwner(_loggedInUser.MatricId);
            this._upcomingEventsList = model.DomainModels.EventModel.getNotByOwner(_loggedInUser.MatricId);

            this.upcomingEventsListGrid.ItemsSource = this._upcomingEventsList;
            this.createdEventsListGrid.ItemsSource = this._createdEventsList;
        }
    }
}
