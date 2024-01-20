using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class FileAccessor
{
    private const int Padding = 1;
    private int _cols;
    private bool _isInitialized;
    private int[,] _numbers;

    private int _rows;

    /// <summary>
    ///     Creates a 3x3 array which is a slice of array with (x,y) element in the middle
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public int[,] this[int x, int y]
    {
        get
        {
            if (!_isInitialized) return new[,] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };

            //idk i dont want to write a size check

            var window = new int[3, 3];

            for (var i = -1; i < 2; i++)
            for (var j = -1; j < 2; j++)
                window[i + 1, j + 1] = _numbers[Padding + x + i, Padding + y + j];

            return window;
        }
    }

    public (int, int) Size => (_rows, _cols);


    /// <summary>
    ///     Starts a task which reads all values from the file, saves the file in class, does not check the validity
    /// </summary>
    /// <param name="path">Path to a .txt file</param>
    /// <returns> Returns a Task with bool value where the result indicates if file was read successfully or not </returns>
    public Task<bool> Open(string path)
    {
        if (!File.Exists(path)) return Task.FromResult(false);
        return Task.Run(() =>
        {
            try
            {
                // Read numbers from file
                var lines = File.ReadAllLines(path);
                var rows = lines.Length;
                var cols = lines[0].Length;

                var ints = new int[rows, cols];

                for (var i = 0; i < rows; i++)
                for (var j = 0; j < cols; j++)
                    ints[i, j] = int.Parse(lines[i][j].ToString());

                // Create a new padded 2D array with increased dimensions
                var numbers = new int[rows + 2 * Padding, cols + 2 * Padding];

                // Fill the padded array with -1
                for (var i = 0; i < rows + 2 * Padding; i++)
                for (var j = 0; j < cols + 2 * Padding; j++)
                    numbers[i, j] = -1;

                // Copy the original array into the padded array
                for (var i = 0; i < rows; i++)
                for (var j = 0; j < cols; j++)
                    numbers[i + Padding, j + Padding] = ints[i, j];

                _numbers = numbers;
                _cols = cols;
                _rows = rows;

                _isInitialized = true;
            }
            catch (Exception e)
            {
                Debug.Log($"Reading text file failed with the following exception\n{e.Message}");
                return false;
            }

            return true;
        });
    }
}