using App.Scripts.Modules.Grid;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services
{

    public interface IFigureRotator
    {
        /// <summary>
        /// создает новую матрицу повернуют на 90 * rotateCount. Если rotateCount положительное - то по часовой
        /// если отрицательное - то против часовой
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rotateCount"></param>
        /// <returns></returns>
        Grid<bool> RotateFigure(Grid<bool> grid, int rotateCount);
    }
}