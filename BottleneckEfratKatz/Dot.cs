using System;
using System.Collections.Generic;

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
            ProjectedFrom = new List<Dot>(); ///по умолчанию точка - не с диагонали, если да - то список будет заполняться при проецировании дургих точек на диагональную
        }

        private double _birthTime;
        private double _deathTime;
        private int _sourceMult; ///исходная кратность
        private int _currMult;  ///текущая кратность

        private List<Dot> _projectedFrom; /// если пустой, то это точка не с диагонали
                                         /// если не пустой - то точка с диагонали, а элементы - те точки, проекцией которых она является
        public int SourceMult
        {
            get { return _sourceMult; } ///читаем исходную кратность
            set { _sourceMult = value; CurrMult = value; } ///задаём исходную и => текущую кратность
        }

        public double BirthTime        { get => _birthTime;     set => _birthTime     = value; }
        public double DeathTime        { get => _deathTime;     set => _deathTime     = value; }
        public int    CurrMult         { get => _currMult;      set => _currMult      = value; }
        public List<Dot> ProjectedFrom { get => _projectedFrom; set => _projectedFrom = value; }

        public static double Distance(Dot parent, Dot inherior)
        {
            try
            {
                if (parent.ProjectedFrom.Count == 0 || ///если хотя бы один элемент не диагональный, иначе расстояние равно нулю
                    inherior.ProjectedFrom.Count == 0)
                {
                    double result = Math.Max(Math.Abs(parent.BirthTime - inherior.BirthTime), Math.Abs(parent.DeathTime - inherior.DeathTime)); /// max{|birth1 - birth2|, |death1 - death2|}
                    return result;
                }
                else
                {
                    double result = 0.0;
                    return result;
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem with distance", e);
                throw;
            }

        }
    }
}