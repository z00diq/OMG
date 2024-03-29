using System.Collections.Generic;

namespace App.Scripts.Extensions
{
    public static class Extensions
    {
        public static bool IsRowEmpty<T>(this List<List<T>> matrix,int index)
        {
            T defaultValue = default;
            bool isEmpty = true;

            foreach (var item in matrix[index])
            {
                if (defaultValue.Equals(item) == false)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }

        public static bool IsColumnEmpty<T>(this List<List<T>> matrix,int index)
        {
            T defaultValue = default;
            bool isEmpty = true;

            foreach (var row in matrix)
            {
                if (defaultValue.Equals(row[index]) == false)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }

        public static void CreateFromArray<T>(this List<List<T>> matrix, T[][] array)
        {
            foreach (var item in array)
            {
                List<T> changingRow = new List<T>();
                changingRow.AddRange(item);
                matrix.Add(changingRow);
            }
        }
    }
}
