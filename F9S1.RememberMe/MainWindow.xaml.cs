//Inian Parameshwaran
//Akshay Viswanathan

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
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace F9S1.RememberMe
{
    /// <summary>
    /// 
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private System.Windows.Forms.NotifyIcon m_notifyIcon;

        Controller dispatch;
        List<Task> taskInfo;
        List<string> taskDetails;
        string lastInput;
        const string FIND_COMMAND = "find";
        const string ADD_COMMAND = "add";
        const string EDIT_COMMAND = "edit";
        const string QUIT_COMMAND = "quit";
        const string DELETE_COMMAND = "delete";
        const string ARCHIVE_COMMAND = "archive";
        const string CLEAR_COMMAND = "clear";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        int numberBackSpace = 0;
        string[] userPrompts = { "", "details", "deadline", "label", "priority" };
        public MainWindow()
        {
            Tester fileTest = new Tester();
            //fileTest.Test();
            initialiseNotificationIcon();
            dispatch = new Controller(this);
            taskInfo = dispatch.GetTasks();
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
                //dispatch.Log(e.StackTrace);
            }
            inputBox.Focus();
            helpBox.Visibility = System.Windows.Visibility.Collapsed;
            SetDisplay();
        }
        void initialiseNotificationIcon()
        {
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "Remember Me has been minimised. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "Remember Me";
            m_notifyIcon.Text = "Remember Me";
            m_notifyIcon.Icon = new System.Drawing.Icon("AddedIcon.ico");
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);
        }
        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;
            dispatch.UserDispatch(QUIT_COMMAND);
        }

        private WindowState m_storedWindowState = WindowState.Normal;
        private void OnStateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (m_notifyIcon != null)
                    m_notifyIcon.ShowBalloonTip(2000);
            }
            else
                m_storedWindowState = WindowState;
        }
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        void m_notifyIcon_Click(object sender, System.EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }
        void maximiseWindow()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Show();
            }
        }
        private void displayHelp()
        {
            doneButton.Visibility = System.Windows.Visibility.Visible;
            dataGrid1.Visibility = System.Windows.Visibility.Collapsed;
            helpBox.Visibility = System.Windows.Visibility.Visible;
            inputBox.Visibility = System.Windows.Visibility.Collapsed;
            displayBox.Visibility = System.Windows.Visibility.Collapsed;
            inputBox.Text = "";
            helpBox.Focus();
            helpBox.Text = Utility.HELP;
        }
        private int numberOfSemiColon(String text)
        {
            // Debug.Assert(text == null); 
            int count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ';')
                {
                    count++;
                }
            }
            return count;
        }
        private void AutoComplete(String input)
        {
            //Debug.Assert(input==null);
            if (input.Equals(""))
                return;
            else if (input.Length < ARCHIVE_COMMAND.Length && input.StartsWith("ar") && input.Equals(ARCHIVE_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, ARCHIVE_COMMAND);
            else if (input.Length < ADD_COMMAND.Length && input.StartsWith("a") && input.Equals(ADD_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, ADD_COMMAND);
            else if (input.Length < EDIT_COMMAND.Length && input.StartsWith("e") && input.Equals(EDIT_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, EDIT_COMMAND);
            else if (input.Length < QUIT_COMMAND.Length && input.StartsWith("q") && input.Equals(QUIT_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, QUIT_COMMAND);
            else if (input.Length < DELETE_COMMAND.Length && input.StartsWith("d") && input.Equals(DELETE_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, DELETE_COMMAND);
            else if (input.Length < CLEAR_COMMAND.Length && input.StartsWith("c") && input.Equals(CLEAR_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, CLEAR_COMMAND);
            else if (input.Length < FIND_COMMAND.Length && input.StartsWith("f") && input.Equals(FIND_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, FIND_COMMAND);
        }
        private void AutoComplete(String input, String keyWord)
        {
            int start = inputBox.Text.Length;
            inputBox.Text += keyWord.Substring(input.Length, keyWord.Length - input.Length);
            inputBox.Select(start, keyWord.Length - input.Length);
        }

        /*  private void sortAutoComplete(String input)
          {
              if (input.Length < DEADLINE.Length && input.StartsWith("d") && input.Equals(DEADLINE.Substring(0, input.Length)))
                  AutoComplete(input, DEADLINE);
              else if (input.Length < SORT_PRIORITY.Length && input.StartsWith("p") && input.Equals(SORT_PRIORITY.Substring(0, input.Length)))
                  AutoComplete(input, SORT_PRIORITY);
          }*/
        private int findNumHits(Task check, string keyword)
        {
            int hitcount = 0;
            if (check.Details.Contains(keyword))
            {
                hitcount++;
            }
            if (check.Labels.Contains(keyword))
            {
                hitcount++;
            }
            if (check.Deadline.ToString("f").ToLower().Contains(keyword.ToLower()))
            {
                hitcount++;
            }

            
            return hitcount;
        }


        int findHits(string keywords, Task findHits, string command)
        {
            int hitcount = 0;
            if (findHits.ToString().Contains(keywords) && findHits.IsArchived == false)
            {
                hitcount = findNumHits(findHits, keywords);
            }
            if ("archive".Contains(keywords) && findHits.IsArchived == true && command == FIND_COMMAND
                || findHits.ToString().Contains(keywords) && findHits.IsArchived == true && (command == FIND_COMMAND || command == DELETE_COMMAND))
            {
                hitcount++;
            }
            if ("highstar".Contains(keywords) && findHits.IsStarred == true && findHits.IsArchived == false)
            {
                hitcount++;
            }
            if ("highstar".Contains(keywords) && findHits.IsStarred == true && findHits.IsArchived == true && command == FIND_COMMAND)
            {
                hitcount++;

            }
            return hitcount;


        }
        public List<string> InstantSearch(string input, string command)
        {
            List<Task> taskList = taskInfo;
            List<string> keywords = new List<string>(input.Split(' '));
            for (int i = 0; i < keywords.Count; i++)
                if (keywords[i].Equals(""))
                    keywords.RemoveAt(i);
            List<int> hitcount = new List<int>();
            for (int i = 0; i < taskList.Count; i++)
                hitcount.Add(0);
            int maxhits = 0;
            for (int i = 0; i < taskList.Count; i++)
            {
                for (int j = 0; j < keywords.Count; j++)
                {
                    hitcount[i] += findHits(keywords[j], taskList[i], command);
                    if (hitcount[i] > maxhits)
                        maxhits = hitcount[i];
                }
            }
            List<string> temp = new List<string>();
            for (int i = maxhits; i > 0; i--)
            {
                for (int j = 0; j < taskList.Count; j++)
                    if (hitcount[j] == i)
                        temp.Add(taskList[j].ToString());
            }

            return temp;
        }
        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string passToRelegator = inputBox.Text;

            if (Keyboard.IsKeyDown(Key.Back))
                numberBackSpace = 1;
            /* if (numberBackSpace == 0 && inputBox.Text.StartsWith(SORT_COMMAND + ';') && !inputBox.Text.EndsWith(";"))
                 sortAutoComplete(inputBox.Text.Substring(inputBox.Text.IndexOf(';') + 1));*/
            numberBackSpace = 0;
            if (Keyboard.IsKeyDown(Key.Back))
                numberBackSpace = 1;
            if (numberBackSpace == 0 && !inputBox.Text.Contains(' ') && !inputBox.Text.Contains(";"))
                AutoComplete(inputBox.Text);
            numberBackSpace = 0;

            string getCommand = "";
            if (inputBox.Text.Contains(";"))
            {

                getCommand = inputBox.Text.Substring(0, inputBox.Text.IndexOf(';')).ToLower().Trim();
                int count = numberOfSemiColon(inputBox.Text);
                if (inputBox.Text[inputBox.Text.Length - 1] == ';' && (getCommand.Equals("add") || getCommand.Equals("edit")) && numberBackSpace == 0)
                {
                    if (Keyboard.IsKeyDown(Key.Back))
                        numberBackSpace = 1;
                    if (numberBackSpace == 0)
                    {

                        int countSemiColon = numberOfSemiColon(inputBox.Text);

                        if (countSemiColon < 5)  //total number of ';'s for add/edit
                        {
                            if (getCommand.Equals("add"))
                                inputBox.Text += userPrompts[countSemiColon];
                            else if (getCommand.Equals("edit"))
                                if (countSemiColon != 1)
                                    inputBox.Text += userPrompts[countSemiColon];
                            int semicolonIndex = inputBox.Text.LastIndexOf(';');
                            inputBox.Select(semicolonIndex + 1, inputBox.Text.Length - semicolonIndex);
                        }
                    }
                }

            }
            numberBackSpace = 0;
            if (getCommand == DELETE_COMMAND || getCommand == EDIT_COMMAND || getCommand == ARCHIVE_COMMAND || getCommand == FIND_COMMAND)
            {
                int posOfSemi = inputBox.Text.LastIndexOf(";");
                string wordToSearch = inputBox.Text.Substring(posOfSemi + 1);
                if (posOfSemi != -1)
                {
                    List<string> toBeDisplay = InstantSearch(wordToSearch, getCommand);
                    SetOutputBox(toBeDisplay);
                }
            }

            if (inputBox.Text.Length <= getCommand.Length + 1)
                SetDisplay();
        }
        public void SetDisplay()
        {
            taskDetails = dispatch.UserDispatch("display");
            SetOutputBox(taskDetails);
        }
        private void setErrorLabel(string errorMessage)
        {
            displayBox.Content = errorMessage;
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            displayBox.Content = "";
            string input = inputBox.Text.ToString();
            if (e.Key == Key.Enter)
            {
                if (inputBox.Text.Trim() == "")
                {
                    SetDisplay();
                }

                else if (input.Trim().ToLower() == "help")
                {
                    inputBox.Text = "";
                    displayHelp();
                    return;
                }
                else if (input.Trim().ToLower() == "sync")
                {
                    inputBox.Text = "";
                    DisplaySync();
                    return;
                }
                else
                {

                    lastInput = input;
                    List<string> output = new List<string>(dispatch.UserDispatch(input));
                    if (output.Count > 0 && output[0] == Utility.ERROR)
                    {
                        inputBox.Text = input;
                        displayBox.Content = input + "\n" + output[1];
                    }

                    else
                    {
                        inputBox.Text = "";
                        displayBox.Content = "";
                    }
                    SetOutputBox(dispatch.UserDispatch("display"));
                }
                taskInfo = dispatch.GetTasks();
            }
            else if (e.Key == Key.Escape)
            {
                inputBox.Clear();
                displayBox.Content = "";
            }
            if (inputBox.Text != "" && e.Key == Key.Tab)
            {
                string words = inputBox.Text.Substring(inputBox.Text.LastIndexOf(';') + 1).ToLower();
                string temp = inputBox.Text.Substring(0, inputBox.Text.LastIndexOf(';') + 1);
                if (temp == "delete;" || temp == "edit;" || temp == "archive;" || temp == "find;")
                {
                    string getCommand = temp.Remove(temp.Length - 1);
                    List<string> toBeDisplay = InstantSearch(words, getCommand);
                    if (toBeDisplay.Count != 0)
                    {
                        inputBox.Text = temp + autoCompleteSearch(words, getCommand);
                        inputBox.Select(inputBox.Text.Length, 0);
                    }
                    else
                        inputBox.Text = "";

                }
                /*  inputBox.Focus();
                   inputBox.SelectionStart = inputBox.Text.Length;*/
                e.Handled = true;
            }
            if (e.Key == Key.Escape)
            {
                if (displayBox.Content.ToString() == "edit: ")
                    dispatch.UserDispatch("");
                inputBox.Text = "";
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void undoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            dispatch.UserDispatch("undo");
            inputBox.Text = "";
            SetDisplay();
            displayBox.Content = "Action undone";
        }

        private void redoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            dispatch.UserDispatch("redo");
            inputBox.Text = "";
            SetDisplay();
            displayBox.Content = "Action redone";
        }
        public string autoCompleteSearch(string input, string command)
        {
            List<Task> contents = taskInfo;
            List<string> keywords = new List<string>(input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < keywords.Count; i++)
                if (keywords[i].Equals(""))
                    keywords.RemoveAt(i);
            List<int> hitcount = new List<int>();
            for (int i = 0; i < contents.Count; i++)
                hitcount.Add(0);
            int maxhits = 0;
            string temp = null;
            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < keywords.Count; j++)
                {
                    hitcount[i] += findHits(keywords[j], contents[i], command);
                    if (hitcount[i] > maxhits)
                    {
                        maxhits = hitcount[i];
                        temp = contents[i].Details;

                    }
                }
            }
            return temp;
        }


        private void inputBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                inputBox.Text = lastInput;
                inputBox.SelectionStart = inputBox.Text.Length;
            }
            else if (e.Key == Key.Down)
                displayBox.Focus();
        }

        private void SetOutputBox(List<string> output)
        {
            /*outputBox.Items.Clear();
            for (int i = 0; i < output.Count; i++)
                outputBox.Items.Add(output[i]);
        */
            if (output.Count > 0 && output[0] == Utility.ERROR)
            {
                displayBox.Content = output[1];
            }
            else
            {
                List<Task> displayList = new List<Task>();
                foreach (string item in output)
                {
                    displayList.Add(new Task(item));
                }

                dataGrid1.DataContext = displayList;
                dataGrid1.Items.Refresh();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inputBox.Focus();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

            //Task temp = ((FrameworkElement)sender).DataContext as Task;
            DataGridRow selectedRow = (DataGridRow)(dataGrid1.ItemContainerGenerator.ContainerFromIndex(dataGrid1.SelectedIndex));
            int positionOfSeperator = selectedRow.Item.ToString().IndexOf(Utility.FILE_SEPARATER);
            String taskName = selectedRow.Item.ToString().Substring(0, positionOfSeperator);
            String command = "delete;" + taskName;
            List<String> output = new List<String>(dispatch.UserDispatch(command));
            SetOutputBox(output);
            taskInfo = dispatch.GetTasks();
        }

        private void Archive_Button_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow selectedRow = (DataGridRow)(dataGrid1.ItemContainerGenerator.ContainerFromIndex(dataGrid1.SelectedIndex));
            int positionOfSeperator = selectedRow.Item.ToString().IndexOf(Utility.FILE_SEPARATER);
            String taskName = selectedRow.Item.ToString().Substring(0, positionOfSeperator);
            String command = "archive;" + taskName;
            List<String> output = new List<String>(dispatch.UserDispatch(command));
            if (output.Count > 0 && output[0] == Utility.ERROR)
            {
                displayBox.Content = output[1];
            }
            else
            {
                SetOutputBox(output);
            }
            taskInfo = dispatch.GetTasks();
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //e.column and e.header datagridtextcolumn and tht dot header
            DataGridColumn currentColumn = e.Column;
            String currentHeader = (String)currentColumn.Header;
            FrameworkElement element;
            String newData;
            Task updatedTask = dispatch.GetTasks().ElementAt(0);//create a temp task
            //  var newData;
            String taskName = (new Task(e.Row.Item.ToString())).Details;
            string command = "";
            if (currentHeader.Equals("Label"))
            {
                element = dataGrid1.Columns[3].GetCellContent(e.Row);
                newData = ((TextBox)element).Text;
                updatedTask = new Task(((Task)(e.Row.Item)).ToString());
                command = "edit " + updatedTask.Details + " #" + newData.Trim();
            }
            if (currentHeader.Equals("Deadline"))
            {
                element = dataGrid1.Columns[2].GetCellContent(e.Row);
                newData = ((TextBox)element).Text;
                updatedTask = new Task(((Task)(e.Row.Item)).ToString());
                command = "edit " + updatedTask.Details + " @" + newData;
            }
            if (updatedTask.IsStarred)
                command += Utility.STARRED;
            List<string> output = dispatch.UserDispatch(command);
            if (output.Count > 0 && output[0] == Utility.ERROR)
            {
                displayBox.Content = output[1];
                output = dispatch.UserDispatch(command);
            }
            SetDisplay();
            dataGrid1.SelectedItem = updatedTask;
            return;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow selectedRow = (DataGridRow)(dataGrid1.ItemContainerGenerator.ContainerFromIndex(dataGrid1.SelectedIndex));
            String details = new Task(selectedRow.Item.ToString()).Details;
            List<Task> updatedList = dispatch.GetTasks();
            String command = selectedRow.Item.ToString();
            Task UpdatedTask = new Task(selectedRow.Item.ToString());
            for (int i = 0; i < updatedList.Count; i++)
            {
                if (updatedList[i].Details == details)
                {
                    updatedList.RemoveAt(i);
                    break;
                }
            }
            ToggleButton button = (ToggleButton)e.OriginalSource;
            if ((bool)button.IsChecked)
            {
                UpdatedTask.IsStarred = true;
            }
            else
            {
                UpdatedTask.IsStarred = false;
            }
            command = "edit " + UpdatedTask.Details;
            if (UpdatedTask.IsStarred)
                command += Utility.STARRED;
            List<string> output = dispatch.UserDispatch(command);
            SetOutputBox(output);
        }

        private void dataGrid1_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            displayBox.Content = "";
        }

        private void syncButton_Click(object sender, RoutedEventArgs e)
        {
            DisplaySync();
        }
        private void DisplaySync()
        {
            if (SyncItemsVisible())
            {
                try
                {
                    if (userBox.Text == "" || passwordBox1.Password == "")
                    {
                        SetSyncItemsCollapsed();
                        return;
                    }
                    dispatch.UserDispatch("sync;" + userBox.Text + ";" + passwordBox1.Password);
                    SetSyncItemsCollapsed();
                    displayBox.Content = "Success";
                    inputBox.Focus();
                }
                catch
                {
                    SetSyncItemsCollapsed();
                    displayBox.Content = "Authentication Error / Internet is too slow";
                    inputBox.Focus();
                }
            }
            else
            {
                SetSyncItemsVisible();
                userBox.Focus();
            }
        }
        void SetSyncItemsVisible()
        {
            helpButton.Visibility = System.Windows.Visibility.Collapsed;
            userLabel.Visibility = System.Windows.Visibility.Visible;
            userBox.Visibility = System.Windows.Visibility.Visible;
            passwordLabel.Visibility = System.Windows.Visibility.Visible;
            passwordBox1.Visibility = System.Windows.Visibility.Visible;
        }
        void SetSyncItemsCollapsed()
        {
            helpButton.Visibility = System.Windows.Visibility.Visible;
            userLabel.Visibility = System.Windows.Visibility.Collapsed;
            userBox.Visibility = System.Windows.Visibility.Collapsed;
            passwordLabel.Visibility = System.Windows.Visibility.Collapsed;
            passwordBox1.Visibility = System.Windows.Visibility.Collapsed;
        }
        bool SyncItemsVisible()
        {
            return (userBox.Visibility == System.Windows.Visibility.Visible ||
                    userLabel.Visibility == System.Windows.Visibility.Visible ||
                    passwordBox1.Visibility == System.Windows.Visibility.Visible ||
                    passwordLabel.Visibility == System.Windows.Visibility.Visible);
        }

        private void inputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            dataGrid1.SelectedIndex = -1;
        }
        private void userBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                passwordBox1.Focus();
        }
        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveHelp();
        }
        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            if (displayBox.Visibility == System.Windows.Visibility.Visible)
                displayHelp();
            else
            {
                RemoveHelp();
            }
        }
        private void RemoveHelp()
        {
            doneButton.Visibility = System.Windows.Visibility.Collapsed;
            helpBox.Visibility = System.Windows.Visibility.Collapsed;
            dataGrid1.Visibility = System.Windows.Visibility.Visible;
            inputBox.Visibility = System.Windows.Visibility.Visible;
            displayBox.Visibility = System.Windows.Visibility.Visible;
            inputBox.Focus();
            SetDisplay();
        }


        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Background = null;
        }
        
    }
}

