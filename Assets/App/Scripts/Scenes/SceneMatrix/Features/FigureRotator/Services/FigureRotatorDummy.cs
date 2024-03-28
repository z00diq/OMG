using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services
{
    public class FigureRotatorDummy : IFigureRotator
    {
        public Grid<bool> RotateFigure(Grid<bool> grid, int rotateCount)
        {
            Vector2Int squareSize = grid.Size;
            var rotated = grid.Clone();

            if (squareSize.x < squareSize.y)
                squareSize.x++;
            else if(squareSize.y < squareSize.x)
                squareSize.y++;

            rotated.UpdateMatrix(squareSize);

            int n = rotated.Size.x;

            for (int period = 0; period < n/2; period++)
            {
                int first = period;
                int last = n - 1 - period;

                for (int i = first; i < last; i++)
                {
                    //против часовой
                    bool temp = rotated[first + i, first];

                    rotated[first + i, first] = rotated[first, last - i];
                    rotated[first, last - i] = rotated[last - i, last];
                    rotated[last - i, last] = rotated[last, first + i];
                    rotated[last, first + i] = temp;


                    //по часовой
                    //bool temp = rotated[first + i, first];

                    //rotated[first + i, first] = rotated[last, first+i];
                    //rotated[last, first + i] = rotated[last-i, last];
                    //rotated[last - i, last] = rotated[first, last - i];
                    //rotated[first, last - i] = temp;
                }
            }

            Vector2Int newSize = new Vector2Int(grid.Size.y, grid.Size.x);
            rotated.UpdateMatrix(newSize);

            return rotated;
        }
    }
}