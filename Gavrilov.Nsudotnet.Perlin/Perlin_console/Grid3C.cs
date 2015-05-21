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
            int i = 0;

            Task[] tasks = new Task[grid3List.Count()];

            Action<object> gridGeneration = (object elem) => 
                {
                    /*
                     * Каждый поток использует свой интерполятор; т.к. значения, используемые в интерполяции
                     * специфичны для каждого ее вида и, более того, нуждаются в обнулении, а сама
                     * операция интерполирования - причина распараллеливания, то пользоваться lock-ом
                     * в этом случае не вариант.
                     * 
                     * Совсем другой случай - рандомизатор; при попытке провернуть то же самое с ним,
                     * программа выдает черно-белое изображение шума - рандом успевает инициализироваться во всех
                     * потоках одинаково, - поэтому на него приходится ставить lock в классе плэйсера.
                     */
                    IGridInterpolator localIGrid;

                    lock (interpolator)
                    {
                        localIGrid = (IGridInterpolator)Activator.CreateInstance(interpolator.GetType());
                    }

                    Grid gridFragment = (Grid)elem;
                    gridFragment.GenerateGrid(placer, localIGrid);
                };

            foreach (Grid element in grid3List)
            {
                //element.GenerateGrid(placer, interpolator);

                tasks[i] = new Task(gridGeneration,element);
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

            return;
        }
    }
}
