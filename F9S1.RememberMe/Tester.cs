//Nalin Ilango

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace F9S1.RememberMe
{
    /// <summary>
    /// Used for testing purposes. It can run tests against a standard output file, or create an output file given the input.
    /// </summary>
    class Tester
    {
        Controller testDispatch;
        List<string> testCases, testResults, expectedResults, storeContents, storeLabels;
        
        public Tester()
        {
            checkFiles();
            testDispatch = new Controller();
            testCases = new List<string>();
            testResults = new List<string>();
            expectedResults = new List<string>();
            storeContents = new List<string>();
            storeLabels = new List<string>();
        }

        /// <summary>
        /// If the files do not exist, creates the files.
        /// </summary>
        private void checkFiles()
        {
            if (!File.Exists(Utility.INPUT_FILE))
            {
                StreamWriter inputStream = new StreamWriter(Utility.INPUT_FILE);
                inputStream.Close();
            }
            if (!File.Exists(Utility.OUTPUT_FILE))
            {
                StreamWriter outputStream = new StreamWriter(Utility.OUTPUT_FILE);
                outputStream.Close();
            }
        }

        /// <summary>
        /// Gets input, expected output and runs the tests. If there are any failed cases, get the failed cases.
        /// </summary>
        public void Test()
        {
            storeContents = ReadLines(Utility.CONTENT_FILE_NAME);
            storeLabels = ReadLines(Utility.LABEL_FILE_NAME);
            testCases = ReadLines(Utility.INPUT_FILE);
            expectedResults = ReadLines(Utility.OUTPUT_FILE);
            RunTests();
            WriteLines(Utility.LABEL_FILE_NAME, storeLabels);
            WriteLines(Utility.CONTENT_FILE_NAME, storeContents);
            if (!AreResultsCorrect())
                AssertResults();
        }

        /// <summary>
        /// Reads the contents of the given file and returns the List of Strings.
        /// </summary>
        /// <param name="fileName">The file to read from.</param>
        /// <returns>The contents of the file.</returns>
        private List<String> ReadLines(string fileName)
        {
            List<string> contents = new List<string>();
            Debug.Assert(File.Exists(fileName));
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    contents.Add(line);
            }
            return contents;
        }
 
        /// <summary>
        /// Runs the tests, stores the output in a local List of Strings.
        /// </summary>
        private void RunTests()
        {
            foreach (string line in testCases)
                testResults.Add(ListToString(testDispatch.UserDispatch(line)));
        }

        private string ListToString(List<string> lines)
        {
            string singleLine = "";
            foreach (string line in lines)
                singleLine += Utility.FILE_SEPARATER + line;
            return singleLine;
        }

        /// <summary>
        /// Returns true if the current output matches the expected output.
        /// </summary>
        /// <returns>True if all cases match, false otherwise</returns>
        private bool AreResultsCorrect()
        {
            return testResults.Equals(expectedResults);
        }

        /// <summary>
        /// Using assert, displays input, output and expected output of failed testcases.
        /// </summary>
        private void AssertResults()
        {
            for (int i = 0; i < testCases.Count; i++)
                Debug.Assert(expectedResults[i] == testResults[i], "Input: " + testCases[i] + "\nOutput: " + testResults[i] + "\nExpected: " + expectedResults[i]);
        }

        /// <summary>
        /// Writes the output for the testcases read from the input file.
        /// Use this only when the expected output changes, and you are sure that this output is correct.
        /// </summary>
        public void GetOutputFile()
        {
            testCases = ReadLines(Utility.INPUT_FILE);
            Debug.Assert(File.Exists(Utility.INPUT_FILE));
            Debug.Assert(File.Exists(Utility.OUTPUT_FILE));
            RunTests();
            WriteLines(Utility.OUTPUT_FILE, testResults);
        }

        /// <summary>
        /// Writes the list of strings to the given file.
        /// </summary>
        /// <param name="fileName">The file to write to.</param>
        /// <returns>The contents to be written.</returns>
        private void WriteLines(string fileName, List<string> contents)
        {
            Debug.Assert(File.Exists(fileName));
            using (StreamWriter writer = new StreamWriter(fileName))
                foreach (string line in contents)
                    writer.WriteLine(line);
        }
    }
}
