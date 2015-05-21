using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlin_console
{
    public interface IKnotValuePlacer
    {
        void Emplace(float[,] grid, int actualWidth, int widthOffset, int actualHeight, int heightOffset, int frequency);
        int GetRandomLimit();
    }
}
