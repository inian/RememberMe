//Abhishek Ravi
//Nalin Ilango

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F9S1.RememberMe
{
    /// <summary>
    /// It contains common utilities; values which are used by more than one class and constant strings.
    /// </summary>
    class Utility 
    {
        public Utility()
        {
        }
        public const string EDIT_PRINT = "edit: ";
        public const string ADD_ERROR = "Add failed. Please check your input";
        public const string DELETE_ERROR = "Delete failed. Please check your input";
        public const string EDIT_ERROR = "Edit Failed. Please check your input";
        public const string SORT_ERROR = "Sort Failed. Please check your input";
        public const string ARCHIVE_ERROR = "Archive Failed. Please check your input";
        public const string DATE_ERROR = "Date Error. Please enter in the format \"<day> <hh:mm>\" or \"<dd-mm-yy> <hh:mm>\"";
        public const string EARLY_DATE_ERROR = "Date/Time has already elapsed. Please check your input";

        public const string ERROR = "Error";
        public const string LABEL_UNDEFINED_ERROR = "Undefined label error. Add new labels using \"label add <newlabel>\"";
        public const string LABEL_INPUT_ERROR = "Input error. The correct way of adding/deleting a label is \"label add/delete <label name>\"";
        public const string INPUT_ERROR = "Input missing";
        public const string LABEL_ERROR = "Label error. Correct way is #<label name>";

        public const string SHORT_DATE_FORMAT = "hh:mm tt dd MMM yyyy";

        public const string DEFAULT_NO_TIME = "undefined";
        public const string FILE_SEPARATER = " ~~ ";
        public const string STARRED = "**";
        public const string UNSTARRED = "--";

        public const string RM_TAG = "[RM!]";

        public static TimeSpan WEEK_INTERVAL = new TimeSpan(7, 0, 0, 0);
        public static TimeSpan MONTH_INTERVAL = new TimeSpan(30, 0, 0, 0);
        public static TimeSpan NO_INTERVAL = new TimeSpan(0, 0, 0, 0);
        public static DateTime DEFAULT_ERROR_DATE = DateTime.MinValue;
        public static DateTime DEFAULT_UNDEFINED_DATE = DateTime.MaxValue;

        public const string INPUT_FILE = "RememberMe.testinput.txt";
        public const string OUTPUT_FILE = "RememberMe.testoutput.txt";
        public const string CONTENT_FILE_NAME = "RememberMe.content.txt";
        public const string LABEL_FILE_NAME = "RememberMe.labels.txt";

        public const string HELP = "Adding a task:" +
                            "\nHave a pending task? Just go ahead and add it into your to-do manager ‘Remember Me!’" +
                            "\nYou can to this by adding the task into the given box. This can be added in the following way:" +
                            "\nadd;<Task name + details>;<Deadline>;<Label>" +
                            "\n                            	                            	(or)" +
                            "\nadd <Task name + details> @<Deadline> #<Label>" +
                            "\n" +
                            "\nArchiving a task:" +
                            "\nDone with a task? Just click on the ‘Tick’ mark next to it to do so." +
                            "\nDon’t want to click? Just type in ‘archive;’ and allow the instant search to help you out every\n"+
                            "time you type in a letter. Once the task is printed on top, press <Tab> to get the complete task name\n"+ 
                            "in the Textbox and just press enter to archive the task." +
                            "\n" +
                            "\nDeleting a task:" +
                            "\nWant to delete a task? Again, we have two options. Click the ’X’ mark next to it to do so." +
                            "\nYou can also type in delete; and again use the instant search to find the appropriate task. Either type\n"+
                            " in the full name or press <Tab> followed by enter." +
                            "\n" +
                            "\nEdit a task:" +
                            "\nYou can go ahead and modify a task by either modifying the data directly in the output or type in ‘edit;’ and utilize the instant search to select the task. Once this is done, just specify which value you want to change alone with the changed value to edit the task." +
                            "\n" +
                            "\nUndo an event:" +
                            "\nMade a mistake? Just type in ‘undo’ or press <Ctrl + Z> to undo it." +
                            "\n" +
                            "\nRedo an event:" +
                            "\nWrong undo? Type in ‘redo’ or press <Ctrl + Y> to redo it.";
    }
}
