using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Diagnostics;

namespace ABSA_Assignment
{
    class Report
    {

        public string path { get; set; }
        public string test { get; set; }

        private ExtentTest curTest;
        private AventStack.ExtentReports.ExtentReports report;


        public Report(string path)
        {
            this.path = path;
        }

        public void CreateReport()
        {
            var htmlReport = new ExtentHtmlReporter(path);
            report = new AventStack.ExtentReports.ExtentReports();
            report.AttachReporter(htmlReport);
            CreateTest();
            report.Flush();
        }

        public void CreateTest()
        {
            if (report == null)
            {
                CreateReport();
            }
            else
            {
                report.Flush();
                curTest = report.CreateTest(test);
                report.Flush();
            }
        }

        private void CheckTest()
        {
            if (curTest == null || curTest.Model.Name != test) CreateTest();
        }

        public string Pass(string msg)
        {
            CheckTest();
            curTest.Pass(msg);
            return null;
        }

        public string Fail(string msg)
        {
            CheckTest();
            curTest.Fail(msg);
            return null;
        }

        public void EndTest(string msg)
        {
            CheckTest();
            curTest.Pass(msg);
            report.Flush();
        }

        public void OpenReport()
        {
            Process.Start(new ProcessStartInfo(path+@"/index.html") { UseShellExecute = true});
        }
    }
}
