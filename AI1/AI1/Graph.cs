using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI1
{
    //граф станів
    public class Graph
    {
        //список вершин у графі 
        private List<State> vertices;

        //к-ть вершин у графі
        int size;

        //геттер для списку вершин
        public List<State> Vertices
        {
            get { return vertices; }
        }

        //геттер для к-ті вершин
        public int Size
        {
            get { return vertices.Count; }
        }

        //конструктор ініціалізації графу за к-тю вершин
        public Graph(int initialSize)
        {
            //перевірка чи розмір не є меншим ніж 0
            if (size < 0)
            {
                throw new ArgumentException("Number of vertices cannot be negative");
            }

            size = initialSize;

            vertices = new List<State>(initialSize);

        }

        //конструктор ініціалізації за списком вершин
        public Graph(List<State> initialNodes)
        {
            vertices = initialNodes;
            size = vertices.Count;
        }

        //метод для заповнення графу станів
        public List<State> FillGraph()
        {
            //початковий стан
            State root = new State(new int[] { 1, 1, 1, 1, -1, 0, 0, 0 });
            //стек для вершин всі стани переходу з якого ще не знайдені
            Stack<State> stack = new Stack<State>();
            //список вершин графу
            List<State> states = new List<State>();
            //список вихідних станів з поточної вершини
            List<State> stateList = new List<State>();
            //поточний стан
            State current = new State(new int[8]);


            bool find = false;
            int i = 0, empty = 0, statesIndex = 0, count = 1;
           
            stack.Push(root);
            root.Array.CopyTo(current.Array, 0);
            //цикл, що працює поки є елементи у стеку
            while (stack.Count > 0)
            {
                //додавання стану до списку станів
                states.Add(current);
                //цикл пошуку пустої лунки
                while (find == false)
                {
                    if (current.Array[i] == -1)
                    {
                        empty = i;
                        find = true;
                    }
                    i++;
                }

                //перевірка чи перша зліва вершина є чорною
                count = FirstLeftBlack(current, stack, empty, states, stateList, count);
                //перевірка чи друга зліва вершина є чорною
                count = SecondLeftBlack(current, stack, empty, states, stateList, count);
                //перевірка чи перша справа вершина є білою
                count = FirstRightWhite(current, stack, empty, states, stateList, count);
                //перевірка чи друга справа вершина є білою
                count = SecondRightWhite(current, stack, empty, states, stateList, count);
                
                //додавання списку станів поточної вершини до графу
                states[statesIndex].AddEdges(stateList);
                
                //к-ть елементів у графі
                statesIndex = count + 1;
                //очистка списку
                stateList.Clear();
                current = new State(new int[8]);
                //вилучення елементу з стеку
                current = stack.Pop();
                find = false;
                i = 0;
            }
            return states;
        }

        //перевірка чи перша зліва вершина є чорною
        public int FirstLeftBlack(State root, Stack<State> stack, int empty, List<State> states, List<State> stateList, int count)
        {
            int[] temp = new int[8];
            root.Array.CopyTo(temp, 0);
            if (empty - 1 >= 0 && root.Array[empty - 1] == 1)
            {
                temp[empty] = 1;
                temp[empty - 1] = -1;
                temp.CopyTo(temp, 0);
                stack.Push(new State(temp));
                count += 1;
                states.Add(new State(temp));
                stateList.Add(new State(temp));
            }
            return count;
        }

        //перевірка чи друга зліва вершина є чорною
        public int SecondLeftBlack(State root, Stack<State> stack, int empty, List<State> states, List<State> stateList, int count)
        {
            int[] temp = new int[8]; 
            root.Array.CopyTo(temp, 0);
            if (empty - 2 >= 0 && root.Array[empty - 2] == 1)
            {
                temp[empty] = 1;
                temp[empty - 2] = -1;
                temp.CopyTo(temp, 0);
                stack.Push(new State(temp));
                count += 1;
                states.Add(new State(temp));
                stateList.Add(new State(temp));
            }
            return count;
        }

        //перевірка чи перша справа вершина є білою
        public int FirstRightWhite(State root, Stack<State> stack, int empty, List<State> states, List<State> stateList, int count)
        {
            int[] temp = new int[8];
            root.Array.CopyTo(temp, 0);
            if (empty + 1 < root.Array.Length && root.Array[empty + 1] == 0)
            {
                temp[empty] = 0;
                temp[empty + 1] = -1;
                temp.CopyTo(temp, 0);
                stack.Push(new State(temp));
                count += 1;
                states.Add(new State(temp));
                stateList.Add(new State(temp));
            }
            return count;
        }

        //перевірка чи друга справа вершина є білою
        public int SecondRightWhite(State root, Stack<State> stack, int empty, List<State> states, List<State> stateList, int count)
        {
            int[] temp = new int[8];
            root.Array.CopyTo(temp, 0);
            if (empty + 2 < root.Array.Length && root.Array[empty + 2] == 0)
            {
                temp[empty] = 0;
                temp[empty + 2] = -1;
                temp.CopyTo(temp, 0);
                stack.Push(new State(temp));
                count += 1;
                states.Add(new State(temp));
                stateList.Add(new State(temp));

            }
            return count;
        }

        //пошук в глибину
        public int DepthFirstSearch(State root, int count)
        {
            //якщо вершина графу ще не пройдена
            if (!root.IsVisited)
            {
                count = root.CountStatesToResult(count);
                Console.Write(root.ToString() + " ");
                root.Visit();

                //прохід пошуку в глибину для кожного дочірнього стану поточної вершини
                foreach (State child in root.Childs)
                {
                    DepthFirstSearch(child, count);
                }
                
            }
            return count;

        }

       
    }
}
