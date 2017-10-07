using System;
using System.Reflection;
using BinSearch;
using System.Collections.Generic;
using System.Linq;

namespace BottleneckEfratKatz
{
    public class Dist
    {
        public Dist(string filenameA, string filenameB) ///конструктор
        {
            _filenameA = filenameA;
            _filenameB = filenameB;

            _inpA = Readers.ReadPersDiag(_filenameA); //выдели в отдельные классы
            _inpB = Readers.ReadPersDiag(_filenameB); //выдели в отдельные классы

            AcupB = PersDiagram.PersDiagramCup(_inpA, _inpB);
            BcupA = PersDiagram.PersDiagramCup(_inpB, _inpA);                     

            _inpAsize = _inpA.SourceSize();
            _inpBsize = _inpB.SourceSize();
            _inpAcupBsize = AcupB.SourceSize();
            _inpBcupAsize = BcupA.SourceSize();

            AcupB.BuildDictIndex(); ///соответствие между точками и индексами
            BcupA.BuildDictIndex(_inpAcupBsize + 1);

            graphG = new BipartiteGraph();
            graphG.BuildAllDistGraph(AcupB, BcupA); ///строим граф связей и размеров из всез точек AcupB во все точки BcupA
            graphGdistI = new BipartiteGraph();
            graphGdistI.BuildGraphGdistI(graphG, 3); //возможно, нужно будет унаследовать отдельный тип для graphGdistI и хранить при нём значение i
            int? index = BinS.BinarySearch(graphG.DistI, BinS.LoopDelegate); //ищем в списке возможных дистанций ту, которая удовлетворяет условию LoopDelegate

        }             

        private readonly string _filenameA;
        private readonly string _filenameB;

        public int _inpAsize; ///размеры для контроля
        public int _inpBsize;
        public int _inpAcupBsize;
        public int _inpBcupAsize;

        PersDiagram _inpA;
        PersDiagram _inpB;
        public PersDiagram AcupB;
        public PersDiagram BcupA;

        public BipartiteGraph graphG; ///граф G всех вершин со всеми + расстояния
        public BipartiteGraph graphGdistI; ///граф G[dist(i)] вершин, расстояния которых не превышают i-е расстояние из всех возможных

        //public bool rLessOrEqual(int i);
        //    //возвращает true, если ответ меньше или равен текущему состоянию              
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Dist PersDiagrs = new Dist(args[0], args[1]);

            //var lefts = new HashSet<int> { 1, 2, 3, 4, 5 };
            //var rights = new HashSet<int> { 6, 7, 8, 9, 10 };

            var lefts  = PersDiagrs.AcupB.FullSetOfIndex;
            var rights = PersDiagrs.BcupA.FullSetOfIndex;


            var edges = new Dictionary<int, HashSet<int>>
            {
                [1] = new HashSet<int> { 163367, 163368 },
                [2] = new HashSet<int> { 163379, 163378 },
                [3] = new HashSet<int> { 163390, 163451 },
                [4] = new HashSet<int> { 163561, 164555 },
                [5] = new HashSet<int> { 164557, 164556 }
            };

            var matches = HopcroftKarp.HopcroftKarpFunction(lefts, rights, edges);

            Console.WriteLine($"# of matches: {matches.Count}\n");

            foreach (var match in matches)
            {
                Console.WriteLine($"Match: {match.Key} -> {match.Value}");
            }
        }
    }
}