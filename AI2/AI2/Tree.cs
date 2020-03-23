using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AI2
{
    //стан переходу
    public class State
    {
        //к-ть води у відрах
        public int pail5L;
        public int pail9L;

        //вихідні стани с поточного та попередній стан
        public State left, right, prev;
        public int fCost, gCost;

        //дефолтний конструктор стану
        public State()
        {

        }
        //конструктори ініціалізації стану
        public State(int pail5L, int pail9L)
        {
            this.pail5L = pail5L;
            this.pail9L = pail9L;
            left = right = prev = null;
        }
        public State(int pail5L, int pail9L, State prev)
        {
            this.pail5L = pail5L;
            this.pail9L = pail9L;
            this.prev = prev;
            left = right = null;
        }
    }
    //структура даних, в якій зберігаються стани, оскільки можливі два вихідні стани, то стани можна записувати у дерево
    public class Tree
    {
        //початковий стан
        public State root;
        //стани, у які не можна переходити після першої ітерації
        public State forbiddenState1 = new State(0, 0);
        public State forbiddenState2 = new State(0, 9);
        public State forbiddenState3 = new State(5, 0);
        public List<State> childs = new List<State>();
        //конструктор для дерева
        public Tree()
        {
            root = null;
        }

        //метод вставки знайдених станів у дерево
        public void Insert(State root, List<State> list, bool first)
        {
            if (first == true)
                this.root = root;
            first = false;
            if (root != null)
            {
                for (int i = 1; i < list.Count - 1; i++)
                {
                    if (list[i].prev.prev == root.prev && list[i].prev.pail5L == root.pail5L && list[i].prev.pail9L == root.pail9L && root.left == null)
                    {
                        root.left = list[i];
                    }
                    else if (list[i].prev.prev == root.prev && list[i].prev.pail5L == root.pail5L && list[i].prev.pail9L == root.pail9L && root.right == null)
                    {
                        root.right = list[i];
                    }
                }
                Insert(root.right, list, first);
                Insert(root.left, list, first);

            }
            else
                return;
        }
        //метод для знаходження можливих станів
        public void FillTree(State root)
        {
            Stack<State> stack = new Stack<State>();

            State current = new State();
            State previous = new State();
            bool firstChild = true, first = true;

            stack.Push(root);
            childs.Add(root);
            current.pail5L = root.pail5L;
            current.pail9L = root.pail9L;

            //цикл проходження по всім можливим станам
            while (stack.Count > 0)
            {
                if (first == true)
                    current = stack.Pop();
                if (!(current.pail5L == 3 && current.pail9L == 0) || !(current.pail9L == 0 && current.pail5L == 3))
                {
                    //можливі переходи
                    FeelEmpty5L(current, firstChild, stack, previous);
                    FeelEmpty9L(current, firstChild, stack, previous);
                    OverflowTo5L(current, firstChild, stack, previous);
                    OverflowTo9L(current, firstChild, stack, previous);
                    PourOut5L(current, firstChild, stack, previous);
                    PourOut9L(current, firstChild, stack, previous);
                }
                firstChild = false;
                previous = current;

                current = new State();

                if (stack.Count > 0)
                    current = stack.Pop();

                first = false;

            }
        }
        //заповнення 5-літрового відра, якщо воно порожнє
        public void FeelEmpty5L(State current, bool firstChild, Stack<State> stack, State previous)
        {
            int temp5L, temp9L;
            State tempPrev = new State();
            if (current.pail5L == 0)
            {
                temp5L = 5;
                temp9L = current.pail9L;
                tempPrev = current;

                CheckIfCanBeNewState(temp5L, temp9L, previous, tempPrev, firstChild, stack);
            }
            else
                return;

        }
        //заповнення 9-літрового відра, якщо воно порожнє
        public void FeelEmpty9L(State current, bool firstChild, Stack<State> stack, State previous)
        {
            int temp5L, temp9L;
            State tempPrev = new State();
            if (current.pail9L == 0)
            {
                temp9L = 9;
                temp5L = current.pail5L;
                tempPrev = current;

                CheckIfCanBeNewState(temp5L, temp9L, previous, tempPrev, firstChild, stack);
            }
            else
                return;

        }
        //переливання води з 5-літрового до 9-літрового, якщо в 5-літровому є вода, а 9-літрове не повне
        public void OverflowTo9L(State current, bool firstChild, Stack<State> stack, State previous)
        {
            int temp5L = 0, temp9L = 0;
            State tempPrev = new State();
            int delta = 0;
            if (current.pail5L != 0 && current.pail9L < 9)
            {
                delta = 9 - current.pail9L;
                tempPrev = current;
                if (delta <= 5)
                {
                    temp5L = current.pail5L - delta;
                    temp9L = delta + current.pail9L;
                }
                else if (delta > 5)
                {
                    temp5L = 0;
                    temp9L = current.pail9L + current.pail5L;
                }

                CheckIfCanBeNewState(temp5L, temp9L, previous, tempPrev, firstChild, stack);
            }
            else
                return;

        }
        //переливання води з 9-літрового до 5-літрового, якщо в 9-літровому є вода, а 5-літрове не повне
        public void OverflowTo5L(State current, bool firstChild, Stack<State> stack, State previous)
        {
            int temp5L = 0, temp9L = 0;
            State tempPrev = new State();
            int delta = 0;
            if (current.pail9L != 0 && current.pail5L < 5)
            {
                delta = 5 - current.pail5L;
                tempPrev = current;
                if (current.pail9L >= delta)
                {
                    temp5L = current.pail5L + delta;
                    temp9L = current.pail9L - delta;
                }
                else if (current.pail9L < delta)
                {
                    temp5L = current.pail9L + current.pail5L;
                    temp9L = 0;
                }

                CheckIfCanBeNewState(temp5L, temp9L, previous, tempPrev, firstChild, stack);
            }
            else
                return;
        }
        //виливання води з 5-літрового, якщо воно повне
        public void PourOut5L(State current, bool firstChild, Stack<State> stack, State previous)
        {
            int temp5L, temp9L = 0;
            State tempPrev = new State();
            if (current.pail5L == 5)
            {
                temp5L = 0;
                temp9L = current.pail9L;
                tempPrev = current;

                CheckIfCanBeNewState(temp5L, temp9L, previous, tempPrev, firstChild, stack);
            }
            else
                return;
        }
        //виливання води з 9-літрового, якщо воно повне
        public void PourOut9L(State current, bool firstChild, Stack<State> stack, State previous)
        {
            int temp5L, temp9L = 0;
            State tempPrev = new State();

            if (current.pail9L == 9)
            {
                temp5L = current.pail5L;
                tempPrev = current;
                temp9L = 0;

                CheckIfCanBeNewState(temp5L, temp9L, previous, tempPrev, firstChild, stack);
            }
            else
                return;
        }

        //метод перевірки чи є знайдений стан доцільним(не рівний попередньому або забороненому)
        public void CheckIfCanBeNewState(int temp5L, int temp9L, State previous, State tempPrev, bool firstChild, Stack<State> stack)
        {
            if (temp5L != previous.pail5L || temp9L != previous.pail9L)
            {
                if (firstChild == true)
                {
                    childs.Add(new State(temp5L, temp9L, tempPrev));
                    stack.Push(new State(temp5L, temp9L, tempPrev));
                }
                else if ((temp5L != forbiddenState1.pail5L || temp9L != forbiddenState1.pail9L)
                    && (temp5L != forbiddenState2.pail5L || temp9L != forbiddenState2.pail9L)
                    && (temp5L != forbiddenState3.pail5L || temp9L != forbiddenState3.pail9L))
                {
                    childs.Add(new State(temp5L, temp9L, tempPrev));
                    stack.Push(new State(temp5L, temp9L, tempPrev));
                }
            }
        }

        public List<State> closeList = new List<State>();
        public List<State> openList = new List<State>();
        bool goalSucc = false;


        //Рекурсивний метод пошуку за першим найкращим співпадінням
        public State RBFS(State Root, State Goal, double limit)
        {
            if (Root == Goal)
            {
                goalSucc = true;
                return Root;
            }
            else
            {
                State[] successors = ExpandNode(Root);
                successors.OrderBy(x => x.fCost);
                if (successors[0].fCost > limit)
                    return successors[0];
                else
                {
                    closeList.Add(Root);
                    foreach (var s in successors)
                    {
                        if (s != closeList.Last())
                            openList.Add(s);
                    }
                    openList.Sort();
                    State bestNode = openList[0];
                    openList.RemoveAt(0);
                    State alternativeNode = openList[0];
                    openList.RemoveAt(0);
                    while (goalSucc == false)
                    {
                        bestNode = RBFS(bestNode, Goal, Math.Min(limit, alternativeNode.fCost));
                        openList.Add(bestNode);
                        openList.OrderBy(x => x.fCost);
                        bestNode = openList[0];
                        openList.RemoveAt(0);
                        alternativeNode = openList[0];
                        openList.RemoveAt(0);
                    }
                    return bestNode;
                }
            }
        }
        //метод пошуку дочірніх станів
        public State[] ExpandNode(State Root)
        {
            State[] succ = new State[2];
            foreach (var s in succ)
            {
                s.prev = root;
                s.gCost = 0;
                State el = s;
                while (el.prev != Root.prev)
                {
                    s.gCost += el.gCost;
                    el = el.prev;
                }
                s.fCost = 0;
                el = s;
                while (!(el.pail5L == 0 && el.pail9L == 3) || !(el.pail5L == 3 && el.pail9L == 0))
                {
                    s.gCost += el.gCost;
                    if (el.left != null && (el.left.right != null || el.left.left != null))
                        el = el.left;
                    else
                        el = el.right;
                }
            }
            return succ;
        }
        //метод відображення знайденого шляху
        public virtual void ShowFindPath()
        {
            int s = SecondMin(root);
            int i;
            for (i = 1; i <= s; i++)
            {
                SearchPath(root, i, i, s);
            }
        }

        //пошук к-ті рівнів у дереві
        public virtual int SecondMin(State root)
        {
            if (root == null)
            {
                return 0;
            }
            else
            {
                //пошук к-ті рівнів у піддеревах
                int lmin = SecondMin(root.left);
                int rmin = SecondMin(root.right);

                //вибір довшої гілки
                if (lmin > rmin)
                {
                    return (lmin + 1);
                }
                else
                {
                    return (rmin + 1);
                }
            }
        }

        //рекурсивний пошук розв'язку у рівні дерева
        public virtual void SearchPath(State root, int limit, int last, int min)
        {
            if (root == null)
            {
                return;
            }
            if (limit == 1)
            {
                Console.Write(root.pail5L + " " + root.pail9L);
                if (last != min)
                    Console.Write(" -> ");
            }
            else if (limit > 1)
            {
                SearchPath(root.left, limit - 1, last, min);
                SearchPath(root.right, limit - 1, last, min);

            }

        }
    }


}

