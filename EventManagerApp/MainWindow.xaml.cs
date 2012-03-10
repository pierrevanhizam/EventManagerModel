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
using System.Windows.Media.Animation;

namespace EventManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this._mainFrame.Navigating += new NavigatingCancelEventHandler(_mainFrame_Navigating);
            this._mainFrame.Navigated += new NavigatedEventHandler(_mainFrame_Navigated);
            //this._mainFrame.Navigate(new Uri("EditEventScreen.xaml", UriKind.Relative));
            //this._mainFrame.Navigate(new Uri("EventsScreen.xaml", UriKind.Relative));
        }

        void _mainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Storyboard sb = (Storyboard)this.FindResource("PageFadeIn");
            sb.Begin();
        }

        void _mainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            Storyboard sb = (Storyboard)this.FindResource("PageFadeOut");
            sb.Begin();
        }
    }
}
