using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI1
{
    
    class Program
    {
        static void Main(string[] args)
        {
            List<State> states = new List<State>();
            int count = 0, i = 0;
          
            State state = new State(new int[] { 1, 1, 1, 1, -1, 0, 0, 0 });
            Graph stateGraph = new Graph(states); ;
            states = stateGraph.FillGraph();
      
            stateGraph = new Graph(states);

            //foreach (State el in states)
            //{
            //    Console.WriteLine(el.ToString());
            //}

            foreach (State el in states)
            {
                count = stateGraph.DepthFirstSearch(el, i);
                i = count;
            }
            Console.WriteLine($"Result can be reached while walkthrough - {count} states");
            Console.ReadKey();
        }
    }
}
