using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perlin_console;

namespace Perlin_console
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            /*foreach (var v in args)
            {
                //Task.run
            }

            Task.WhenAll(tasks).ContinueWith(*/
            Console.WriteLine("Hello Perlin");

            ImageSolver a = new ImageSolver(512,512, args[0]);

            a.Hell();

            Console.WriteLine("Your image is generated. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
