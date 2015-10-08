using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrackForegroundWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ActiveWindowTimeTracker _timeTracker;

        public MainWindow()
        {
            InitializeComponent();

            _timeTracker = new ActiveWindowTimeTracker();
            _timeTracker.StartTracking();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            label.Content = _timeTracker.ToString();
        }
    }
}
