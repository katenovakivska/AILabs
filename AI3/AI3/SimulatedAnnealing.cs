using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI3
{
    public class SimulatedAnnealing
    {
        //локальний шлях
        public List<State> pathLocal = new List<State>();
        //всі пройдені стани
        public List<State> pathAll = new List<State>();
        //температура(максимальна довжина шляху)
        public int temperature;
        //максимально можлива кількість ітерацій
        public int iterations;
        //кінцевий стан(розв'язок)
        public State finish = new State(new int[] { 0, 0, 0, -1, 1, 1, 1, 1 });
        //можливі вихідні стани з поточного
        public List<State> childs = new List<State>();
        //індекс пустої лунки
        int empty;
        //дефолтний конструктор
        public SimulatedAnnealing()
        {
        }
        //конструктор з параметрами температури та кількості ітерацій
        public SimulatedAnnealing(int temperature, int iterations)
        {
            this.temperature = temperature;
            this.iterations = iterations;
        }

        //метод пошуку шляху
        public List<State> Search(State first)
        {
            bool findResult = false;          //змінна, що вказує, чи розв'язок досягнутий
            for (int k = 0; k < iterations; k++)
            {

                if (findResult == true)
                {
                    return pathAll;
                }

                Console.WriteLine($"-------------------Iteration  {k + 1}-------------------");

                pathLocal = new List<State>();
                Random random = new Random();
                State current = new State();

                //генерація стану
                for (int i = first.holes.Length - 1; i >= 0; i--)
                {
                    int j = random.Next(i + 1);
                    var temp = first.holes[j];
                    first.holes[j] = first.holes[i];
                    first.holes[i] = temp;
                }
                //вивід згенерованого стану(початку локального шляху)
                Console.Write("Start state: ");
                for (int i = 0; i < first.holes.Length; i++)
                {
                    Console.Write($"{first.holes[i]}  ");
                }
                Console.WriteLine();

                //пошук значення евристичної функції(кількість лунок значення яких співпадає з розв'язком)
                int h = first.CalculateHeuristic(finish);
                int hOld = h;

                current.holes = first.holes;
                pathLocal.Add(current);
                pathAll.Add(current);
                //пошук локального шляху
                while (temperature > 0 && current.holes != null && !IsFinish(current))
                {
                    //пошук пустої лунки
                    for (int i = 0; i < current.holes.Length; i++)
                    {
                        if (current.holes[i] == -1)
                        {
                            empty = i;
                        }
                    }
                    //пошук вихідних станів з поточного
                    childs = new List<State>();
                    childs = FirstLeftIsBlack(empty, current);
                    childs = FirstRightIsWhite(empty, current);
                    childs = SecondLeftIsBlack(empty, current);
                    childs = SecondRightIsWhite(empty, current);

                    //обрахунок значень евристичних функцій для вихідних станів
                    foreach (var el in childs)
                    {
                        el.CalculateHeuristic(finish);
                        el.delta = el.heuristic - h;
                    }
                    current = new State();
                    int max = -8;
                    //вибір стану за макс значенням евристичної функції
                    foreach (var el in childs)
                    {
                        if (el.delta >= max)
                        {
                            max = el.delta;
                            current.holes = el.holes;
                            current.delta = el.delta;
                            h = el.heuristic;
                        }
                    }
                    //різниця значень евристичних функції поточного та попереднього стану
                    int deltaH = current.delta;
                    Random rand = new Random();
                    const int decimals = 10000;
                    int large = rand.Next(0, decimals);
                    //розрахунок межі
                    double threshold = (double)large / decimals;
                    //розрахунок ймовірності
                    double probability = Math.Exp(-(double)deltaH / (double)temperature);
                    //пониження температури
                    temperature -= 1;
                    //якщо межа більша, то на наступній ітерації значення евристичної функції буде рівним попередньому
                    if (probability < threshold)
                    {
                        h = hOld;
                    }
                    else
                    {
                        hOld = h;
                    }
                    //вставка поточного стану у шлях
                    if (current.holes != null)
                    {
                        pathLocal.Add(current);
                        pathAll.Add(current);
                    }


                    if (current.holes != null && IsFinish(current) == true)
                    {
                        findResult = true;
                    }

                }

                ShowLocalPath();

            }
            return pathAll;
        }
        //відображення локального шляху
        public void ShowLocalPath()
        {
            Console.WriteLine("The local path is: ");
            foreach (var el in pathLocal)
            {
                for (int i = 0; i < el.holes.Length; i++)
                {
                    Console.Write($"{el.holes[i]} ");
                }
                Console.WriteLine();
            }
        }
        //відображення всього шляху
        public void ShowAllPath()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("The all path is: ");

            foreach (var el in pathAll)
            {
                for (int i = 0; i < el.holes.Length; i++)
                {
                    Console.Write($"{el.holes[i]} ");
                }
                Console.WriteLine();
            }
        }
        //перевірка чи стан є розв'язком 
        public bool IsFinish(State state)
        {
            bool equal = true;
            for (int i = 0; i < state.holes.Length; i++)
            {
                if (state.holes[i] != finish.holes[i])
                {
                    equal = false;
                }
            }
            return equal;
        }
        //методи для отримання нових станів
        public List<State> FirstLeftIsBlack(int empty, State current)
        {
            int[] temp = new int[8];
            current.holes.CopyTo(temp, 0);
            if (empty - 1 >= 0 && current.holes[empty - 1] == 1)
            {
                temp[empty] = 1;
                temp[empty - 1] = -1;
                childs.Add(new State(temp));
            }

            return childs;
        }

        public List<State> FirstRightIsWhite(int empty, State current)
        {
            int[] temp = new int[8];
            current.holes.CopyTo(temp, 0);
            if (empty + 1 < current.holes.Length && current.holes[empty + 1] == 0)
            {
                temp[empty] = 0;
                temp[empty + 1] = -1;
                childs.Add(new State(temp));
            }
            return childs;
        }

        public List<State> SecondLeftIsBlack(int empty, State current)
        {
            int[] temp = new int[8];
            current.holes.CopyTo(temp, 0);
            if (empty - 2 >= 0 && current.holes[empty - 2] == 1)
            {
                temp[empty] = 1;
                temp[empty - 2] = -1;
                childs.Add(new State(temp));
            }
            return childs;
        }

        public List<State> SecondRightIsWhite(int empty, State current)
        {
            int[] temp = new int[8];
            current.holes.CopyTo(temp, 0);
            if (empty + 2 < 8 && current.holes[empty + 2] == 0)
            {
                temp[empty] = 0;
                temp[empty + 2] = -1;
                childs.Add(new State(temp));
            }
            return childs;
        }
    }
}
