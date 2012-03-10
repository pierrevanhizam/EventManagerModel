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

            DateTime day;
            Nullable<DateTime> selected_date = eventDatePicker.SelectedDate;
            if(selected_date.HasValue) 
            {
                day = selected_date.Value;
            } else {
                day = DateTime.Now;
            }
            var selected_start_time = eventStartTimeBox.SelectedValue.ToString();
            var selected_end_time = (string) eventEndTimeBox.SelectedValue.getValue();
            string start_time = day.ToShortDateString() + ':' + selected_start_time;
            string end_time = day.ToShortDateString() + ':' + selected_end_time;
            Console.WriteLine(start_time);
            Console.WriteLine(end_time);
            //this.start = DateTime.Parse(start_time);
            //this.end = DateTime.Parse(end_time);
            this.start = DateTime.Now;
            this.end = DateTime.Now;

            this.visibleOnLoginPage = 1;
            // TODO: change dummy matric and name, visibleOnLoginPage
            model.DomainModels.EventModel.create("U096988R", "Felix", 1, this.start, this.end, this.capacity, this.budget, this.description, this.visibleOnLoginPage);
            eventNameBox.Text = "SAVED";
        }

    }
}
