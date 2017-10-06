using System;
using System.IO;

namespace BottleneckEfratKatz
{

    public partial class Dist
    {
        ///граф G[dist(i)] вершин, расстояния которых не превышают i-е расстояние из всех возможных

        //public bool rLessOrEqual(int i);
        //    //возвращает true, если ответ меньше или равен текущему состоянию

        public class Readers
        {
            public static string ReadLine(StreamReader reader)
            //читаем строку, обрезаем по краям
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!String.IsNullOrWhiteSpace(line))
                        return line.Trim();
                }
                return null;
            }

            public PersDiagram ReadPersDiag(string _filename)
            //читаем из файла, заносим повторяющиеся строки в кратности
            {
                try
                {
                    PersDiagram persD = new PersDiagram();
                    using (StreamReader reader = new StreamReader(_filename))
                    {
                        var line = ReadLine(reader);
                        while (line != null)
                        {
                            var data = line.Split(_separators, 2, StringSplitOptions.RemoveEmptyEntries);
                            var birth = double.Parse(data[0]);
                            var death = double.Parse(data[1]);

                            if (birth != death) ///сейчас учитываем только недиагональные точки
                            {
                                Dot addedDot = new Dot(birth, death); ///создаём точку с единичной кратностью
                                persD.AddDot(addedDot);              ///добавляем её в перс диаграмму, с учётом повторений меняется кратность                            
                            }
                            line = ReadLine(reader);
                        }
                        return persD;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot parse a file", e);
                    throw;
                }

            }
        }
        

    }
}