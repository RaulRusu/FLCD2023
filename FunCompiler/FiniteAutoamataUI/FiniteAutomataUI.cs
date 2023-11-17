using FunCompiler.DataStructers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.FiniteAutoamataUI
{
    public class FiniteAutomataUI
    {
        private FiniteAutomata finiteAutomata;

        public FiniteAutomataUI(FiniteAutomata autoamata)
        {
            finiteAutomata = autoamata;
        } 

        private void DisplayMenu()
        {
            Console.WriteLine("1. States");
            Console.WriteLine("2. The alphabet");
            Console.WriteLine("3. Transitions");
            Console.WriteLine("4. Initial state");
            Console.WriteLine("5. Final state");
        }

        private void PrintStates()
        {
            Console.WriteLine("States");

            finiteAutomata.States.ForEach(state =>
            {
                Console.Write($"{state} ");
            });
            Console.WriteLine();
        }

        private void PrintAlphabet()
        {
            Console.WriteLine("Alphabet");

            finiteAutomata.InputSymbols.ForEach(symbol =>
            {
                Console.Write($"{symbol} ");
            });
            Console.WriteLine();
        }

        private void PrintTransitions()
        {
            Console.WriteLine("Transitions");

            finiteAutomata.TransitionFunction.ForEach(transition =>
            {
                Console.WriteLine($"{transition.StartingState} {transition.EndingState} {transition.Symbol}");
            });
            Console.WriteLine();
        }

        private void PrintInitialState()
        {
            Console.WriteLine($"Inital State: {finiteAutomata.InitialState}");
        }

        private void PrintFinalStates()
        {
            Console.WriteLine("Final states");

            finiteAutomata.FinalStates.ForEach(state =>
            {
                Console.Write($"{state} ");
            });
            Console.WriteLine();
        }

        private void HandleCommand(string command)
        {
            if (command == "1")
            {
                PrintStates();
            }
            if (command == "2")
            {
                PrintAlphabet();
            }
            if (command == "3") 
            {
                PrintTransitions();
            }
            if (command == "4") 
            { 
                PrintInitialState();
            }
            if (command == "5") 
            {
                PrintFinalStates();
            }
        }

        public void Run()
        {
            while (true)
            {
                DisplayMenu();
                var command = Console.ReadLine();

                if (command == "")
                {
                    continue;
                }
                command = command.Trim();

                HandleCommand(command);
                if (command == "exit")
                {
                    return;
                }
            }
        }
    }
}
