using System;
using System.IO;
using System.Reflection;
using BinSearch;

namespace BottleneckEfratKatz
{

    public class Dist
    {
        public Dist(string filenameA, string filenameB) ///конструктор
        {
            _filenameA = filenameA;
            _filenameB = filenameB;
            _inpA = ReadPersDiag(_filenameA); //выдели в отдельные классы
            _inpB = ReadPersDiag(_filenameB); //выдели в отдельные классы
            AcupB = PersDiagramCup(_inpA, _inpB);
            BcupA = PersDiagramCup(_inpB, _inpA);
            _inpAsize = _inpA.SourceSize();
            _inpBsize = _inpB.SourceSize();
            _inpAcupBsize = AcupB.SourceSize();
            _inpBcupAsize = BcupA.SourceSize();
            graphG = new Graph();
            graphG.BuildAllDistGraph(AcupB, BcupA); ///строим граф связей и размеров из всез точек AcupB во все точки BcupA
            graphGdistI = new Graph();
            graphGdistI.BuildGraphGdistI(graphG, 3); //возможно, нужно будет унаследовать отдельный тип для graphGdistI и хранить при нём значение i
            int? index = BinS.BinarySearch(graphG.DistI, BinS.LoopDelegate); //ищем в списке возможных дистанций ту, которая удовлетворяет условию LoopDelegate
        }


        private static readonly string[] _separators = { "\t", " ", ";" }; /// + это один или более знаков

        private readonly string _filenameA;
        private readonly string _filenameB;

        public int _inpAsize; ///размеры для контроля
        public int _inpBsize;
        public int _inpAcupBsize;
        public int _inpBcupAsize;

        PersDiagram _inpA;
        PersDiagram _inpB;
        PersDiagram AcupB;
        PersDiagram BcupA;

        public Graph graphG; ///граф G всех вершин со всеми + расстояния
        public Graph graphGdistI; ///граф G[dist(i)] вершин, расстояния которых не превышают i-е расстояние из всех возможных

        //public bool rLessOrEqual(int i);
        //    //возвращает true, если ответ меньше или равен текущему состоянию
        public PersDiagram PersDiagramCup(PersDiagram persDiagramA, PersDiagram persDiagramB)
            //создаёт объединение (с учётом кратностей) множества и и множества проекций
        {
            PersDiagram persDiagramResult = new PersDiagram();
            foreach (Dot dot in persDiagramA.DotList)
            {
                Dot nonProjectedDot = new Dot(dot.BirthTime, dot.DeathTime, dot.SourceMult); ///копируем первую персистентную диаграмму                
                persDiagramResult.AddDot(nonProjectedDot);
            }

            foreach (Dot dot in persDiagramB.DotList)
            {
                var diagCoord = (dot.BirthTime + dot.DeathTime) / 2; ///координата проекции на диагональ
                Dot projectedDot = new Dot(diagCoord, diagCoord, dot.SourceMult);
                projectedDot.ProjectedFrom.Add(dot); ///сохраняем в свойстве projectedDot тот факт, что dot спроецирована на projectedDot
                persDiagramResult.AddDot(projectedDot);
            }
            return persDiagramResult;
        }


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
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Dist PersDiagrs = new Dist(args[0], args[1]);
        }
    }
}