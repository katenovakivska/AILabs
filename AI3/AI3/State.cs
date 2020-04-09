using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI3
{
    public class State
    {
        //массив лунок
        public int[] holes;
        //значення евристичної функції в стані
        public int heuristic;
        //різниця евристичних функцій поточного стану та попереднього до нього
        public int delta;
        //дефолтний конструктор
        public State()
        {
        }
        //конструктор ініціалізації
        public State(int[] holes)
        {
            this.holes = holes;
        }
        //метод обрахунку значення евристичної функції
        public int CalculateHeuristic(State finish)
        {
            heuristic = 0;

            for (int i = 0; i < holes.Length; i++)
            {
                if (holes[i] == finish.holes[i])
                {
                    heuristic++;
                }
            }

            return heuristic;
        }

    }

}
