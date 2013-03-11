//Abhishek Ravi

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace F9S1.RememberMe
{
    /// <summary>
    /// This class manages the export to the google calendar.
    /// </summary>
    class Export
    {
        List<Task> taskList;
        CalendarService Gcal;
        EventQuery query;

        /// <summary>
        /// Gets the RM! tasks from GCal and exports the current tasklist to it, each task prefixed with a tag.
        /// </summary>
        /// <param name="username">Given by user.</param>
        /// <param name="password">Given by user.</param>
        /// <param name="taskList">Obtained from controlle.r</param>
        public void Synchronize(string username, string password, List<Task> listOfTasks)
        {
            taskList = listOfTasks;
            GoogleGetTasks(username, password);
            GoogleDelete();
            GoogleAdd();
        }

        /// <summary>
        /// Gets the old tasklist from GCal.
        /// </summary>
        /// <param name="username">Given by user.</param>
        /// <param name="password">Given by user.</param>
        private void GoogleGetTasks(string username, string password)
        {
            Gcal = new CalendarService("remMe");
            Gcal.setUserCredentials(username, password);
            query = new EventQuery("https://www.google.com/calendar/feeds/default/private/full");
            EventFeed feed = Gcal.Query(query);
        }

        /// <summary>
        /// Deletes the old tasks from GCal (all tasks prefixed with RM!).
        /// </summary>
        private void GoogleDelete()
        {
            query.Query = Utility.RM_TAG;
            EventFeed allTasks = Gcal.Query(query);
            for (int i = 0; i < allTasks.Entries.Count; i++)
            {
                AtomEntry task = allTasks.Entries[i];
                task.Delete();
            }
        }

        /// <summary>
        /// Adds the current tasklist to GCal prefixed with RM!
        /// </summary>
        private void GoogleAdd()
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                EventEntry entry = new EventEntry();
                entry.Title.Text = Utility.RM_TAG + taskList[i].Details;
                entry.Content.Content = "Label = " + taskList[i].Labels;
                if (taskList[i].Deadline.Year != DateTime.MaxValue.Year)
                {
                    When eventTime = new When(taskList[i].Deadline, taskList[i].Deadline.AddHours(1));
                    entry.Times.Add(eventTime);
                }
                Uri postUri = new Uri("https://www.google.com/calendar/feeds/default/private/full");
                AtomEntry insertedEntry = Gcal.Insert(postUri, entry);
            }
        }
    }
}

