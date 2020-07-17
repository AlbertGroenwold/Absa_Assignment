using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ABSA_Assignment
{
    [TestClass]
    class Program
    {

        private TestContext testContext;

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

        [TestMethod, TestCategory("Task1")]
        public void Task1()
        {
            Task1 runner = new Task1();
            runner.Run();
        }

        [TestMethod, TestCategory("Task2")]
        public void Task2()
        {
            Task2 runner = new Task2();
            runner.Run();
        }
    }
}
