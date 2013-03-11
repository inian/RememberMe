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

namespace F9S1.RememberMe
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MainWindow mainForm;
        Controller dispatchSmall;
        public Window1(MainWindow MainForm)
        {
            mainForm = MainForm;
            dispatchSmall = new Controller();
            InitializeComponent();
            inputBox.Focus();
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            string input = inputBox.Text;
            input = input.Trim();
            if (e.Key == Key.Enter && input != "")
            {
                if (input == "close")
                {
                    inputBox.Text = "";
                    this.Hide();
                    mainForm.Show();
                   
                    return;
                   
                }
                dispatchSmall.UserDispatch(input);
                inputBox.Text = "";
            }
        }
    }
   
}
