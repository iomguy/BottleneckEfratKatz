using System.Collections.Generic;
using System.Linq;
using System;

namespace BottleneckEfratKatz
{
    public class BipartiteGraph : Graph
    {
        public BipartiteGraph() ///конструктор
            :base()
        {

        }

        private PersDiagram _left; ///вершины левого слоя двудольного графа
        private PersDiagram _right; ///вершины правого слоя двудольного графа

        public PersDiagram Left     { get => _left;            set => _left            = value; }
        public PersDiagram Right    { get => _right;           set => _right           = value; }

        public void BuildAllDistGraph(PersDiagram A, PersDiagram B)
        //строит словарь дуг и список расстояний для всех точек A со всеми точками B (по возрастанию)
        {
            foreach (Dot dotA in A.DotList)
            {
                foreach (Dot dotB in B.DotList)
                {
                    if (!(((dotA.ProjectedFrom.Count == 0) && (dotB.ProjectedFrom.Count != 0) && !(dotB.ProjectedFrom.Contains(dotA))) || ///проверка на то, что ребро не является skewed edge (иначе не включаем ребро в граф)
                           ((dotB.ProjectedFrom.Count == 0) && (dotA.ProjectedFrom.Count != 0) && !(dotA.ProjectedFrom.Contains(dotB)))))   ///skewed edge - это ребро от недиагональной точки до диагональной, но не являющейся её проекцией 
                    {
                        Arc arcAB = new Arc(dotA, dotB);

                        ArcsList.Add(arcAB);

                        if (EdgesDict.ContainsKey(dotA))
                            EdgesDict[dotA].Add(dotB);
                        else
                            EdgesDict[dotA] = new HashSet<Dot> { dotB };

                        if (!(DistI.Contains(arcAB.Distance))) ///добавляем расстояние, если такого ещё не было
                            DistI.Add(arcAB.Distance);
                    }
                }
            }

            Left  = A;
            Right = B;

            DistI.Sort();
        }

        public HashSet<Dot> HashSetFilter(Dot dotLeft, HashSet<Dot> SetEdgesFromDotLeft, double r)
            //создаёт для dot1 новый HashSet из точек, расстояние до которых не > r, чтобы затем добавить это всё в EdgesDict
        {
            var result = new HashSet <Dot>(SetEdgesFromDotLeft);
            foreach (Dot dotRight in SetEdgesFromDotLeft)
            {
                if (Dot.Distance(dotLeft,dotRight) > r)
                    result.Remove(dotRight);
            }

            if (result.Count() == 0)
            {
                throw new Exception("У точки из left не осталось рёбер");
            }
                
             /// если у какой-то точки из left не осталось рёбер, то мы точно не найдём идеальное паросочетание, нужно увеличивать расстояние
                                                                   /// граф можно не достраивать
            return result;
        }

        public void BuildGraphGdistI(BipartiteGraph G, int i)
            //обновляет граф G[dist(i)], фильтруя самый большой граф G в зависимости от значения i
        {
            //ArcsList = G.ArcsList.Where(x => (x.Value <= G.DistI[i+1])).ToDictionary(x => x.Key, x => x.Value);
            try
            {
                double r  = G.DistI[i - 1];
                ArcsList  = G.ArcsList.Where(x => (x.Distance <= r)).ToList();
                EdgesDict = G.EdgesDict.ToDictionary(x => x.Key, x => HashSetFilter(x.Key, x.Value, r));
                Left      = G.Left;
                Right     = G.Right;
            }
            catch (Exception)
            {
                throw new Exception("У точки из left не осталось рёбер");
            }          
        }

        public HashSet<int> Edge(int i)
            //будет применяться к GdistI, по индексу точки из left будет выдавать индексы точек из right, до которых есть рёбра через точки
        {
            Dot dotLeft            = Left.DictIndex[i];
            HashSet<Dot> setOfDotsRight = EdgesDict[dotLeft];
            HashSet<int> SetOfIndexRight = new HashSet<int>();

            foreach (Dot dotRight in setOfDotsRight)
            {
                SetOfIndexRight.UnionWith(dotRight.SetOfIndex);
            }                
            return SetOfIndexRight;
        }
    }

}