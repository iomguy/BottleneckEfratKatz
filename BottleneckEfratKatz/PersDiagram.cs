using System.Collections.Generic;

namespace BottleneckEfratKatz
{
    public class PersDiagram 
        // список из точек персистентной диаграммы с кратностями
        // отдельный тип создаётся, т.к. все операции добавления и вычитания элементов должны вестись с обновлением кратностей
    {
        public PersDiagram() ///конструктор
        {
            DotList = new List<Dot>(); //тут стоит использовать массив
            DictIndex = new Dictionary<int, Dot>(); ///словарь { целочисленный индекс : точка}
            FullSetOfIndex = new HashSet<int>();
        }

        private List<Dot> _dotList; /// список точек с кратностями
        private IDictionary<int, Dot> dictIndex;
        private HashSet<int> fullSetOfIndex;

        public  List<Dot> DotList              { get =>  _dotList;      set => _dotList       = value; }
        public IDictionary<int, Dot> DictIndex { get => dictIndex;      set => dictIndex      = value; }
        public HashSet<int> FullSetOfIndex     { get => fullSetOfIndex; set => fullSetOfIndex = value; }

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

        public void BuildDictIndex(int firstIndex = 1)
            //строит словарь {целочисленный индекс : точка}
            //присваивает каждой точке индексы, которые ей соответствуют в зависимости от кратности dot.SetOfIndex = {int1, int2 ...}
        {
            var resultDict = new Dictionary<int, Dot>();
            int i = firstIndex;
            foreach (Dot dot in DotList)
            {
                for (int j = 0; j < dot.SourceMult; j++)
                {
                    DictIndex[i] = dot;
                    dot.SetOfIndex.Add(i);
                    FullSetOfIndex.Add(i);
                    i++;
                }
            }
            return;
        }

        public int indexOfBirthDeath(double birth, double death) 
            //ищет индекс по координатам birth, death
        {
            int index = DotList.FindIndex(dot => dot.BirthTime == birth && dot.DeathTime == death); ///возвращает -1, если элемент не найден
            return index;
        }

        public Dot AddDot(Dot dot) 
            //добавляет точку с некоторой кратностью с учётом того, что такая точка уже может быть
        {
            int repeatedString = indexOfBirthDeath(dot.BirthTime, dot.DeathTime);
            if (repeatedString == -1)        ///если такая строка ещё не попадалась
            {
                DotList.Add(dot);            /// добавляем новую точку в перс диаграмму
                return dot;                ///возвращаем ссылку на эту же точку, если она ещё не встречалась
            }
            else
                DotList[repeatedString].SourceMult += dot.SourceMult; ///точку не добавляем, увеличиваем кратность, если попадалась
                return DotList[repeatedString];                     ///возвращаем ссылку на точку, кратность которой увеличиваем
        }

        public static PersDiagram PersDiagramCup(PersDiagram persDiagramA, PersDiagram persDiagramB)
        //создаёт объединение (с учётом кратностей) множества и и множества проекций
        {
            PersDiagram persDiagramResult = new PersDiagram();
            foreach (Dot dot in persDiagramA.DotList)
            {
                ///Dot nonProjectedDot = new Dot(dot.BirthTime, dot.DeathTime, dot.SourceMult); ///копируем первую персистентную диаграмму                
                Dot nonProjectedDot = dot; ///ссылаемся на первую персистентную диаграмму  
                Dot toDot = persDiagramResult.AddDot(nonProjectedDot);
            }

            foreach (Dot dot in persDiagramB.DotList)
            {
                var diagCoord = (dot.BirthTime + dot.DeathTime) / 2; ///координата проекции на диагональ
                Dot projectedDot = new Dot(diagCoord, diagCoord, dot.SourceMult);
                
                Dot toDot = persDiagramResult.AddDot(projectedDot);
                toDot.ProjectedFrom.Add(dot); ///сохраняем в свойстве toDot (на которую в итоге спроецировали) тот факт, что dot спроецирована на toDot
            }
            return persDiagramResult;
        }

        ///нужно создать метод, который будет выдавать элементы с ненулевыми кратностями
        ///
    }
}