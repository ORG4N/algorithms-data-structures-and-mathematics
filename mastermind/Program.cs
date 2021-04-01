using System;

namespace mastermind
{
    class Program
    {
        public enum States {START, PLAYING, QUIT};
        public static States currentState = States.START;

        static void Main(string[] args)
        {
            Program app = new Program();

            Console.WriteLine("MASTERMIND\n");

            Console.WriteLine("Inputs:");
            Console.WriteLine(" 'rules' - to learn how to play");
            Console.WriteLine(" 'play'  - to start playing\n");

            while (currentState != States.QUIT)
            {
                string input = app.readInput();
                app.processInput(input);
            }
        }

        // Take an input from the user and convert to lowercase
        public string readInput()
        {
            Console.Write("Input here: ");
            string input = Console.ReadLine();
            return input.ToLower();
        }


        // Use the input to determine if the program state needs to change
        public void processInput(string input)
        {
            if (input == "rules" && currentState == States.START) { printRules(); }
            if (input == "play" && currentState == States.START) { currentState = States.PLAYING; }
        }


        // Write all the rules to the screen
        public void printRules()
        {
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" - Player A will secretly select N pegs from a selection of M colours and position them in a set order to create a hidden pattern.");
            Console.WriteLine(" - Player B will then make a series of guesses to try to find out Player A's pattern.\n");

            Console.WriteLine(" - After confirming their turn, Player B will be able to see which of their guesses are correct via black and white pegs.");
            Console.WriteLine("    - Black: position and colour are both correct");
            Console.WriteLine("    - White: position is incorrect, but the colour exists somewhere within the pattern\n");


            Console.WriteLine(" - The program will take the role of Player A, and therefore the user is Player B.");
            Console.WriteLine(" - The game will end when either:");
            Console.WriteLine("    a. The user forfeits or exits the program.");
            Console.WriteLine("    b. The user wins the game.");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------\n");
        }
    }
}
