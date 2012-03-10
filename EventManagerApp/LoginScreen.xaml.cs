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
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Page
    {
        private List<model.Event> _upcomingEventsList;

        public LoginScreen()
        {
            this.InitializeComponent();
            this.GetUpcomingEvents();
            this.upcomingEventsListBox.ItemsSource = this._upcomingEventsList;
        }

        private void GetUpcomingEvents()
        {
            this._upcomingEventsList = model.DomainModels.EventModel.getAllByMonth(3);
        }

        // Method to validate user login information.
        private bool ValidateUser(string matricId, string password)
        {
            return model.DomainModels.StudentModel.authenticate(matricId, password);
        }

        // Event handler for getting more events from DB.
        private void UpcomingEventsFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Console.WriteLine(upcomingEventsFilter.SelectedItem.ToString());
        }

        // Event handler for 'Previous' button.
        private void PrevButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Get events from the previous month.
        }

        // Event handler for 'Next' button.
        private void NextButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Get events from the next month.
        }

        // Event handler when user clicks on 'Login' button.
        private void LoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string matricId = usernameText.Text;
            string password = passwordText.Password;
            if (this.ValidateUser(matricId, password))
            {
                Console.WriteLine("authentication successful");

                // Navigate to EventsScreen.xaml with user ID for authentication.
                // int userID = ?;
                // this.NavigationService.Navigate(new Uri("EventsScreen.xaml"), UriKind.Relative, userID);
            }
            else
            {
                MessageBox.Show(
                    "Your matriculation number and password were not recognized. Please check and try again.",
                    "Authentication Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation
                );
                Console.WriteLine("authentication failed");
            }
        }

    }
}
