using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI3
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] values = new int[] { 1,1,1,1,-1,0,0,0};
            //int[] values = new int[] { 1,0,1,0,-1,1,0,1};
            State state = new State(values);
            int iterations = 50, temperature = 200;
            SimulatedAnnealing annealing = new SimulatedAnnealing(temperature, iterations);
            annealing.Search(state);
            annealing.ShowAllPath();
           
            Console.ReadKey();
        }
    }
}
