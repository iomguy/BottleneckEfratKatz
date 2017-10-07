using System.Collections.Generic;
using System.Linq;

namespace BottleneckEfratKatz
{
    public class BipartiteGraphDistI : BipartiteGraph
    {
        public BipartiteGraphDistI(PersDiagram A, PersDiagram B, BipartiteGraph G, int i) ///конструктор
            :base(A, B)
        {
            BuildGraphGdistI(G, i);
        }

        bool _lessOrEqualR; ///true - если искомое расстояние меньше R

        public HashSet<Dot> HashSetFilter(Dot dot1, HashSet<Dot> SetEdgesFromDot1, double r)
        //создаёт для dot1 новый HashSet из точек, расстояние до которых не > r, чтобы затем добавить это всё в EdgesDict
        {
            var result = new HashSet<Dot>(SetEdgesFromDot1);
            foreach (Dot dot2 in SetEdgesFromDot1)
            {
                if (Dot.Distance(dot1, dot2) > r)
                    result.Remove(dot2);
            }
            return result;
        }

        public void BuildGraphGdistI(Graph G, int i)
        //обновляет граф G[dist(i)], фильтруя самый большой граф G в зависимости от значения i
        {
            //ArcsList = G.ArcsList.Where(x => (x.Value <= G.DistI[i+1])).ToDictionary(x => x.Key, x => x.Value);
            double r = G.DistI[i - 1];
            ArcsList = G.ArcsList.Where(x => (x.Distance <= r)).ToList();
            EdgesDict = G.EdgesDict.ToDictionary(x => x.Key, x => HashSetFilter(x.Key, x.Value, r));
        }

        //public static HashSet<int> Edge(int i)
        //{
        //    bool answer = (0.02 <= y) ? true : false;
        //    return answer;
        //}
    }
}