//Nalin Ilango
//Akshay Viswanathan

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace F9S1.RememberMe
{
    /// <summary>
    /// It parses the user input based on the type of input, and returns back either the parsed input for further processing, or the error message to be displayed.
    /// </summary>
    class Parser
    {
        enum Command
        {
            add,
            edit,
            delete,
            clear,
            undo,
            redo,
            error,
            search,
            find,
            display,
            archive,
            sync,
            label
        };

        private static NLog.Logger logger;
        List<string> parsedInput, inputLabels, betaParse, labels;
        string taskInterval, taskDetails, taskTime, input, taskLabels, taskImportance;
        Command commandName;

        /// <summary>
        /// Initializes global variables;
        /// </summary>
        public Parser()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
        }
        /// <summary>
        /// The controller calls this function to parse the user input.
        /// Depending on the type of input it calls the corresponding function to parse it.
        /// </summary>
        /// <param name="input">The input to be parsed.</param>
        /// <param name="labels">The list of labels.</param>
        /// <returns>The parsed input.</returns>
        public List<string> InputParse(string userInput, List<string> userLabels)
        {
            Debug.Assert(userInput != null);
            input = userInput;
            Debug.Assert(userLabels != null);
            labels = userLabels;
            commandName = ToCommand(input.Trim().Split(new char[] { ' ', ';' })[0].ToLower());

            if (commandName == Command.add || commandName == Command.edit)
            {
                SymbolParse();
            }
            else if (commandName == Command.label)
            {
                LabelParse(input, labels);
            }
            else
            {
                CommandParse(input);
            }

            Debug.Assert(parsedInput != null);
            return parsedInput;
        }
        /// <summary>
        /// Finds out which commmand the user has given
        /// </summary>
        /// <param name="input">The user input.</param>
        /// <returns>The command to be executed. It is add by default.</returns>
        private Command ToCommand(string input)
        {
            Debug.Assert(input != null);
            switch (input)
            {
                case "edit":
                    return Command.edit;
                case "delete":
                    return Command.delete;
                case "clear":
                    return Command.clear;
                case "undo":
                    return Command.undo;
                case "redo":
                    return Command.redo;
                case "display":
                    return Command.display;
                case "search":
                    return Command.search;
                case "find":
                    return Command.find;
                case "sync":
                    return Command.sync;
                case "label":
                    return Command.label;
                case "archive":
                    return Command.archive;
                default:
                    return Command.add;
            }
        }
        private void SymbolParse()
        {
            Debug.Assert(input != null);
            Debug.Assert(labels != null);

            parsedInput = new List<string>();
            inputLabels = new List<string>();
            betaParse = new List<string>(input.Split(new Char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries));
            inputLabels = new List<string>();
            taskImportance = "";
            taskTime = "";
            taskDetails = "";
            taskInterval = "";
            taskLabels = "";


            if (commandName == Command.add)
            {
                if (!IsAddValid(ref input))
                    return;
            }
            else if (commandName == Command.edit)
            {
                if (!IsEditValid(ref input))
                    return;
            }

            if (input.Contains(';'))
                GetFields();
            else
                ExtractSymbols();

            if (CheckFields())
                SetParsedInput();
        }
        private bool IsAddValid(ref string input)
        {
            if (betaParse[0].Trim().ToLower().Equals("add"))
            {
                string[] temp = input.Split(new Char[] { ' ', ';' }, 2);
                if (temp.Length < 2)
                {
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.INPUT_ERROR);
                    return false;
                }
                else
                    input = temp[1];
            }
            return true;
        }
        private bool IsEditValid(ref string input)
        {
            if (betaParse[0].Trim().ToLower().Equals("edit"))
            {
                string[] temp = input.Split(new Char[] { ' ', ';' }, 2);
                if (temp.Length < 2)
                {
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.INPUT_ERROR);
                    return false;
                }
                else
                    input = temp[1];
            }
            return true;
        }
        private bool GetFields()
        {
            input = AddSemicolons(input);
            FieldParse(input);
            return true;
        }
        private string AddSemicolons(string input)
        {
            Debug.Assert(input != null);
            int count = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ';')
                {
                    count++;
                }
            }
            int numOfSemi = 4;
            for (int i = 0; i < numOfSemi - count; i++)
                input += ";";
            return input;
        }
        private void ExtractSymbols()
        {
            ExtractPriority();
            ExtractLabels();
            ExtractDate();
            }
        private void ExtractLabels()
        {
            if (input.Contains('#'))
            {
                if (!(betaParse.Contains("#")))
                    for (int i = 0; i < betaParse.Count; )
                        if (betaParse[i].Contains('#'))
                        {
                            inputLabels.Add(betaParse[i].Substring(1));
                            betaParse.RemoveAt(i);
                        }
                        else
                            i++;
            }
        }
        private void ExtractDate()
        {
            if (!(input.Contains('@')))
            {
                char[] splitter = new char[] { ' ' };
                taskTime = Utility.DEFAULT_NO_TIME;
                taskInterval = Utility.NO_INTERVAL.ToString();
                taskDetails = input.Split('@', '#')[0].Trim();
                Debug.Assert(taskDetails != null);
            }
            else
            {
                taskDetails = input.Split('@', '#')[0].Trim();
                int _at = input.IndexOf('@');
                int _hash = input.IndexOf('#');
                int length = input.Length;
                taskTime = input.Substring(_at + 1, ((_hash - _at > 0) ? _hash - _at - 1 : length - _at - 1));
                taskTime = taskTime.Trim();
                taskInterval = GetRepeat(taskTime).ToString();
                Debug.Assert(taskInterval != null);
                if (taskTime.Contains('%'))
                    taskTime = taskTime.Replace(taskTime.Substring(taskTime.IndexOf('%')).Split(' ', ';')[0], "");
                Debug.Assert(taskTime != null);
                DateTime deadline = ToDate(taskTime);
                Debug.Assert(deadline != null);
                taskTime = deadline.ToString(Utility.SHORT_DATE_FORMAT);
            }
        }
        private void ExtractPriority()
        {
            taskImportance = input.Contains("**").ToString();
            input = input.Replace("**", "");
        }
        private bool CheckFields()
        {
            return (CheckDetails() && CheckDeadline() && CheckLabels());
        }
        private bool CheckDetails()
        {
            if (taskDetails == null || taskDetails == "")
            {
                logger.Info("No details entered");
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.INPUT_ERROR);
                return false;
            }
            return true;
        }
        private bool CheckDeadline()
        {
            if (taskTime == Utility.DEFAULT_ERROR_DATE.ToString(Utility.SHORT_DATE_FORMAT))
            {
                logger.Info("Incorrect Date format");
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.DATE_ERROR);
                return false;
            }
            DateTime deadline;
            try
            {
                if (taskTime != Utility.DEFAULT_NO_TIME)
                    deadline = DateTime.Parse(taskTime);
                else
                    deadline = Utility.DEFAULT_UNDEFINED_DATE;
            }
            catch
            {
                deadline = Utility.DEFAULT_ERROR_DATE;
            }
            if (deadline < System.DateTime.Now)
            {
                logger.Info("DateTime elapsed");
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.EARLY_DATE_ERROR);
                return false;
            }
            return true;
        }
        private bool CheckLabels()
        {
            bool check;
            if (inputLabels.Count == 0)
                check = CheckLabels(taskLabels);
            else
                check = CheckLabels(inputLabels);
            if (!check)
            {
                logger.Info("Label not found");
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.LABEL_UNDEFINED_ERROR);
            }
            return check;
        }
        private bool CheckLabels(string input)
        {

            Debug.Assert(input != null);
            Debug.Assert(labels != null);
            string[] inputSplit = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in inputSplit)
            {
                if (!labels.Contains(item))
                    return false;
            }
            return true;
        }
        private bool CheckLabels(List<string> inputLabels)
        {
            foreach (string item in inputLabels)
            {
                if (!labels.Contains(item))
                    return false;
            }
            return true;
        }
        private void SetParsedInput()
        {
            parsedInput.Add(commandName.ToString());
            parsedInput.Add(taskDetails);
            parsedInput.Add(taskTime);
            if (taskLabels == "")
                parsedInput.Add(String.Concat(inputLabels));
            else
                parsedInput.Add(taskLabels);
            parsedInput.Add(taskImportance.ToString());
            parsedInput.Add(taskInterval);
        }
        public void LabelParse(string input, List<string> labels)
        {

            Debug.Assert(input != null);
            Debug.Assert(labels != null);
            char[] splitter = new char[] { ' ', ';' };
            parsedInput = new List<string>();
            List<string> betaParse = new List<string>(input.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            if (betaParse.Count < 3 || (betaParse[1] != "add" && betaParse[1] != "delete"))
            {
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.LABEL_INPUT_ERROR);
                return;
            }
            parsedInput.Add(betaParse[0].ToLower());
            parsedInput.Add(betaParse[1].ToLower());
            parsedInput.Add(betaParse[2].ToLower());
        }
        public void CommandParse(string input)
        {

            Debug.Assert(input != null);
            parsedInput = new List<string>();
            if (input.Contains(';'))
            {
                char[] splitter = new char[] { ';' };
                parsedInput = new List<string>(input.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                char[] splitter = new char[] { ' ' };
                parsedInput = new List<string>(input.Split(splitter, 2, StringSplitOptions.RemoveEmptyEntries));
            }
            parsedInput[0] = parsedInput[0].ToLower();
        }
        private TimeSpan GetRepeat(string dateInput)
        {

            Debug.Assert(dateInput != null);
            TimeSpan interval;
            if (dateInput.Contains('%'))
            {
                int posmod = dateInput.IndexOf('%');
                string next = dateInput.Substring(posmod + 1).Split(' ', ';')[0];
                if (next == "w" || next == "weekly")
                {
                    interval = Utility.WEEK_INTERVAL;
                }
                else if (next == "m" || next == "monthly")
                {
                    interval = Utility.MONTH_INTERVAL;
                }
                else if (Char.IsDigit(next[0]))
                {
                    interval = new TimeSpan(int.Parse(next[0].ToString()), 0, 0, 0);
                }
                else
                    interval = Utility.NO_INTERVAL;
            }
            else
                interval = Utility.NO_INTERVAL;
            return interval;
        }
        public void FieldParse(string input)
        {
            Debug.Assert(input != null);
            List<string> betaParse = new List<string>(input.Split(';')), parsedInput = new List<string>();

            taskDetails = betaParse[0].Trim();
            taskTime = ToDate(betaParse[1].Trim().ToLower()).ToString(Utility.SHORT_DATE_FORMAT);   //Deadline
            taskLabels = betaParse[2].Trim().ToLower();                                             //Labels
            taskImportance = (betaParse[3].Trim().ToLower() == "high").ToString();                  //Priority
            taskInterval = GetRepeat(betaParse[1].Trim()).ToString();                               //Repetition
        }
        public string ToDayValid(string day)
        {
            Debug.Assert(day != null);
            if (day.Contains("monday") || day.Contains("mon"))
                return "monday";
            else if (day.Contains("tuesay") || day.Contains("tue"))
                return "tuesday";
            else if (day.Contains("wednesday") || day.Contains("wed"))
                return "wednesday";
            else if (day.Contains("thursday") || day.Contains("thu") || day.Contains("thurs"))
                return "thursday";
            else if (day.Contains("friday") || day.Contains("fri"))
                return "friday";
            else if (day.Contains("saturday") || day.Contains("sat"))
                return "saturday";
            else if (day.Contains("sunday") || day.Contains("sun"))
                return "sunday";
            else if (day.Contains("today"))
                return "today";
            else if (day.Contains("tomorrow") || day.Contains("tom"))
                return "tomorrow";
            else
            {
                return "nil";
            }
        }
        private DayOfWeek toDay(string day)
        {
            Debug.Assert(day != null);
            day = day.Trim();
            if (day == "monday" || day == "mon")
                return DayOfWeek.Monday;
            else if (day == "tuesday" || day == "tue")
                return DayOfWeek.Tuesday;
            else if (day == "wednesday" || day == "wed")
                return DayOfWeek.Wednesday;
            else if (day == "thursday" || day == "thu")
                return DayOfWeek.Thursday;
            else if (day == "friday" || day == "fri")
                return DayOfWeek.Friday;
            else if (day == "saturday" || day == "sat")
                return DayOfWeek.Saturday;
            else if (day == "sunday" || day == "sun")
                return DayOfWeek.Sunday;
            else if (day == "tomorrow")
                return System.DateTime.Today.AddDays(1).DayOfWeek;
            else
                return System.DateTime.Today.DayOfWeek;
        }
        private int NumberOfDays(string day)
        {
            Debug.Assert(day != null);
            DayOfWeek deadline = toDay(day);
            DayOfWeek curDay = System.DateTime.Today.DayOfWeek;
            if (deadline >= curDay)
                return (deadline - curDay);
            else
                return deadline - curDay + 7;
        }
        private DateTime updateTime(ref DateTime Template, DateTime containsTime)
        {
            Template = Template.AddHours(containsTime.Hour);
            Template = Template.AddMinutes(containsTime.Minute);
            Template = Template.AddSeconds(containsTime.Second);
            return Template;
        }
        private string RemoveDay(string date, string day)
        {
            Debug.Assert(day != null);
            Debug.Assert(date != null);
            day = day.Substring(0, 3);
            string[] dateParse = date.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dateParse.Length; i++)
                if (dateParse[i].Contains(day))
                    dateParse[i] = "";
            return String.Concat(dateParse);
        }
        public DateTime ToDate(string toBeConverted)
        {
            Debug.Assert(toBeConverted != null);
            if (toBeConverted.Length == 0)
                return Utility.DEFAULT_UNDEFINED_DATE;
            DateTime tempDate = new DateTime();
            string day = ToDayValid(toBeConverted.ToLower().Trim());
            if (day != "nil")
            {
                tempDate = System.DateTime.Today.Date;
                int x = NumberOfDays(day);
                tempDate = tempDate.AddDays(x * 1.0);
                toBeConverted = RemoveDay(toBeConverted, day);
                DateTime tempTime;
                try
                {
                    tempTime = DateTime.Parse(toBeConverted);
                    tempDate = updateTime(ref tempDate, tempTime);
                }
                catch (Exception e)
                {
                }
            }
            else
            {

                try
                {
                    tempDate = DateTime.Parse(toBeConverted);
                }
                catch (Exception e)
                {
                    if (e is FormatException)
                        return Utility.DEFAULT_ERROR_DATE;
                }
            }
            return tempDate;
        }
    }
}
