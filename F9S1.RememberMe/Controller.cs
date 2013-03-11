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
    /// The head function of the Logic Layer, it manages interaction between various classes in Logic as well as interactions with other Layers.
    /// </summary>
    class Controller
    {
        Parser parse;
        Export gSync;
        Storage store;
        Operations taskData;
        MainWindow startWindow;
        bool isModified;
        List<string> output, parsedInput;
        string input, commandName;

        /// <summary>
        /// Initializes the objects for many other classes. Also contains a reference to the current instance of MainWindow.
        /// </summary>
        /// <param name="appWindow">The object for which the reference is created.</param>
        public Controller(MainWindow appWindow)
        {
            Initialize();
            startWindow = appWindow;
            AlarmCheck checkAlarm = new AlarmCheck(this);
        }
        
        /// <summary>
        /// Initializes the objects for many other classes.
        /// </summary>
        public Controller()
        {
            Initialize();
        }
        private void Initialize()
        {
            gSync = new Export();
            parse = new Parser();
            store = new Storage();
            taskData = new Operations(store.ReadTasks(), store.ReadLabels());
        }

        /// <summary>
        /// Sends the input to parser, checks the parsed input and depending on the task calls the corresponding function in operations.
        /// </summary>
        /// <param name="userInput">The input given.</param>
        /// <returns>The output to be displayed.</returns>
        public List<string> UserDispatch(string userInput)
        {
            Debug.Assert(userInput != null && userInput != "");
            input = userInput;
            
            output = new List<string>();
            isModified = false;
            CheckClose(input);
            
            parsedInput = parse.InputParse(input, taskData.GetLabels());
            Debug.Assert(parsedInput != null);

            SwitchDispatch();
            
            if (isModified)
            {
                store.WriteTasks(taskData.GetList(), taskData.GetLabels());
                taskData.UpdateTasks();
            }

            if (output.Count == 0)
                output = taskData.Display();
            return output;
        }
        private void CheckClose(string input)
        {
            if (input.Trim().Length > 3)
            {
                if (input.Trim().ToLower().Equals("exit") ||
                    input.Trim().ToLower().Equals("quit"))
                {
                    store.WriteTasks(taskData.GetList(), taskData.GetLabels());
                    Environment.Exit(0);
                }
            }
        }
        private void SwitchDispatch()
        {
            commandName = parsedInput[0];
            if (commandName != Utility.ERROR)
                parsedInput.RemoveAt(0);

            switch (commandName)
            {

                case "label":
                    {
                        LabelDispatch();
                        break;
                    }
                case "sync":
                    {
                        gSync.Synchronize(parsedInput[0], parsedInput[1], taskData.TaskList);
                        break;
                    }
                case "add":
                    {
                        AddDispatch();
                        break;
                    }

                case "delete":
                    {
                        DeleteDispatch();
                        break;
                    }

                case "edit":
                    {
                        EditDispatch();
                        break;
                    }
                case "archive":
                    {
                        ArchiveDispatch();
                        break;
                    }
                case "undo":
                    {
                        isModified = taskData.UndoAction();
                        break;
                    }
                case "redo":
                    {
                        isModified = taskData.RedoAction();
                        break;
                    }
                case "clear":
                    {
                        isModified = taskData.ClearTasks();
                        break;
                    }
                case Utility.ERROR:
                    {
                        output = parsedInput;
                        break;
                    }
            }
        }
        private void LabelDispatch()
        {
            if (parsedInput[0] == "add")
                isModified = taskData.AddLabel(parsedInput[1]);
            else if (parsedInput[0] == "delete")
                isModified = taskData.DeleteLabel(parsedInput[1]);
        }
        private void AddDispatch()
        {
            isModified = taskData.AddTask(parsedInput);
            if (!isModified)
            {
                output.Add(Utility.ERROR);
                output.Add(Utility.ADD_ERROR);
            }
        }
        private void DeleteDispatch()
        {
            if (!(parsedInput.Count < 1))
                isModified = taskData.DeleteTask(parsedInput[0]);
            if (!isModified)
            {
                output.Add(Utility.ERROR);
                output.Add(Utility.DELETE_ERROR);
            }
        }
        private void EditDispatch()
        {
            isModified = taskData.EditTask(parsedInput, input);
            if (!isModified)
            {
                output.Add(Utility.ERROR);
                output.Add(Utility.EDIT_ERROR);
            }
        }
        private void ArchiveDispatch()
        {
            if (!(parsedInput.Count < 1))
                isModified = taskData.ArchiveTask(parsedInput[0]);
            if (!isModified)
            {
                output.Add(Utility.ERROR);
                output.Add(Utility.ARCHIVE_ERROR);
            }
        }
        
        /// <summary>
        /// Updates the display in the mainwindow.
        /// </summary>
        public void UpdateDisplay()
        {
            startWindow.SetDisplay();
        }

        /// <summary>
        /// Obtains a copy of the list of tasks from Operations
        /// </summary>
        /// <returns>List of Tasks</returns>
        public List<Task> GetTasks()
        {
            return taskData.TaskList;
        }
    }
}
