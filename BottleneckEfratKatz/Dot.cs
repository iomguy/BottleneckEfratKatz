using System;

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
}