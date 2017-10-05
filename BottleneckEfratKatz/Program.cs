using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using BinSearch;

namespace BottleneckEfratKatz
{
    public class Dot 
        //координата точки и кратность
    {
        public Dot(double birth, double death, int mult = 1) ///конструктор
        {
            BirthTime = birth;
            DeathTime = death;
            SourceMult = mult;
        }

        private double _birthTime;
        private double _deathTime;
        private int _sourceMult; ///исходная кратность
        private int _currMult;  ///текущая кратность

        public int SourceMult
        {
            get { return _sourceMult; } ///читаем исходную кратность
            set { _sourceMult = value; CurrMult = value; } ///задаём исходную и => текущую кратность
        }

        public double BirthTime { get => _birthTime; set => _birthTime = value; }
        public double DeathTime { get => _deathTime; set => _deathTime = value; }
        public int    CurrMult  { get => _currMult;  set => _currMult  = value; }

        public static double Distance(Dot parent, Dot inherior)
        {
            try
            {
                double result = Math.Sqrt(Math.Pow((parent.BirthTime - inherior.BirthTime), 2) + Math.Pow((parent.DeathTime - inherior.DeathTime), 2));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem with distance", e);
                throw;
            }

        }
    }

    public class PersDiagram 
        //список из точек персистентной диаграммы с кратностями
        // отдельный тип создаётся, т.к. все операции добавления и вычитания элементов должны вестись с обновлением кратностей
    {
        public PersDiagram() ///конструктор
        {
            DotList = new List<Dot>(); //тут стоит использовать массив
        }

        private List<Dot> _dotList; /// список точек с кратностями
        public List<Dot> DotList { get => _dotList;       set => _dotList = value; }

        public int SourceSize() 
            //считает размер с учётом исходных кратностей персистентной диаграммы
        {
            int i = 0;
            foreach (Dot dot in DotList)
            {
                i += dot.SourceMult;
            }
            return i;
        }

        public int indexOfBirthDeath(double birth, double death) 
            //ищет индекс по координатам birth, death
        {
            int index = DotList.FindIndex(dot => dot.BirthTime == birth && dot.DeathTime == death); ///возвращает -1, если элемент не найден
            return index;
        }

        public void AddDot(Dot dot) 
            //добавляет точку с некоторой кратностью с учётом того, что такая точка уже может быть
        {
            int repeatedString = indexOfBirthDeath(dot.BirthTime, dot.DeathTime);
            if (repeatedString == -1)        ///если такая строка ещё не попадалась
            {
                DotList.Add(dot);            /// доабавляем новую точку в перс диаграмму
            }
            else
                DotList[repeatedString].SourceMult += dot.SourceMult; ///точку не добавляем, увеличиваем кратность, если попадалась
        }
                
        ///нужно создать метод, который будет выдавать элементы с ненулевыми кратностями
        ///
    }

    public class Arc
       //дуга из двух точек
    {
        public Arc(Dot dotParent, Dot dorInherior)
        {
            Parent    = dotParent;
            Inherior  = dorInherior;
            Distance = Dot.Distance(Parent, Inherior);
        }

        private Dot _parent;
        private Dot _inherior;
        private double _distance;

        public Dot Parent   { get => _parent;   set => _parent   = value; }
        public Dot Inherior { get => _inherior; set => _inherior = value; }
        public double Distance { get => _distance; set => _distance = value; }
    }

    public class Graph
    {
        public Graph() ///конструктор
        {
            ///тут построить _arcsDict
            ///
            ArcsList = new List<Arc>(); //тут стоит использовать массив
            DistI    = new List<double>();
        }

        private List<Arc> _arcsList;
        private List<double> _distI;

        public List<Arc> ArcsList { get => _arcsList; set => _arcsList = value; }
        public List<double> DistI { get => _distI;    set => _distI    = value; }

        ///тут хранятся расстояния в графе G по возрастанию и без повторений

        public void BuildAllDistGraph(PersDiagram A, PersDiagram B) 
            //строит словарь дуг и расстояний для всех точек A со всеми точками B (по возрастанию)
        {
            foreach (Dot dotA in A.DotList)
            {
                foreach (Dot dotB in B.DotList)
                {
                    Arc arcAB = new Arc(dotA, dotB);

                    ArcsList.Add(arcAB);
                    if (!(DistI.Contains(arcAB.Distance))) ///добавляем расстояние, если такого ещё не было
                        DistI.Add(arcAB.Distance);
                }
            }
            DistI.Sort();
        }

        public void BuildGraphGdistI(Graph G, int i)
            //обновляет граф G[dist(i)], фильтруя самый большой граф G в зависимости от значения i
        {
            //ArcsList = G.ArcsList.Where(x => (x.Value <= G.DistI[i+1])).ToDictionary(x => x.Key, x => x.Value);
            ArcsList = G.ArcsList.Where(x => (x.Distance <= G.DistI[i - 1])).ToList();
        }
    }

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
            graphGdistI.BuildGraphGdistI(graphG, 10); //возможно, нужно будет унаследовать отдельный тип для graphGdistI и хранить при нём значение i
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
                Dot projectedDot = new Dot(dot.BirthTime, dot.DeathTime, dot.SourceMult); ///копируем первую персистентную диаграмму
                persDiagramResult.AddDot(projectedDot);
            }

            foreach (Dot dot in persDiagramB.DotList)
            {
                var diagCoord = (dot.BirthTime + dot.DeathTime) / 2; ///координата проекции на диагональ
                Dot projectedDot = new Dot(diagCoord, diagCoord, dot.SourceMult);
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

                        Dot addedDot = new Dot(birth, death); ///создаём точку с единичной кратностью
                        persD.AddDot(addedDot);              ///добавляем её в перс диаграмму, с учётом повторений меняется кратность
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