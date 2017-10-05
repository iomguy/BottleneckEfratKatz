using System.Collections.Generic;

namespace BottleneckEfratKatz
{
    public class PersDiagram 
        //список из точек персистентной диаграммы с кратностями
        // отдельный тип создаётся, т.к. все операции добавления и вычитания элементов должны вестись с обновлением кратностей
    {
        public PersDiagram() ///конструктор
        {
            DotList = new List<Dot>(); //тут стоит использовать массив
        }

        private List<Dot> _dotList; /// список точек с кратностями
        public  List<Dot> DotList { get => _dotList;       set => _dotList = value; }

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
}