using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI2
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            State Root = new State(0, 0);
            tree.FillTree(Root);
            Console.WriteLine("First find path is:");
            State root = new State();
            State Result = new State();
            bool first = true;
            tree.Insert(tree.childs[0], tree.childs, first);
            tree.ShowFindPath();
            Console.ReadKey();
        }
    }
}
