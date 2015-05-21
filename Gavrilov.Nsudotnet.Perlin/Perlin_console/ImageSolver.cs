using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Perlin_console
{
    class ImageSolver
    {
        private List<Grid3C> gridList;

        private System.Drawing.Bitmap noiseMap;

        private RandomPlacer knotPlacer;

        private BiqubicInterpolator interpolator;

        private string noizeFilename;

        public ImageSolver(int size, int frequencyLimit, double randomLimit, string filename)
        {
            int width = 0;
            int height = 0;

            if (!(size > 0 && (size & (size - 1)) == 0))
            {
                width = (int)Math.Pow(2, Math.Truncate(Math.Log(size, 2)));
            }
            else
            {
                width = size;
            }

            height = size;

            if (frequencyLimit < 2)
            {
                frequencyLimit = 2;
            }
            else if (frequencyLimit > Math.Log(Math.Min(width, height), 2))
            {
                frequencyLimit = (int)Math.Log(Math.Min(width, height), 2);
            }

            gridList = new List<Grid3C>();

            int i = 0;
            int frequency = 2;

            while (i < frequencyLimit)
            {

                gridList.Add(new Grid3C(width,height,frequency));

                frequency *= 2;
                
                i++;

            }

            knotPlacer = new RandomPlacer(50 + (int)Math.Round(randomLimit * 200));
            interpolator = new BiqubicInterpolator();

            noiseMap = new Bitmap(width, height);

            noizeFilename = filename;
        }

        public void Hell()
        {
            Color color;
            //float noise = 0;

            Color noiseColor;

            int i = 0;

            Task[] tasks = new Task[gridList.Count()];

            Action<object> gridGeneration = (object elem) =>
            {
                //IGridInterpolator localIGrid = (IGridInterpolator)Activator.CreateInstance(interpolator.GetType());

                Grid3C gridFragment = (Grid3C)elem;
                gridFragment.GenerateGrid(knotPlacer, interpolator);
            };

            foreach (Grid3C element in gridList)
            {
                //element.GenerateGrid(knotPlacer, interpolator);

                tasks[i] = new Task(gridGeneration, element);
                tasks[i].Start();
                i++;
            }

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Grid layer did not finish generation: {0}", e.InnerException.ToString());
                return;
            }

            for(int y = 0; y < noiseMap.Height; y++)
            {
                for (int x = 0; x < noiseMap.Width; x++)
                {
                    noiseColor = Color.FromArgb(255, 255, 255);

                    foreach (Grid3C element in gridList)
                    {
                        color = element.GetColor(x, y);

                        noiseColor = Color.FromArgb((int)color.R * noiseColor.R / 255, (int)color.G * noiseColor.G / 255, (int)color.B * noiseColor.B / 255);
                    }

                    noiseMap.SetPixel(x, y, noiseColor);
                }
            }

            noiseMap.Save(noizeFilename, System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}
