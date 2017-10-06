using System.Collections.Generic;
using System.Linq;

namespace BottleneckEfratKatz
{
    public class Graph
    {
        public Graph() ///конструктор
        {
            ///тут построить _arcsDict
            ///
            ArcsList = new List<Arc>(); //тут стоит использовать массив
            DistI    = new List<double>();
            EdgesDict = new Dictionary<Dot, HashSet<Dot>>();
        }

        private List<Arc> _arcsList;
        private List<double> _distI;
        private Dictionary<Dot, HashSet<Dot>> _edges;

        public List<Arc> ArcsList { get => _arcsList; set => _arcsList = value; }
        public List<double> DistI { get => _distI;    set => _distI    = value; }
        public Dictionary<Dot, HashSet<Dot>> EdgesDict { get => _edges; set => _edges = value; }

        ///тут хранятся расстояния в графе G по возрастанию и без повторений

        public void BuildAllDistGraph(PersDiagram A, PersDiagram B) 
            //строит словарь дуг и список расстояний для всех точек A со всеми точками B (по возрастанию)
        {
            foreach (Dot dotA in A.DotList)
            {
                foreach (Dot dotB in B.DotList)
                {
                    if (!( ((dotA.ProjectedFrom.Count == 0) && (dotB.ProjectedFrom.Count != 0) && !(dotB.ProjectedFrom.Contains(dotA))) || ///проверка на то, что ребро не является skewed edge (иначе не включаем ребро в граф)
                           ((dotB.ProjectedFrom.Count == 0) && (dotA.ProjectedFrom.Count != 0) && !(dotA.ProjectedFrom.Contains(dotB))) ))   ///skewed edge - это ребро от недиагональной точки до диагональной, но не являющейся её проекцией 
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
            DistI.Sort();
        }

        public void BuildGraphGdistI(Graph G, int i)
            //обновляет граф G[dist(i)], фильтруя самый большой граф G в зависимости от значения i
        {
            //ArcsList = G.ArcsList.Where(x => (x.Value <= G.DistI[i+1])).ToDictionary(x => x.Key, x => x.Value);
            ArcsList = G.ArcsList.Where(x => (x.Distance <= G.DistI[i - 1])).ToList();
        }
    }
}