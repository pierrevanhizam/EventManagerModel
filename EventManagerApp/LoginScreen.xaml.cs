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
        public Dictionary<string,int> _monthFiltersDict { get; set;}

        public LoginScreen()
        {
            this.InitializeComponent();
            this._createMonthFilters();
            this._getUpcomingEvents(DateTime.Now.Month);
        }

        private void UpcomingEventsFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string filter = this.upcomingEventsFilter.SelectedValue.ToString();
            if (!filter.Equals("View By Month"))
            {
                DateTime filterMonth = DateTime.Parse(filter);
                this._getUpcomingEvents(filterMonth.Month);
            }
            else
            {
                this._getUpcomingEvents(DateTime.Now.Month);
            }
        }

        private void PrevButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.upcomingEventsFilter.SelectedIndex != 0)
            {
                this.upcomingEventsFilter.SelectedIndex--;
            }
        }

        private void NextButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.upcomingEventsFilter.Items.Count != this.upcomingEventsFilter.SelectedIndex)
            {
                this.upcomingEventsFilter.SelectedIndex++;
            }        
        }

        private void LoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string matricId = usernameText.Text;
            string password = passwordText.Password;
            if (this._validateUser(matricId, password))
            {
                Console.WriteLine("authentication successful");
                model.Student student = model.DomainModels.StudentModel.getByMatricId(matricId);
                this.NavigationService.Navigate(new Uri("EventsScreen.xaml", UriKind.Relative));
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

        private void _createMonthFilters()
        {
            var month = DateTime.Now;
            this._monthFiltersDict = new Dictionary<string,int>();
            this._monthFiltersDict.Add("View By Month",0);
            for(int i=1;i<=12;i++)
            {
                month = month.AddMonths(1);
                this._monthFiltersDict.Add(month.ToString("y"),i);
            }

        }

        private void _getUpcomingEvents(int month)
        {
            this._upcomingEventsList = model.DomainModels.EventModel.getAllByMonth(month);
            this.upcomingEventsListBox.ItemsSource = this._upcomingEventsList;
        }

        private bool _validateUser(string matricId, string password)
        {
            return model.DomainModels.StudentModel.authenticate(matricId, password);
        }

    }
}
