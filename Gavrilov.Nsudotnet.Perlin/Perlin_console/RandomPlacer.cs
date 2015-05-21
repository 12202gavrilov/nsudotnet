using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlin_console
{
    class RandomPlacer : IKnotValuePlacer
    {
        Random Randomizer;

        int lowerAmplitude;

        public RandomPlacer(int limit)
        {
            Randomizer = new Random();

            lowerAmplitude = limit;
        }

        public int GetRandomLimit()
        {
            return lowerAmplitude;
        }

        public void Emplace(float[,] grid, int actualWidth, int widthOffset, int actualHeight, int heightOffset, int frequency)
        {
            //int col = 0;
            for (int y = 0; y <= actualHeight + 2 * heightOffset; y += actualHeight/frequency)
            {
                for (int x = 0; x <= actualWidth + 2 * widthOffset; x += actualWidth/frequency)
                {
                    //grid[x, y] = col;
                    grid[x, y] = Randomizer.Next(lowerAmplitude, 255);

                    //col += 15;

                    //System.Console.Write(grid[x,y]);
                    //System.Console.Write(" ");
                }

                //System.Console.WriteLine(" ");
            }

            return;
        }
    }
}
