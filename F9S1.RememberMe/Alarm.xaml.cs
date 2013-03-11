//Inian Parameshwaran

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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
namespace F9S1.RememberMe
{
    /// <summary>
    /// Interaction logic for Alarm.xaml
    /// </summary>
    public partial class Alarm : Window
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        String taskName;
        int[] time;
        public delegate void timeCheck();
        public Alarm(String name, DateTime deadline)
        {
            Debug.Assert(name != null);
            Debug.Assert(deadline != null);
            InitializeComponent();
            days.Text = "0";
            hours.Text = "0";
            minutes.Text = "5";
            taskName = name;
            label1.Content = taskName + " : ";
            label1.Content += deadline.ToString(Utility.SHORT_DATE_FORMAT);
        }
        public void setTime()
        {
            time = new int[3];
            if (days.Text == "")
            {
                time[0] = 0;
            }
            else
            {
                time[0] = int.Parse(days.Text);
            }
            if (hours.Text == "")
            {
                time[1] = 0;
            }
            else
            {
                time[1] = int.Parse(hours.Text);
            }
            if (minutes.Text == "" || (minutes.Text == "0" && time[1] == 0 && time[2] == 0))
            {
                time[2] = 5;
            }
            else
            {
                time[2] = int.Parse(minutes.Text);
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            setTime();
            this.Close();
        }
        public int[] getTimeArray()
        {
            return time;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            Keyboard.Focus(archive_button);
        }

        private void days_GotFocus(object sender, RoutedEventArgs e)
        {
            days.Select(0, 1);
        }

        private void hours_GotFocus(object sender, RoutedEventArgs e)
        {
            hours.Select(0, 1);
        }

        private void minutes_GotFocus(object sender, RoutedEventArgs e)
        {
            minutes.Select(0, 1);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void days_TextChanged(object sender, TextChangedEventArgs e)
        {
            int dayEntered = 0;
            try
            {
                dayEntered = int.Parse(days.Text);
            }
            catch (Exception a)
            {

                logger.Warn("Incorrect days format");
                days.Text = "0";
                days.Select(0, 1);
            }
            if (dayEntered > 31)
            {
                days.Text = "0";
                days.Select(0, 1);
            }

        }

        private void hours_TextChanged(object sender, TextChangedEventArgs e)
        {
            int hoursEntered = 0;
            try
            {
                hoursEntered = int.Parse(hours.Text);
            }
            catch (Exception a)
            {

                logger.Warn("Incorrect hours format");
                hours.Text = "0";
                hours.Select(0, 1);
            }
            if (hoursEntered > 23)
            {
                hours.Text = "0";
                hours.Select(0, 1);
            }

        }

        private void minutes_TextChanged(object sender, TextChangedEventArgs e)
        {
            int minuteEntered = 0;
            try
            {
                minuteEntered = int.Parse(minutes.Text);
            }
            catch (Exception a)
            {
                logger.Warn("Incorrect minutes format");  
                minutes.Text = "0";
                minutes.Select(0, 1);
            }
            if (minuteEntered > 59)
            {
                minutes.Text = "0";
                minutes.Select(0, 1);
            }
        }
    }
}
