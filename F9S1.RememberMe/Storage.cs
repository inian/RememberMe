//Akshay Viswanathan

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using System.Diagnostics;

namespace F9S1.RememberMe
{
    class Storage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Storage()
        {
            if (!File.Exists(Utility.CONTENT_FILE_NAME))
            {
                logger.Warn("Contents file did not exist");
                StreamWriter contentStream = new StreamWriter(Utility.CONTENT_FILE_NAME);
                contentStream.Close();
            }

            if (!File.Exists(Utility.LABEL_FILE_NAME))
            {
                logger.Warn("Label file did not exist");
                StreamWriter labelStream = new StreamWriter(Utility.LABEL_FILE_NAME);
                labelStream.WriteLine("others");
                labelStream.WriteLine("work");
                labelStream.WriteLine("home");
                labelStream.Close();
            }
        }

        private void WriteLabels(List<string> labels)
        {
            TextWriter writer = new StreamWriter(Utility.LABEL_FILE_NAME);
            for (int i = 0; i < labels.Count; i++)
            {
                writer.WriteLine(labels[i]);
            }
            writer.Close();
        }

        public List<string> ReadLabels() //Exception if file is missing
        {
            try
            {
                List<string> labels = new List<string>();
                using (StreamReader reader = new StreamReader(Utility.LABEL_FILE_NAME))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        labels.Add(line);
                    }
                }
                return labels;
            }
            catch (FileNotFoundException e)
            {
                logger.Error("Label File not found");

                File.Create(Utility.LABEL_FILE_NAME);

            }
            catch (Exception e)
            {
                logger.Error("Label File unknown error");
            }

            List<string> dummy = new List<String>();
            return dummy;
          }

        public void WriteTasks(List<string> contents,List<string> labels) //Exception if file is missing
        {
            try
            {
                WriteLabels(labels);
            }

            catch (Exception e)
            { logger.Error("Label unknown error");
            
            }
            try
            {
                TextWriter writer = new StreamWriter(Utility.CONTENT_FILE_NAME);
                for (int i = 0; i < contents.Count; i++)
                {
                    writer.WriteLine(contents[i]);
                }
                writer.Close();
            }
            catch (FileNotFoundException e)
            {
                logger.Error("Label File not found");
                File.Create(Utility.CONTENT_FILE_NAME);

            }
            catch (Exception e)
            {
                logger.Error("Label File unknown error");
            }
        }

        
        public List<string> ReadTasks() //Exception if file is missing
        {
            try
            {
                List<string> contents = new List<string>();
                using (StreamReader reader = new StreamReader(Utility.CONTENT_FILE_NAME))
                {

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        contents.Add(line);
                    }
                }
                return contents;

            }
            catch (FileNotFoundException e)
            {
                logger.Error("Contents File not found");
                File.Create(Utility.CONTENT_FILE_NAME);

            }
            catch (Exception e)
            {
                logger.Error("File unknown error");
            } 
            List<string> dummy = new List<String>();
            return dummy;
            }

    }
}
