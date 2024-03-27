using App.Scripts.Modules.Grid;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{
    public interface IFigureParser
    {
        /// <summary>
        /// парсит предложенный текст в грид с фигурой
        /// если формат файла неверный то кидается исключение ExceptionParseFigure
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Grid<bool> ParseFile(string text);
    }
}