using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlin_console
{
    class BiqubicInterpolator : IGridInterpolator
    {
        private float[,] coeficientMatrix;
        float[] gamma;
        float[] beta;

        public BiqubicInterpolator()
        {
            gamma = new float[16];
            beta = new float[16];

            coeficientMatrix = new float[,] { {0, 0, 0, 0, 0, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                              {0, -12, 0, 0, 0, -18, 0, 0, 0, 36, 0, 0, 0, -6, 0, 0},
                                              {0, 18, 0, 0, 0, -36, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0},
                                              {0, -6, 0, 0, 0, 18, 0, 0, 0, -18, 0, 0, 0, 6, 0, 0},
                                              {0, 0, 0, 0, -12, -18, 36, -6, 0, 0, 0, 0, 0, 0, 0, 0},
                                              {4, 6, -12, 2, 6, 9, -18, 3, -12, -18, 36, -6, 2, 3, -6, 1},
                                              {-6, -9, 18, -3, 12, 18, -36, 6, -6, -9, 18, -3, 0, 0, 0, 0},
                                              {2, 3, -6, 1, -6, -9, 18, -3, 6, 9, -18, 3, -2, -3, 6, -1},
                                              {0, 0, 0, 0, 18, -36, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                              {-6, 12, -6, 0, -9, 18, -9, 0, 18, -36, 18, 0, -3, 6, -3, 0},
                                              {9, -18, 9, 0, -18, 36, -18, 0, 9, -18, 9, 0, 0, 0, 0, 0},
                                              {-3, 6, -3, 0, 9, -18, 9, 0, -9, 18, -9, 0, 3, -6, 3, 0},
                                              {0, 0, 0, 0, -6, 18, -18, 6, 0, 0, 0, 0, 0, 0, 0, 0},
                                              {2, -6, 6, -2, 3, -9, 9, -3, -6, 18, -18, 6, 1, -3, 3, -1},
                                              {-3, 9, -9, 3, 6, -18, 18, -6, -3, 9, -9, 3, 0, 0, 0, 0},
                                              {1, -3, 3, -1, -3, 9, -9, 3, 3, -9, 9, -3, -1, 3, -3, 1}  
                                            };
        }

        private void NewGamma(int gammaX, int deltaX, int gammaY, int deltaY, float[,] grid)
        {
            int i = 0;
            int gx = gammaX * deltaX;
            int gy = gammaY * deltaY;

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    gamma[i] = grid[gx,gy];
                    gx += deltaX;
                    i++;
                }

                gx = gammaX * deltaX;
                gy += deltaY;
            }

            for (int x = 0; x < 16; x++)
            {
                beta[x] = 0;

                for (int y = 0; y < 16; y++)
                {
                    beta[x] += coeficientMatrix[x, y] * gamma[y] / 36;
                }
            }

            return;
        }

        public void Interpolate(float[,] grid, int actualWidth, int widthOffset, int actualHeight, int heightOffset, int frequency)
        {
            int gammaX = 0;
            int gammaY = 0;
            
            float relevanceX = 0;
            float relevanceY = 0;

            NewGamma(gammaX, widthOffset, gammaY, heightOffset, grid);

            for (int y = heightOffset; y < actualHeight + heightOffset; y += 1)
            {
                if (y == (actualHeight / frequency) * (gammaY + 2))
                {
                    gammaY += 1;
                    //NewGamma(gammaX, widthOffset, gammaY, heightOffset, grid);
                }
                NewGamma(gammaX, widthOffset, gammaY, heightOffset, grid);

                for (int x = widthOffset; x < actualWidth + widthOffset; x += 1)
                {
                    if(x == (actualWidth/frequency)*(gammaX + 2))
                    {
                        gammaX += 1;
                        NewGamma(gammaX, widthOffset, gammaY, heightOffset, grid);
                    }

                    relevanceX = (x / (float)(actualWidth / frequency) - (gammaX + 1));
                    relevanceY = (y / (float)(actualHeight / frequency) - (gammaY + 1));

                    //relevanceX = ((float)(x + 1) / (float)(actualWidth));
                    //relevanceY = ((float)(y + 1) / (float)(actualHeight));

                    if (relevanceX == 0 && relevanceY == 0)
                    {
                        continue;
                    }

                    int step = 0;

                    grid[x, y] = 0;

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            grid[x, y] += beta[step] * (float)Math.Pow(relevanceX,i) * (float)Math.Pow(relevanceY,j);
                            step++;

                        }
                    }
                }

                gammaX = 0;
            }

            //float test = grid[325, 325];

            return;
        }
    }
}
