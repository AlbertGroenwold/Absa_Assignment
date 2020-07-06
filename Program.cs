using System;
using System.Threading.Tasks;

namespace ABSA_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var report = Task1.run();

            report = Task2.run();

            report.Flush();
        }
    }
}
