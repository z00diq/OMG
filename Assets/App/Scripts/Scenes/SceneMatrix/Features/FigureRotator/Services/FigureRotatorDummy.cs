using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services
{
    public class FigureRotatorDummy : IFigureRotator
    {
        private delegate void DoRotate(Grid<bool> grid,int first,int last,int i);
        
        public Grid<bool> RotateFigure(Grid<bool> grid, int rotateCount)
        {
            DoRotate currentRotate;
            currentRotate = rotateCount < 0 ? Rotatec—ounterClockwise : RotateClockwise;
            Grid<bool> rotatedSquare = grid;

            for (int i = 0; i < Mathf.Abs(rotateCount); i++)
                rotatedSquare = RotateOneTime(rotatedSquare, currentRotate);

            return rotatedSquare;
        }

        private Grid<bool> RotateOneTime(Grid<bool> grid, DoRotate currentRotate)
        {
            Vector2Int squareSize = grid.Size;
            if (squareSize.x < squareSize.y)
                squareSize.x++;
            else if (squareSize.y < squareSize.x)
                squareSize.y++;

            var rotatedSquare = new Grid<bool>(squareSize);
            rotatedSquare.Clone(grid);

            int n = rotatedSquare.Size.x;


            for (int period = 0; period < n / 2; period++)
            {
                int first = period;
                int last = n - 1 - period;

                for (int i = first; i < last; i++)
                {
                    currentRotate(rotatedSquare, first, last, i);
                }
            }

            if(grid.Size!=rotatedSquare.Size)
                rotatedSquare.Resize();

            return rotatedSquare;
        }

        private void RotateClockwise(Grid<bool> rotated, int first, int last, int i)
        {
            bool temp = rotated[first + i, first];

            rotated[first + i, first] = rotated[last, first + i];
            rotated[last, first + i] = rotated[last - i, last];
            rotated[last - i, last] = rotated[first, last - i];
            rotated[first, last - i] = temp;
        }

        private void Rotatec—ounterClockwise(Grid<bool> rotated, int first, int last, int i)
        {
            bool temp = rotated[first + i, first];

            rotated[first + i, first] = rotated[first, last - i];
            rotated[first, last - i] = rotated[last - i, last];
            rotated[last - i, last] = rotated[last, first + i];
            rotated[last, first + i] = temp;
        }
    }
}