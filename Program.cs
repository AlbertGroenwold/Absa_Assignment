using System;
using System.Threading.Tasks;

namespace ABSA_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var reportTask1 = Task1.run();

            reportTask1.Flush();

            var reportTask2 = Task2.run();

            reportTask2.Flush();
        }
    }
}
