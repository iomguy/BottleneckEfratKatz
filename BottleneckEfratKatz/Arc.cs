namespace BottleneckEfratKatz
{
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
}