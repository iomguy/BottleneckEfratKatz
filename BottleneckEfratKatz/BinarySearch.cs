using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinSearch
{
    class BinS
    {
        //static void Main(string[] args)
        //{
        //    Func<int, bool> LoopDelegate = Loop;

        //    List<int> a = new List<int>() { 1, 3, 5, 7, 9 };
        //    Console.WriteLine("Ищем  6: {0}", BinarySearch(a, LoopDelegate));
        //    Console.ReadLine();
        //}
        public static Func<double, bool> LoopDelegate = Loop;

        private static bool Loop(double y)
        {
            bool answer = (0.02 <= y) ? true : false;
            return answer;
        }

        //delegate bool LoopDelegate();

        /// <summary>
        /// Бинарный поиск в отсортированном массиве.
        /// </summary>
        /// <param name="a">Отсортированный по возрастанию массив типа int[]</param>
        /// <param name="x">Искомый элемент.</param>
        /// <returns>Возвращает индекс искомого элемента либо null, если элемент не найден.</returns>
        public static int? BinarySearch(List<double> a, Func<double, bool> Loop)
        {
            // Проверить, имеет ли смыл вообще выполнять поиск:
            // - если длина массива равна нулю - искать нечего;
            // - если искомый элемент меньше первого элемента массива, значит, его в массиве нет;
            // - если искомый элемент больше последнего элемента массива, значит, его в массиве нет.
            // if ((a.Count == 0) || (x < a[0]) || (x > a[a.Count - 1]))
            if (a.Count == 0)
                return null;

            // Приступить к поиску.
            // Номер первого элемента в массиве.
            int first = 0;
            // Номер элемента массива, СЛЕДУЮЩЕГО за последним
            int last = a.Count;

            // Если просматриваемый участок не пуст, first < last
            while (first < last)
            {
                int mid = first + (last - first) / 2;

                //if (x <= a[mid])
                if (Loop(a[mid]))
                    last = mid;
                else
                    first = mid + 1;
            }

            // Теперь last может указывать на искомый элемент массива.
            //if (a[last] == x)
            if (last == first) /// выдаст наименьший элемент списка, который больше или равен введённого числа
                return last;
            else
                return null;
        }
    }
}
