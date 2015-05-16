using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlin_console
{
    public class Grid
    {
        private int frequency;
        private int width;
        private int height;

        private float[,] grid;    //[width,height]

        public Grid(int imageWidth, int imageHeight, int gridFrequency)
        {
            //frequency value control exception

            frequency = gridFrequency;

            width = imageWidth;
            height = imageHeight;

            //сверху и снизу добавлены отступы для осуществления корректной интерполяции на краях
            grid = new float[imageWidth + 2 * (imageWidth / frequency) + 1, imageHeight + 2 * (imageHeight / frequency) + 1];

            return;
        }

        public float GetColor(int x,int y)
        {
            if (grid[x + (width / frequency), y + (height / frequency)] > 255)
            {
                return 255;
            }
            else if (grid[x + (width / frequency), y + (height / frequency)] < 0)
            {
                return 0;
            }

            return grid[x + (width / frequency), y + (height / frequency)];
        }

        private void EmplaceKnotValues(IKnotValuePlacer placer)
        {
            placer.Emplace(grid, width, width / frequency, height, height / frequency, frequency);
            return;
        }

        private void InterpolateGridValues(IGridInterpolator interpolator)
        {
            // относительные значения: [0,1-условно, т.е. единица не достигается]
            int x = 0;
            int y = 0;

            interpolator.Interpolate(grid, width, width / frequency, height, height / frequency, frequency);
            return;
        }

        public void GenerateGrid(IKnotValuePlacer placer, IGridInterpolator interpolator)
        {
            EmplaceKnotValues(placer);

            InterpolateGridValues(interpolator);

            return;
        }
    }
}

/*
 * -1,-1    0,-1    1,-1    2,-1
 * 
 * -1,0     0,0     1,0     2,0
 * 
 * -1,1     0,1     1,1     2,1
 * 
 * -1,2     0,2     1,2     2,2
*/

//следует ли интерполяцию внедрять в Grid-класс в качестве метода, или же вывести его в виде интерфейса, подобно IKnotValuePlacer (проблемно по причине различного количества требуемых точек)?
//IKnotValuePlacer - метод Next(int x,int y) - выдать следующее значение в узле.
//коэфициенты bi считаются на каждом шаге интерполяции (на каждой новой точке) заново, как и x и y.
//frequency - сама частота расположения узлов, или степень двойки - частоты расположения узлов?

//при интерполяции в процессе инкрементации точки x (y - соответственно, тоже), будет регулярно возникать ситуация
// попадания в существующий узел; в этом случае разумно сменять и кластер точек - на единичную величину в соответствующем направлении;
// при ресете позиции x (смене позиции y) ресет делается только по x-направлению (y - в случае описанной выше ситуации)
