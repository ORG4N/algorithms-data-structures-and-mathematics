using System;

namespace mastermind
{
    class Program
    {
        public enum States {START, NUMBERS, POSITIONS, PLAYING, END, QUIT};
        public static States currentState = States.START;

        public int numbersMax = 0;
        public int positions = 0;

        public int[] pattern = new int[0]; 

        static void Main(string[] args)
        {
            Program app = new Program();

            Console.WriteLine("MASTERMIND\n");

            Console.WriteLine("Inputs:");
            Console.WriteLine(" 'rules' - to learn how to play");
            Console.WriteLine(" 'play'  - to start playing");
            Console.WriteLine(" 'quit'  - to exit the program");

            while (currentState != States.QUIT)
            {
                string input = app.readInput();
                app.processInput(input);
                app.appLoop();
            }
        }

        public void appLoop()
        {
            if (currentState == States.START)
            {
                Console.WriteLine("Inputs:");
                Console.WriteLine(" 'rules' - to learn how to play");
                Console.WriteLine(" 'play'  - to start playing");
                Console.WriteLine(" 'quit'  - to exit the program");
            }

            if (currentState == States.NUMBERS)
            {
                Console.WriteLine("\nRange of numbers   : 1 to -");
                Console.WriteLine("Amount of positions: -\n");

                Console.WriteLine("Choose the amount of possible NUMBERS for the pegs to be chosen from.");
                Console.WriteLine(" - Input must be an integer between 1 and 8");
            }

            if (currentState == States.POSITIONS)
            {
                Console.WriteLine("\nRange of numbers   : 1 to {0}", numbersMax);
                Console.WriteLine("Amount of positions: -\n");

                Console.WriteLine("Choose the amount of POSITIONS within the secret pattern.");
                Console.WriteLine(" - Input must be an integer greater than 1");
            }

            if (currentState == States.PLAYING)
            {
                Console.Clear();
                Console.WriteLine("\nRange of numbers   : 1 to {0}", numbersMax);
                Console.WriteLine("Amount of positions: {0}\n", positions);

                pattern = new int[positions];
                getPattern();
            }
        }

        // Take an input from the user and convert to lowercase
        public string readInput()
        {
            Console.Write("\nInput here: ");
            string input = Console.ReadLine();
            Console.WriteLine("");
            return input.ToLower();
        }


        // Use the input to determine if the program should do anything (i.e. change a state, call a method)
        public void processInput(string input)
        {
            if (input == "rules" && currentState == States.START) { printRules(); }

            else if (input == "quit") { currentState = States.QUIT; }

            else if (input == "play" && currentState == States.START) 
            {
                currentState = States.NUMBERS;
                Console.Clear();
            }

            else if (currentState == States.NUMBERS)
            {
                int num;
                bool isNumber = Int32.TryParse(input, out num);

                if (isNumber)
                {
                    if (num > 0 && num <= 8)
                    {
                        numbersMax = num;
                        currentState = States.POSITIONS;
                    }

                    else { Console.WriteLine("\nINVALID INPUT - Input must be a number and between 1 and 8\n"); }

                }

                else {Console.WriteLine("\nINVALID INPUT - Input must be a number and between 1 and 8\n");}
            }

            else if (currentState == States.POSITIONS)
            {
                int num;
                bool isNumber = Int32.TryParse(input, out num);

                if (isNumber)
                {
                    if (num > 0)
                    {
                        positions = num;
                        currentState = States.PLAYING;
                    }

                    else {Console.WriteLine("\nINVALID INPUT - Input must be a number and greater than 1\n");}
                }

                else {Console.WriteLine("\nINVALID INPUT - Input must be a number and greater than 1\n");}
            }
        }


        // Write all the rules to the screen
        public void printRules()
        {
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" - Player A will secretly select N pegs from a selection of M numbers and position them in a set order to create a hidden pattern.");
            Console.WriteLine(" - Player B will then make a series of guesses to try to find out Player A's pattern.\n");

            Console.WriteLine(" - After confirming their turn, Player B will be able to see which of their guesses are correct via black and white pegs.");
            Console.WriteLine("    - Black: position and number are both correct");
            Console.WriteLine("    - White: position is incorrect, but the number exists somewhere within the pattern\n");


            Console.WriteLine(" - The program will take the role of Player A, and therefore the user is Player B.");
            Console.WriteLine(" - The game will end when either:");
            Console.WriteLine("    a. The user forfeits or exits the program.");
            Console.WriteLine("    b. The user wins the game.");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------\n");
        }

        public int getNumber()
        {
            Random rand = new Random();
            int number = rand.Next(1, numbersMax+1);

            return number;
        }

        public void getPattern()
        {
            for(int i=0; i<positions; i++)
            {
                int randomNumber = getNumber();
                pattern[i] = randomNumber;
                //Console.Write(pattern[i] + "");
            }
        }
    }
}
