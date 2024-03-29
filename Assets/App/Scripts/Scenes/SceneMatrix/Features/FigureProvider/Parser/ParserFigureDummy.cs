using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using System;
using System.Linq;
using UnityEngine;

public class ParserFigureDummy : IFigureParser
{
    public Grid<bool> ParseFile(string text)
    {
        string[] rawData = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        //rawData = new string[]{"3","2","0 1 2 4" };

        TryParseData(rawData, out int width, out int height, out int[] cells);

        var grid = new Grid<bool>(new Vector2Int(width, height));

        foreach (var cell in cells)
            grid[cell % width, cell / width] = true;

        CheckCreatedFugureFormat(grid, cells);

        return grid;
    }

    private bool TryParseData(string[] data, out int width, out int height, out int[] cells)
    {
        const int maxLenghtArrayWithCorrectData = 3;
        const int widthIndex = 0;
        const int heightIndex = 1;
        const int cellsIndex = 2;

        if (data.Length != maxLenghtArrayWithCorrectData)
            throw new ExceptionParseFigure("Файл имеет неправильный формат");

        if (int.TryParse(data[widthIndex], out width) == false ||
            int.TryParse(data[heightIndex], out height) == false)
            throw new ExceptionParseFigure("Неверный тип данных для ширины и(или) высоты в исходном файле");
       
        cells = CreateNumbersArray(data[cellsIndex]);

        Array.Sort(cells);
        int pointer = 0;

        for (int i = 1; i < cells.Length; i++)
        {
            if (cells[pointer] != cells[i])
                pointer++;
            else
                throw new ExceptionParseFigure("Данные для номеров ячеек содержат повторяющиеся элементы");
        }

        return true;
    }

    private int[] CreateNumbersArray(string data)
    {
        int[] cells;
        string[] cellsString = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        cells = new int[cellsString.Length];
        int index = 0;

        foreach (var cellString in cellsString)
        {
            if (int.TryParse(cellString, out int result) == false)
                throw new ExceptionParseFigure("Неверный тип данных для номера заполненной ячейки");

            cells[index++] = result;
        }

        return cells;
    }

    private bool CheckCreatedFugureFormat(Grid<bool> figure, int[] cellsNumber)
    {
        foreach (int cell in cellsNumber)
        {
            int i = cell / figure.Width;
            int j = cell % figure.Width;
            int iMinusOne = Math.Clamp(i - 1, 0, figure.Height - 1);
            int iPlusOne = Math.Clamp(i + 1, 0,  figure.Height - 1);
            int jMinusOne = Math.Clamp(j - 1, 0, figure.Width - 1);
            int jPlusOne = Math.Clamp(j + 1, 0, figure.Width - 1);

            bool upperNeighboorExist = false;
            bool lowerNeighboorExist = false;
            bool rightNeighboorExist = false;
            bool leftNeighboorExist = false;
            bool isNeghboorsCorrect = false;

            //chck
            if (i != iMinusOne)
                lowerNeighboorExist = figure[j, iMinusOne];

            if(i != iPlusOne)
                upperNeighboorExist = figure[j, iPlusOne];

            if (j != jMinusOne)
                leftNeighboorExist = figure[jMinusOne, i];
            //chck
            if (j != jPlusOne)
                rightNeighboorExist = figure[jPlusOne, i];

            isNeghboorsCorrect = lowerNeighboorExist || upperNeighboorExist || leftNeighboorExist || rightNeighboorExist;

            if(isNeghboorsCorrect==false)
                throw new ExceptionParseFigure("Полученная фигура не соответствует прравилам");
        }

        return true;
    }
}