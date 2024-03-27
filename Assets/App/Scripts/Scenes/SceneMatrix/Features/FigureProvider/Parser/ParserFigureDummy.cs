using App.Scripts.Modules.Grid;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{
    public class ParserFigureDummy : IFigureParser
    {
        public Grid<bool> ParseFile(string text)
        {
            string[] rawData = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            rawData = new string[]
            {"3","3","lihjbgt 5 6 7 ;fogn,fkln 1 0"};

            IsDataCorrectFormat(rawData);
            if (IsDataCorrectFormat(rawData) == false)
                return null;

            var grid = new Grid<bool>(new Vector2Int(1, 1));
            string[] cells = rawData[2].Split(' ');

            foreach (var cell in cells)
            {
                int cellValue = int.Parse(cell);
                grid[cellValue % 1, cellValue / 1] = true;
            }
            
            return grid;            
        }

        private bool IsDataCorrectFormat(string[] data)
        {
            if (data.Length != 3)
                throw new ExceptionParseFigure("Файл имеет неправильный формат");

            if (int.TryParse(data[0], out int width) == false ||
                int.TryParse(data[1], out int heght) == false)
                throw new ExceptionParseFigure("Неверный тип данных для ширины и(или) высоты в исходном файле");

            if (Regex.IsMatch(data[2], @"(\d+\s?)+") == false)
                throw new ExceptionParseFigure("Данные о заполеннх клетках имеют неверный формат");

            return true;
        }
    }
}