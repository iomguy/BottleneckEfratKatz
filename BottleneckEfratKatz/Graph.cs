using System.Collections.Generic;

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

    }

}