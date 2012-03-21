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
using System.Collections.ObjectModel;

namespace EventManagerApp
{
    /// <summary>
    /// Interaction logic for EventsScreen.xaml
    /// </summary>
    public partial class EventsScreen : Page
    {
        private ObservableCollection<EventItem> _upcomingEventsList;
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
                this._loggedInUser = this._loggedInUser = model.DomainModels.StudentModel.getByMatricId(navData.loggedInUser.MatricId);
                this.loggedInUserLabel.Content = this._loggedInUser.Name;
                this.updateScreen();
            }

            if (navData.statusCode != NavigationData.STATUS_NONE)
            {
                if (navData.statusCode == NavigationData.STATUS_NOTICE)
                {
                    this.statusMessage.Style = (Style)this.FindResource("StatusMessageNoticeStyle");
                }
                else if (navData.statusCode == NavigationData.STATUS_ERROR)
                {
                    this.statusMessage.Style = (Style)this.FindResource("StatusMessageErrorStyle");
                }

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

                // Display delete notice.
                this.statusMessage.Content = "Your event has been successfully deleted.";
                this.statusMessage.Style = (Style)this.FindResource("StatusMessageErrorStyle");
                Storyboard sb = (Storyboard)this.FindResource("StatusMessageFadeIn");
                this.statusMessage.BeginStoryboard(sb);
            } 
        }

        protected void updateScreen()
        {
            // Reload user data from database, since the list of registered events have changed.
            this._loggedInUser = model.DomainModels.StudentModel.getByMatricId(this._loggedInUser.MatricId);

            List<model.Event> upcomingEvents = model.DomainModels.EventModel.getAll();
            this._upcomingEventsList = new ObservableCollection<EventItem>();
            foreach ( model.Event e in upcomingEvents )
            {
                bool isRegistered = false;

                foreach (model.Event r in this._loggedInUser.RegisteredEvents)
                {
                    if (r.Id == e.Id) isRegistered = true;
                    Console.WriteLine(isRegistered);
                }  
                this._upcomingEventsList.Add(new EventItem(e, (e.Owner.MatricId == this._loggedInUser.MatricId), isRegistered));
            }
            this.upcomingEventsListGrid.ItemsSource = this._upcomingEventsList;

            // Load events based on logged in user (created events for now, registered events in the future).
            this._createdEventsList = model.DomainModels.EventModel.getByOwner(_loggedInUser.MatricId);
            this.createdEventsListGrid.ItemsSource = this._createdEventsList;

            this.registeredEventsListGrid.ItemsSource = this._loggedInUser.RegisteredEvents;
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)e.Source;
            model.DomainModels.EventModel.registerGuest(this._loggedInUser.MatricId, Convert.ToInt32(b.Tag));

            this.updateScreen();
        }

        private void unregisterButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)e.Source;
            
            // Insert unregister guest DB call here.

            this.updateScreen();
        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            // Does not do anything at the moment.
        }
    }
}
