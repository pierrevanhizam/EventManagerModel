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
        public int budget;
        public int capacity;
        public string description;
        public string eventName;
        public short visibleOnLoginPage;

        public EditEventScreen()
        {
            this.venues = model.DomainModels.VenueModel.getAll();
            InitializeComponent();
            this.DataContext = this;
            // Set the title according to current action (Edit or Add).
            this.Title = "Create Event";
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

            // TODO: change dummy matric and name
            //model.DomainModels.EventModel.create("U096988R", "Felix", 1, this.start, this.end, this.capacity, this.budget, this.description, this.visibleOnLoginPage);
            
            // TODO: Redirect back to Event Main screen with notify box that it has been saved.
            eventNameBox.Text = "SAVED";
        }

    }
}
