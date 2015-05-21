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
            
            Console.WriteLine("Hello Perlin");

            int size = 0;
            int limit = 0;
            double brightness = 0.0;

            try
            {
                size = Convert.ToInt32(args[0]);
                limit = Convert.ToInt32(args[1]);
                brightness = Convert.ToDouble(args[2]);
            }
            catch (Exception se)
            {
                Console.WriteLine("Something's wrong with your parameters.");
                Console.WriteLine("Usage: size, frequency limit, grid brightness level [0,1], filename");
                Console.ReadKey();
                
                return;
            }

            ImageSolver a = new ImageSolver(size, limit, brightness, args[3]);

            a.Hell();

            Console.WriteLine("Your image is generated. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
