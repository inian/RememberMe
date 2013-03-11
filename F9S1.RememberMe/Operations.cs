//Akshay Viswanathan
//Abhishek Ravi

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace F9S1.RememberMe
{
    /// <summary>
    /// This class encapsulates the task list and its associate functions. 
    /// The tasklist's functions are called by the controller depending on the user input.
    /// Also contains the undo and redo functions.
    /// </summary>
    class Operations
    {
        
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        List<Task> taskList;
        List<string> labels;
        Stack<List<Task>> undoStack, redoStack;
        
        public List<Task> TaskList
        {
            get
            {
                return new List<Task>(taskList);
            }
        }

        public List<string> Labels
        {
            get
            {
                return labels;
            }
        }
        
        /// <summary>
        /// Initializes the tasklist, the label list and the undo, redo stacks.
        /// </summary>
        /// <param name="stringListTasks">The list of strings to initialize tasks with</param>
        /// <param name="labelList">The list of labels</param>
        public Operations(List<string> stringListTasks, List<string> labelList)
        {
            Debug.Assert(stringListTasks != null);
            Inititalize();
            for (int i = 0; i < stringListTasks.Count; i++)
            {
                taskList.Add(new Task(stringListTasks[i]));
            }
            for (int i = 0; i < labelList.Count; i++)
            {
                labels.Add(labelList[i]);
            }
        }

        private void Inititalize()
        {
            taskList = new List<Task>();
            labels = new List<string>();
            undoStack = new Stack<List<Task>>();
            redoStack = new Stack<List<Task>>();
            undoStack.Push(new List<Task>(taskList));
        }

        /// <summary>
        /// Update the redostack and undostack. Is called whenever a change is made to the tasklist.
        /// </summary>
        public void UpdateTasks()
        {
            if ((redoStack.Count > 0) && (taskList.Equals(redoStack.Peek())))
            {
                undoStack.Push(redoStack.Pop());
            }
            else
            {
                undoStack.Push(new List<Task>(taskList));
                redoStack.Clear();
            }
        }   

        /// <summary>
        /// Undos the previous action committed. 
        /// </summary>
        /// <returns>Always returns false, writing to file is done next time another operation is called, or the program closes.</returns>
        public bool UndoAction()
        {
            if (undoStack.Count > 1)
            {
                redoStack.Push(undoStack.Pop());
                taskList = new List<Task>(undoStack.Peek());
            }
            else
                logger.Info("No more undos");
            return false;
        }

        /// <summary>
        /// Redoes the previous undo.
        /// </summary>
        /// <returns>Always returns false, writing to file is done next time another operation is called, or the program closes.</returns>
        public bool RedoAction()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(redoStack.Pop());
                taskList = new List<Task>(undoStack.Peek());
            }
            else
                logger.Info("No more redos");
            return false;
        }

        /// <summary>
        /// Gets the local list of labels.
        /// </summary>
        /// <returns>Returns the tasks as a list of strings.</returns>
        public List<string> GetLabels()
        {
            return new List<string>(labels);
        }
        
        /// <summary>
        /// Gets the local list of tasks.
        /// </summary>
        /// <returns>Returns the tasks as a list of strings.</returns>
        public List<string> GetList()
        {
            List<string> stringListTasks = new List<string>();
            for (int i = 0; i < taskList.Count; i++)
            {
                stringListTasks.Add(taskList[i].ToString());
            }
            return stringListTasks;
        }

        /// <summary>
        /// Deletes the specified label from the list of labels.
        /// </summary>
        /// <param name="newLabel">The label to be deleted.</param>
        /// <returns>True if successfully deleted, false otherwise.</returns>
        public bool DeleteLabel(string newLabel)
        {
            Debug.Assert(newLabel != null);
            for (int i = 0; i < labels.Count; i++)
                if (labels[i].ToLower() == newLabel.ToLower())
                {
                    labels.Remove(newLabel);
                    return true;
                }
            return false;
        }      

        /// <summary>
        /// Adds the given label to the list of labels.
        /// </summary>
        /// <param name="newLabel">The string to be added.</param>
        /// <returns>True if successfully added, false otherwise.</returns>
        public bool AddLabel(string newLabel)
        {
            Debug.Assert(newLabel != null);
            for (int i = 0; i < labels.Count; i++)
                if (labels[i].ToLower() == newLabel.ToLower())
                    return false;
            labels.Add(newLabel);
            
            return true;
        }

        /// <summary>
        /// Adds a task to the task list.
        /// </summary>
        /// <param name="newTask">A list of strings which contain details of the task.</param>
        /// <returns>True if successfully added, false otherwise.</returns>
        public bool AddTask(List<string> newTask)
        {
            Debug.Assert(newTask != null);
            newTask[0] = GetDuplicate(newTask[0]);
            taskList.Add(new Task(newTask));
            return true;
        }

        /// <summary>
        /// Creates a duplicate name based on the number of already present tasks with the same names, if there are other tasks.
        /// </summary>
        /// <param name="taskDetails">The task name to be checked.</param>
        /// <returns>The duplicate name.</returns>
        private string GetDuplicate(string taskDetails)
        {
            Debug.Assert(taskDetails != null);
            int count = 0;
            string newDetails = taskDetails;
            bool isModified = true;
            while (isModified)
            {
                isModified = false;
                for (int i = 0; i < taskList.Count; i++)
                    if (taskList[i].Details == newDetails)
                    {
                        count++;
                        isModified = true;
                    }
                if (count != 0)
                {
                    newDetails = taskDetails + "(" + count + ")";
                }
            }
            if (isModified)
                logger.Info("Task with same name exists");
            return newDetails;
        }

        /// <summary>
        /// Deletes the task whose details are given from the task list.
        /// </summary>
        /// <param name="taskDetails">Details of the task to be deleted.</param>
        /// <returns>True if successfuly deleted, false otherwise.</returns>
        public bool DeleteTask(string taskDetails)
        {
            Debug.Assert(taskDetails != null);
            Task foundTask = SearchTask(taskDetails);
            Task Temp = foundTask;
            if (foundTask != null)
            {
                  taskList.Remove(foundTask);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Archives the task whose details are given.
        /// </summary>
        /// <param name="taskDetails">Details of the task to be archived.</param>
        /// <returns>True if successfuly archived, false otherwise.</returns>
        public bool ArchiveTask(string taskDetails)
        {
            int n = -1;
            Debug.Assert(taskDetails != null);
            Task tempTask = SearchTask(taskDetails, ref n);
            if (tempTask != null && !tempTask.IsArchived)
            {
                ArchiveByInterval(new Task(tempTask.ToString()), n);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if it the given task has an interval or not, and archives accordingly.
        /// </summary>
        /// <param name="foundTask">The task to be archived</param>
        /// <param name="n">The position of the task in the task list</param>
        private void ArchiveByInterval(Task foundTask, int n)
        {
            foundTask.IsArchived = true;
            if (foundTask.Interval != TimeSpan.Parse("00:00:00"))
            {
                DeleteTask(foundTask.Details);
                foundTask.Deadline = foundTask.Deadline.Add(foundTask.Interval);
                foundTask.IsArchived = false;
                taskList.Insert(n, foundTask);
            }
            else
            {
                taskList[n] = foundTask;
            }
        }

        /// <summary>
        /// Clears all tasks in the task list.
        /// </summary>
        /// <returns>Always true.</returns>
        public bool ClearTasks()
        {
            taskList.Clear();
            return true;
        }

        /// <summary>
        /// Edits the given task in the task list.
        /// </summary>
        /// <param name="editInput">The details to be edited.</param>
        /// <param name="input">Given by the input.</param>
        /// <returns>True if succesfully edited, false otherwise.</returns>
        public bool EditTask(List<string> editInput, string input)
        {
            Debug.Assert(input != null);
            Debug.Assert(editInput != null);
            int n = -1;
            if (editInput.Count < 5)
                return false;
            Task foundTask = SearchTask(editInput[0], ref n);
            if (foundTask != null && foundTask.IsArchived == false)
            {
                CheckEdit(ref editInput, input, foundTask);
                taskList[n] = new Task(editInput);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks which fields to edit by comparing to the user input.
        /// </summary>
        /// <param name="editInput">The parsed input.</param>
        /// <param name="input">The user input.</param>
        /// <param name="foundTask">The task to be edited.</param>
        private void CheckEdit(ref List<string> editInput, string input, Task foundTask)
        {
            if (!(input.Contains('@')))
            {
                editInput[1] = foundTask.Deadline.ToString(Utility.SHORT_DATE_FORMAT);
                editInput[4] = foundTask.Interval.ToString();
            }
            if (!(input.Contains('#')))
                editInput[2] = foundTask.Labels.ToString();
        }

        /// <summary>
        /// Searches in the task list for the task which matches the details given.
        /// </summary>
        /// <param name="taskDetails">Given by the user.</param>
        /// <returns>The task, if found.</returns>
        private Task SearchTask(string taskDetails)
        {
            Debug.Assert(taskDetails != null);
            Task toBeFound = null;
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].Details.Equals(taskDetails))
                {
                    toBeFound = taskList[i];
                }
            }

            logger.Info("Task not found");
            return toBeFound;
        }

        /// <summary>
        /// Searches in the task list for the task which matches the details given.
        /// </summary>
        /// <param name="taskDetails">Given by the user.</param>
        /// <param name="k">An integer to contain the position of the task in the tasklist.</param>
        /// <returns>The task, if found.</returns>
        public Task SearchTask(string taskDetails, ref int k)
        {
            Debug.Assert(taskDetails != null);
            Task toBeFound = null;
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].Details.Equals(taskDetails))
                {
                    toBeFound = taskList[i];
                    k = i;
                }
            }

            logger.Info("Task not found");
            return toBeFound;
        }

        /// <summary>
        /// Gets a list of all non-archived tasks in the tasklist.
        /// </summary>
        /// <returns>The tasks as a list of strings.</returns>
        public List<string> Display()
        {
            List<string> taskDetails = new List<string>();
            for (int i = 0; i < taskList.Count; i++)
                if(taskList[i].IsArchived==false)
                    taskDetails.Add(taskList[i].ToString());
            return taskDetails;
        }
    }
}
