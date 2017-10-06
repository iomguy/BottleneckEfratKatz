using System;
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
            _inpA = Readers.ReadPersDiag(_filenameA); //выдели в отдельные классы
            _inpB = Readers.ReadPersDiag(_filenameB); //выдели в отдельные классы
            AcupB = PersDiagram.PersDiagramCup(_inpA, _inpB);
            BcupA = PersDiagram.PersDiagramCup(_inpB, _inpA);
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