using System;
using System.Diagnostics;
using ServiceStack.Text;

namespace SelfHost1
{
    class Program
    {
        static void Main(string[] args)
        {
            new AppHost().Init().Start("http://*:8088/");
            "ServiceStack SelfHost listening at http://localhost:8088 ".Print();
            Process.Start("http://localhost:8088/");

            Console.ReadLine();
        }
    }
}
