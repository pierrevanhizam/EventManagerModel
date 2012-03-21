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
    /// Interaction logic for EditEventScreen.xaml
    /// </summary>
    public partial class EditEventScreen : Page
    {
        // Private fields.
        private model.Event _curEvent;
        private model.Student _loggedInUser;

        // Fields for use in XAML data binding.
        public List<model.Venue> venues { get; set; }

        public model.Event CurEvent
        {
            get { return this._curEvent; }
        }

        public Boolean VisibleOnLoginPage
        {
            get { return (this._curEvent.ViewAtLoginPage == 1); }
            set { this._curEvent.ViewAtLoginPage = 0; }
        }

        // Constructor
        public EditEventScreen()
        {
            this.venues = model.DomainModels.VenueModel.getAll();
            InitializeComponent();
            this.DataContext = this;

            // Set the title according to current action (Edit or Add).
            this.Title = "Create Event";
            this._curEvent = new model.Event();

            // Set the ID to -1 since it's a new Event.
            this._curEvent.Id = -1;
        }

        public void SetupNavigationHandler(NavigationService ns)
        {
            ns.LoadCompleted += new LoadCompletedEventHandler(NavigationService_LoadCompleted);
        }

        private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            this.NavigationService.LoadCompleted -= NavigationService_LoadCompleted;

            NavigationData navData = (NavigationData)e.ExtraData;
            if (navData != null)
            {
                if (navData.loggedInUser == null)
                {
                    this.NavigationService.Navigate(new Uri("LoginScreen.xaml", UriKind.Relative));
                }
                else
                {
                    this._loggedInUser = navData.loggedInUser;
                    this.loggedInUserLabel.Content = this._loggedInUser.Name;
                }

                // If eventID from navData exists, it means that we need to edit the event!
                if (navData.eventID != -1)
                {
                    this.Title = "Edit Event";

                    // Load event data from database and its data will bind onto the form.
                    this._curEvent = model.DomainModels.EventModel.getByID(navData.eventID);
                }
            }
            
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._curEvent.Id != -1)
            {
                // Update entry to database.
                // Melvin: Can't we just have a method from the model to take in the whole Event object instead?
                model.DomainModels.EventModel.update(
                    this._curEvent.Id,
                    this._loggedInUser.MatricId,
                    this._curEvent.Name,
                    this._curEvent.VenueId,
                    this._curEvent.Start,
                    this._curEvent.End,
                    this._curEvent.Capacity,
                    Convert.ToInt32(this._curEvent.Budget),
                    this._curEvent.Description,
                    this._curEvent.ViewAtLoginPage
               );
            }
            else
            {
                // Create new event entry to database.
                // Melvin: Can't we just have a method from the model to take in the whole Event object instead?
                model.DomainModels.EventModel.create(
                    this._loggedInUser.MatricId,
                    this._curEvent.Name,
                    this._curEvent.VenueId,
                    this._curEvent.Start,
                    this._curEvent.End,
                    this._curEvent.Capacity,
                    Convert.ToInt32(this._curEvent.Budget),
                    this._curEvent.Description,
                    this._curEvent.ViewAtLoginPage
               );
            }
            
            
            // Redirect back to Event Main screen with notify box showing that it has been saved.
            NavigationData navData = new NavigationData();
            navData.loggedInUser = this._loggedInUser;
            navData.statusCode = NavigationData.STATUS_NOTICE;

            if (this._curEvent.Id != -1)
            {
                navData.statusMessage = "Your event has been successfully edited.";
            }
            else
            {
                navData.statusMessage = "Your event has been successfully created.";
            }

            EventsScreen eventsScreen = new EventsScreen();
            eventsScreen.SetupNavigationHandler(this.NavigationService);
            this.NavigationService.Navigate(eventsScreen, navData);
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out now?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new Uri("LoginScreen.xaml", UriKind.Relative));
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you wish to cancel? Any unsaved changes will be lost!", "Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Redirect back to Event Main screen with notify box showing that it is cancelled.
                NavigationData navData = new NavigationData();
                navData.loggedInUser = this._loggedInUser;

                EventsScreen eventsScreen = new EventsScreen();
                eventsScreen.SetupNavigationHandler(this.NavigationService);
                this.NavigationService.Navigate(eventsScreen, navData);
            }
        }

        private void budgetNewSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void budgetNewCancel_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
