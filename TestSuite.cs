﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ABSA_Assignment
{
    [TestClass]
    public class TestSuite
    {

        private TestContext testContext;
        static Application app { get; set; }

        public TestContext TestContext
        {
            get
            {
                return testContext;
            }
            set
            {
                testContext = value;
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            app = new Application();
        }

        [TestMethod]
        public void Task1()
        {
            app.report.test = TestContext.TestName;

            app.task1.GetList();

            app.task1.GetRetrieverList();

            app.task1.GetRandomImage();
        }

        [TestMethod]
        public void Task2()
        {

            app.report.test = TestContext.TestName;

            app.util.LaunchDriver();

            app.task2.ValidateUserIsPresent("User Name", "novak");//hardcoded af

            app.task2.AddUsers();

            app.task2.ValidateNewUsersIsPresent();

            app.util.ShutDown();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            app.report.EndTest(TestContext.TestName + " Completed");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            app.report.OpenReport();
        }
    }
}