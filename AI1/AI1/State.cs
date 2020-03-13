using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI1
{
    //стан
    public class State
    {
        //массив стану з порядком лунок
        public int[] array;
        //список дочірніх станів стану
        List<State> childs;
        //статус стану
        bool isVisited;

        //геттер та сеттер для списку дочірніх станів
        public List<State> Childs
        {
            get { return childs; }
            set { childs = value; }
        }

        //гетткр та сеттер для массиву з порядком лунок
        public int[] Array
        {
            get { return array; }
            set { this.array = value; }
        }

        //геттер та сеттер для статусу стану
        public bool IsVisited
        {
            get { return isVisited; }
            set { isVisited = value; }
        }

        //геттер для к-ті дочірніх станів
        public int ChildCount
        {
            get { return childs.Count; }
        }

        //конструктор ініціалізації стану за массивом порядку лунок
        public State(int[] array)
        {
            this.array = array;
            isVisited = false;
            childs = new List<State>();
        }

        //конструктор ініціалізації за массивом порядку станів та списком дочірніх станів 
        public State(int[] array, List<State> childs)
        {
            this.array = array;
            isVisited = false;
            this.childs = childs;
        }

        //зміна статусу вершини на відвіданий
        public void Visit()
        {
            isVisited = true;
        }

        //додавання до списку дочірніх нового дочірнього стану
        public void AddEdges(List<State> newChilds)
        {
            childs.AddRange(newChilds);
        }

        //перевантаження методу для виводу графа
        public override string ToString()
        {

            StringBuilder allChilds = new StringBuilder("");
            
            for (int i = 0; i < this.Array.Length; i++)
            {
                allChilds.Append(this.Array[i] + " ");
                
            }

            allChilds.Append(": ");

            foreach (State child in childs)
            {
                for (int i = 0; i < child.Array.Length; i++)
                {
                    allChilds.Append(child.Array[i] + "  ");
                }
                allChilds.Append("\n");
            }

            return allChilds.ToString();

        }
        //підрахунок к-ті станів у які необхідно перейти, щоб прийти до розв'язку
        public int CountStatesToResult(int count)
        {

            StringBuilder allChilds = new StringBuilder("");

            for (int i = 0; i < this.Array.Length; i++)
            {
                allChilds.Append(this.Array[i] + " ");

            }

            allChilds.Append(": ");

            foreach (State child in childs)
            {
                for (int i = 0; i < child.Array.Length; i++)
                {
                    
                    allChilds.Append(child.Array[i] + "  ");
                }
                if (!allChilds.Equals("0 0 0 -1 1 1 1 1 "))
                    count += 1;
                allChilds.Append("\n");
            }

            return count;

        }

    }
}
