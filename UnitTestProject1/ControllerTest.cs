using F9S1.RememberMe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
    
    
    /// <summary>
    ///This is a test class for ControllerTest and is intended
    ///to contain all ControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ControllerTest
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
        ///A test for UserDispatch - testing normal add
        ///</summary>
        [TestMethod()]
        public void UserDispatchTest()
        {
            Controller target = new Controller(); 
            string userInput = "add test"; 
            List<string> expected = new List<string>();
            expected.Add("test ~~ 11:59 PM 31 Dec 9999 ~~  ~~ False ~~ False ~~ 00:00:00");
            List<string> actual;
            actual = target.UserDispatch(userInput);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UserDispatch
        ///</summary>
      /*  [TestMethod()]
        public void UserDispatchTest1()
        {
            Controller target = new Controller();
            string userInput = ""; 
            List<string> expected = null; 
            List<string> actual;
            actual = target.UserDispatch(userInput);
            Assert.AreEqual(expected, actual);
        }*/
    }
}
