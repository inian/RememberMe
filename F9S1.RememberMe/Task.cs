//Abhishek Ravi

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace F9S1.RememberMe
{
    class Task
    {
        private string details;     //No semicolons
        public string Details       //Property
        {
            get
            {
                return details;
            }
            set
            {
                details = value;
            }
        }

        private DateTime deadline;  //After now
        public string DisplayDeadline
        {
            get
            {
                if (deadline.Equals(Utility.DEFAULT_UNDEFINED_DATE))
                    return "";
                else
                {
                    string temp = deadline.ToString(Utility.SHORT_DATE_FORMAT);
                    if (temp == "11:59 PM 31 Dec 9999")
                        temp = "";
                    return temp;
                }
            }
            set
            {
                if (value == Utility.DEFAULT_NO_TIME)
                    deadline = Utility.DEFAULT_UNDEFINED_DATE;
                else
                    deadline = DateTime.Parse(value);
            }
        }
        public DateTime Deadline    //Property
        {
            get
            {
                return deadline;
            }
            set
            {
                deadline = value ;          
            }
        }

        private TimeSpan interval;
        public TimeSpan Interval
        {
            get;
            set;
        }

        public bool IsRepeat
        {
            get
            {
                return !(interval.Equals(Utility.NO_INTERVAL));
            }
        }

        private bool isStarred;
        public bool IsStarred       //Property
        {
            get
            {   
                return isStarred;
            }
            set
            {
                isStarred = value;
            }
        }
        
        private bool isArchived;
        public bool IsArchived       //Property
        {
            get
            {
                return isArchived;
            }
            set
            {
                isArchived = value;
            }
        }

        private string[] labels;    //Single word, alphabets, underscore, digits
        public string Labels      //Property
        {
            get
            {
                return ConvertLabelsToString(labels);
            }
            set
            {
                if (value == null)
                    labels = new string[]{""};
                else
                    labels = ConvertStringToLabels(value);
            }
        }

        public Task(List<string> values)
        {
            Debug.Assert(values != null);
            Details = values[0];
            if (values[1] == Utility.DEFAULT_NO_TIME)
                Deadline = Utility.DEFAULT_UNDEFINED_DATE;
            else
                Deadline = DateTime.Parse(values[1]);
            Labels = values[2];
            IsStarred = Boolean.Parse(values[3]);
            IsArchived = false;
            Interval = TimeSpan.Parse(values[4]);
        }
        public Task(string line)
        {
            Debug.Assert(line != null);
            List<string> values = FromString(line);
            Details = values[0];
            if (values[1] == Utility.DEFAULT_NO_TIME)
                Deadline = Utility.DEFAULT_UNDEFINED_DATE;
            else
                Deadline = DateTime.Parse(values[1]);
            Labels = values[2];
            IsStarred = Boolean.Parse(values[3]);
            IsArchived = Boolean.Parse(values[4]);
            Interval = TimeSpan.Parse(values[5]);
            
        }
        
/*        public override int GetHashCode()
        {
            return details.GetHashCode() + deadline.GetHashCode() + isStarred.GetHashCode() + labels.GetHashCode();
        }
*/
        private List<string> FromString(string line)
        {
            Debug.Assert(line != null);
            return new List<string>(line.Split(new string[]{Utility.FILE_SEPARATER},StringSplitOptions.None));
        }

 /*       private string SetIntLength(string shortInt, int length)
        {
            int difference = length - shortInt.Length;
            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                    shortInt = "0" + shortInt;
            }
            return shortInt;
        }
*/
        public override string ToString()
        {

            return Details + Utility.FILE_SEPARATER + Deadline.ToString(Utility.SHORT_DATE_FORMAT) + Utility.FILE_SEPARATER + Labels + Utility.FILE_SEPARATER + IsStarred.ToString() + Utility.FILE_SEPARATER + IsArchived.ToString() + Utility.FILE_SEPARATER + Interval.ToString();
        }

        public override bool Equals(object compareObject)
        {
            if (!((compareObject.GetType()).Equals(typeof(Task)))) //disconnected object problem
                return true;
            Task compareTask = (Task) compareObject;
            return (Details == compareTask.Details) &&
                   (Deadline.Equals(compareTask.Deadline)) &&
                   (IsStarred == compareTask.IsStarred) &&
                   (Labels == compareTask.Labels);
        }

        private string ConvertLabelsToString(string[] toConvert)
        {
            string result = "";
            for (int i = 0; i < toConvert.Length; i++)
            {
               result += " " + toConvert[i];
            }
            return result;
        }

        private string[] ConvertStringToLabels(string toConvert)
        {
            return toConvert.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        public string SetLength(string input, int length)
        {
            int size = input.Length;
            if (size <= length)
            {
                for (int i = 0; i <= (length - size); i++)
                {
                    input = input + " ";
                }
                return input;
            }
            else
            {
                return input.Substring(0, length - 4) + ("...  ");
            }
        }

/*        public string GetDisplay()
        {
            string stars, archives;
            if (IsStarred)
            {
                stars = "*";
            }
            else
            {
                stars = " ";
            }
            if (IsArchived)
            {
                archives = "ARCH.";
            }
                else
            {
                archives = "     ";
            } 
           // return stars + " " + SetLength(Details, 30) + " " + SetLength(Deadline.ToString(Utility.SHORT_DATE_FORMAT), 15) +  " " + SetLength(Labels.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim(), 8) + " " + archives;
            return this.ToString();
        }
*/        
    }
}
 