using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Perlin_console
{
    public class Grid3C
    {
        private Grid rColor;
        private Grid gColor;
        private Grid bColor;

        private List<Grid> grid3List;

        public Grid3C(int imageWidth, int imageHeight, int gridFrequency)
        {
            rColor = new Grid(imageWidth, imageHeight, gridFrequency);
            gColor = new Grid(imageWidth, imageHeight, gridFrequency);
            bColor = new Grid(imageWidth, imageHeight, gridFrequency);

            grid3List = new List<Grid>();

            grid3List.Add(rColor);
            grid3List.Add(gColor);
            grid3List.Add(bColor);
        }

        public Color GetColor(int x, int y)
        {
            return Color.FromArgb((int)rColor.GetColor(x,y),(int)gColor.GetColor(x,y),(int)bColor.GetColor(x,y));
        }

        public void GenerateGrid(IKnotValuePlacer placer, IGridInterpolator interpolator)
        {
            foreach (Grid element in grid3List)
            {
                element.GenerateGrid(placer, interpolator);
            }

            return;
        }
    }
}
