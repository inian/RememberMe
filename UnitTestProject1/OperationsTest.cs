using F9S1.RememberMe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
    
    
    /// <summary>
    ///This is a test class for OperationsTest and is intended
    ///to contain all OperationsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OperationsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for DeleteLabel - label exists
        ///</summary>
        [TestMethod()]
        public void DeleteLabelTest()
        {
            List<string> stringListTasks = new List<string>();
            List<string> labelList = new List<string>();
            labelList.Add("work");
            labelList.Add("home");
            labelList.Add("others");
            Operations target = new Operations(stringListTasks, labelList);
            string newLabel = "work"; 
            bool expected = true;
            bool actual;
            actual = target.DeleteLabel(newLabel);
            Assert.AreEqual(expected, actual);

            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DeleteLabel - label does not exist
        ///</summary>
        [TestMethod()]
        public void DeleteLabelTest1()
        {
            List<string> stringListTasks = new List<string>();
            List<string> labelList = new List<string>();
            labelList.Add("work");
            labelList.Add("home");
            labelList.Add("others");
            Operations target = new Operations(stringListTasks, labelList);
            string newLabel = "labeldoesnotexist"; 
            bool expected = false; 
            bool actual;
            actual = target.DeleteLabel(newLabel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for AddLabel
        ///</summary>
        [TestMethod()]
        public void AddLabelTest()
        {
            List<string> stringListTasks = new List<string>();
            List<string> labelList = new List<string>(); 
            Operations target = new Operations(stringListTasks, labelList); 
            string newLabel = "Academics"; 
            bool expected = true; 
            bool actual;
            actual = target.AddLabel(newLabel);
            Assert.AreEqual(expected, actual);
        }



        /// <summary>
        ///A test for DeleteTask
        ///</summary>
        [TestMethod()]
        public void DeleteTaskTest()
        {
            List<string> stringListTasks = new List<string>();
            List<string> labelList = new List<string>();
            stringListTasks.Add("finish assignment ~~ 12:00 AM 08 Nov 2011 ~~  work ~~ False ~~ False ~~ 00:00:00");
            stringListTasks.Add("play counter strike ~~ 12:00 AM 11 Nov 2011 ~~  ~~ True ~~ False ~~ 00:00:00");
            Operations target = new Operations(stringListTasks, labelList);
            string taskDetails = "finish assignment"; 
            bool expected = true; 
            bool actual;
            actual = target.DeleteTask(taskDetails);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DeleteTask
        ///</summary>
        [TestMethod()]
        public void DeleteTaskTest1()
        {
            List<string> stringListTasks = new List<string>();
            List<string> labelList = new List<string>();
            stringListTasks.Add("finish assignment ~~ 12:00 AM 08 Nov 2011 ~~  work ~~ False ~~ False ~~ 00:00:00");
            stringListTasks.Add("play counter strike ~~ 12:00 AM 11 Nov 2011 ~~  ~~ True ~~ False ~~ 00:00:00");
            Operations target = new Operations(stringListTasks, labelList);
            string taskDetails = "Task not found"; 
            bool expected = false; 
            bool actual;
            actual = target.DeleteTask(taskDetails);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ArchiveTask - Task not not archived before
        ///</summary>
        [TestMethod()]
        public void ArchiveTaskTest()
        {
            List<string> stringListTasks = new List<string>();
            List<string> labelList = new List<string>();
            stringListTasks.Add("finish assignment ~~ 12:00 AM 08 Nov 2011 ~~  work ~~ False ~~ False ~~ 00:00:00");
            stringListTasks.Add("play counter strike ~~ 12:00 AM 11 Nov 2011 ~~  ~~ True ~~ False ~~ 00:00:00");
            Operations target = new Operations(stringListTasks, labelList);
            string taskDetails = "play counter strike";
            bool expected = true; 
            bool actual;
            actual = target.ArchiveTask(taskDetails);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ArchiveTask - Task archived before
        ///</summary>
        [TestMethod()]
        public void ArchiveTaskTest1()
        {
            List<string> stringListTasks = new List<string>();
            List<string> labelList = new List<string>();
            stringListTasks.Add("finish assignment ~~ 12:00 AM 08 Nov 2011 ~~  work ~~ False ~~ False ~~ 00:00:00");
            stringListTasks.Add("play counter strike ~~ 12:00 AM 11 Nov 2011 ~~  ~~ True ~~ True ~~ 00:00:00");
            Operations target = new Operations(stringListTasks, labelList);
            string taskDetails = "play counter strike";
            bool expected = false; 
            bool actual;
            actual = target.ArchiveTask(taskDetails);
            Assert.AreEqual(expected, actual);
        }
    }
}
