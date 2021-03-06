﻿using System;
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
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Page
    {
        private List<model.Event> _upcomingEventsList;
        public Dictionary<string, DateTime> _monthFiltersDict { get; set; }

        public LoginScreen()
        {
            this.InitializeComponent();
            this._createMonthFilters();
            this._getUpcomingEvents(DateTime.Now);
        }

        private void UpcomingEventsFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string filter = this.upcomingEventsFilter.SelectedValue.ToString();
            if (!filter.Equals("View By Month"))
            {
                DateTime filterMonth = DateTime.Parse(filter);
                this._getUpcomingEvents(filterMonth);
            }
            else
            {
                this._getUpcomingEvents(DateTime.Now);
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

                // Create NavigationData object to store values and transport them between pages.
                NavigationData navData = new NavigationData();
                navData.loggedInUser = student;

                EventsScreen eventsScreen = new EventsScreen();
                eventsScreen.SetupNavigationHandler(this.NavigationService);
                this.NavigationService.Navigate(eventsScreen, navData);
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
            var dateCounter = DateTime.Now;
            this._monthFiltersDict = new Dictionary<string,DateTime>();

            // Add default option.
            this._monthFiltersDict.Add(dateCounter.ToString("y"), dateCounter);
            
            for (int i = 0; i < 12; i++)
            {
                dateCounter = dateCounter.AddMonths(1);
                this._monthFiltersDict.Add(dateCounter.ToString("y"), dateCounter);
            }
        }

        private void _getUpcomingEvents(DateTime dateFilter)
        {
            this._upcomingEventsList = model.DomainModels.EventModel.getAllByYearMonth(dateFilter);
            this.upcomingEventsListBox.ItemsSource = this._upcomingEventsList;

            if (this._upcomingEventsList.Count > 0)
            {
                this.noUpcomingEventsNotice.Visibility = Visibility.Collapsed;
                this.upcomingEventsScrollViewer.Visibility = Visibility.Visible;

                Storyboard sb = (Storyboard)this.FindResource("ContentFadeIn");
                this.upcomingEventsScrollViewer.BeginStoryboard(sb);
            }
            else
            {
                this.noUpcomingEventsNotice.Visibility = Visibility.Visible;
                this.upcomingEventsScrollViewer.Visibility = Visibility.Collapsed;

                Storyboard sb = (Storyboard)this.FindResource("ContentFadeIn");
                this.noUpcomingEventsNotice.BeginStoryboard(sb);
            }  
        }

        private bool _validateUser(string matricId, string password)
        {
            return model.DomainModels.StudentModel.authenticate(matricId, password);
        }

    }
}
