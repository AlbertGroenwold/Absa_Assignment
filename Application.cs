using System;

namespace ABSA_Assignment
{
    class Application
    {

        public Task1 task1 { get; set; }
        public Task2 task2 { get; set; }
        public Report report { get; set; }
        public Driver util { get; set; }
        public REST rest { get; set; }

        public Application()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Reports\" + DateTime.Now.ToString("HH-mm dd-MM-yyyy") + @"\";

            util = new Driver(path);
            report = new Report(path);
            rest = new REST();

            task1 = new Task1(rest, report);//this feels wrong af
            task2 = new Task2(util,report);
        }

    }
}
