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

        public List<model.Venue> venues { get; set; }
        public DateTime start;
        public DateTime end;
        public int ID = -1;
        public int budget;
        public int capacity;
        public string description;
        public string eventName;
        public short visibleOnLoginPage;
        private model.Student _loggedInUser;

        public EditEventScreen()
        {
            this.venues = model.DomainModels.VenueModel.getAll();
            InitializeComponent();
            this.DataContext = this;
            // Set the title according to current action (Edit or Add).
            this.Title = "Create Event";
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

                    // Load event data from database and assign it to their values accordingly.
                    model.Event curEvent = model.DomainModels.EventModel.getByID(navData.eventID);
 
                    this.ID = curEvent.Id;
                    this.eventNameBox.Text = curEvent.Name;
                    this.eventDescText.Text = curEvent.Description;
                    this.eventBudgetBox.Text = curEvent.Budget.ToString();
                    this.eventCapacityBox.Text = curEvent.Capacity.ToString();
                    this.eventDatePicker.SelectedDate = curEvent.Start;

                    this.eventStartTimeBox.SelectedValue = new DateTime().AddHours(curEvent.Start.Hour);
                    this.eventEndTimeBox.SelectedValue = new DateTime().AddHours(curEvent.End.Hour);

                    this.eventVisibleCheckbox.IsChecked = (curEvent.ViewAtLoginPage == 1);
                }
            }
            
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            this.eventName = eventNameBox.Text;
            this.description = eventDescText.Text;
            this.budget = Convert.ToInt32(eventBudgetBox.Text);
            this.capacity = Convert.ToInt32(eventCapacityBox.Text);

            // get selected date from DatePicker (default is NOW())
            DateTime selected_date = (DateTime)eventDatePicker.SelectedDate;

            // merge date and time into DateTime
            DateTime startHourDateTime = (DateTime)eventStartTimeBox.SelectedValue;
            this.start = selected_date.Add(startHourDateTime.Subtract(new DateTime()));
            DateTime endHourDateTime = (DateTime)eventEndTimeBox.SelectedValue;
            this.end = selected_date.Add(endHourDateTime.Subtract(new DateTime()));

            if (eventVisibleCheckbox.IsChecked.HasValue && (bool)(eventVisibleCheckbox.IsChecked))
            {
                this.visibleOnLoginPage = 1;
            }
            else
            {
                this.visibleOnLoginPage = 0;
            }

            if (this.ID != -1)
            {
                // @TODO: Edit existing entry to database.
                model.DomainModels.EventModel.update(this.ID, this._loggedInUser.MatricId, this.eventName, 1, this.start, this.end, this.capacity, this.budget, this.description, this.visibleOnLoginPage);
            }
            else
            {
                // Create new event entry to database.
                model.DomainModels.EventModel.create(this._loggedInUser.MatricId, this.eventName, 1, this.start, this.end, this.capacity, this.budget, this.description, this.visibleOnLoginPage);
            }
            
            
            // Redirect back to Event Main screen with notify box showing that it has been saved.
            NavigationData navData = new NavigationData();
            navData.loggedInUser = this._loggedInUser;
            navData.statusCode = NavigationData.STATUS_NOTICE;

            if (this.ID != -1)
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

    }
}
