using System;
using System.IO;

namespace VectorLength
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Программа для вычисления длины вектора");
            Console.WriteLine("Введите имя файла:");
            string filename = Console.ReadLine();

            try
            {
                string[] lines = File.ReadAllLines(filename);

                if (lines.Length == 0)
                {
                    Console.WriteLine("Файл пустой!");
                    return;
                }
                int n = int.Parse(lines[0]);
                Console.WriteLine($"Размерность: {n}");
                double[,] g = new double[n, n];
                for (int i = 0; i < n; i++)
                {
                    string[] numbers = lines[i + 1].Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        g[i, j] = double.Parse(numbers[j]);
                    }
                }
                bool symmetric = true;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (Math.Abs(g[i, j] - g[j, i]) > 0.0001)
                        {
                            symmetric = false;
                            break;
                        }
                    }
                    if (!symmetric) break;
                }
                if (!symmetric)
                {
                    Console.WriteLine("Ошибка: матрица не симметричная!");
                    return;
                }
                double[] v = new double[n];
                string[] vectorNumbers = lines[n + 1].Split(' ');
                for (int i = 0; i < n; i++)
                {
                    v[i] = double.Parse(vectorNumbers[i]);
                }
                double length = CalculateLength(g, v, n);
                Console.WriteLine($"Длина вектора: {length}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            Console.ReadKey();
        }
        static double CalculateLength(double[,] g, double[] v, int n)
        {
            double[] temp = new double[n];
            for (int i = 0; i < n; i++)
            {
                temp[i] = 0;
                for (int j = 0; j < n; j++)
                {
                    temp[i] += g[i, j] * v[j];
                }
            }
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                result += v[i] * temp[i];
            }
            return Math.Sqrt(result);
        }
    }
}