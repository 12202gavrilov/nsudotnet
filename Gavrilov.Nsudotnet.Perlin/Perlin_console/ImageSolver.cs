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

        public ImageSolver(int width, int height/*list of structs: [number of grids, placer instance, interpolator instance]*/,string filename)
        {
            gridList = new List<Grid3C>();

            int i = 0;
            int frequency = 2;// Math.Min(width, height); это период

            while (i < 8)
            {

                gridList.Add(new Grid3C(width,height,frequency));

                frequency *= 2;
                
                i++;

            }

            knotPlacer = new RandomPlacer(190);
            interpolator = new BiqubicInterpolator();

            noiseMap = new Bitmap(width, height);

            noizeFilename = filename;
        }

        public void Hell()
        {
            Color color;
            //float noise = 0;

            Color noiseColor;

            foreach (Grid3C element in gridList)
            {
                element.GenerateGrid(knotPlacer, interpolator);
            }

            for(int y = 0; y < noiseMap.Height; y++)
            {
                for (int x = 0; x < noiseMap.Width; x++)
                {
                    //color = 1;
                    //noise = 255;

                    noiseColor = Color.FromArgb(255, 255, 255);

                    foreach (Grid3C element in gridList)
                    {
                        color = element.GetColor(x, y);

                        noiseColor = Color.FromArgb((int)color.R * noiseColor.R / 255, (int)color.G * noiseColor.G / 255, (int)color.B * noiseColor.B / 255);

                        //noise *= color / 255;
                    }

                    noiseMap.SetPixel(x, y, noiseColor);
                }
            }

            noiseMap.Save(noizeFilename, System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}
