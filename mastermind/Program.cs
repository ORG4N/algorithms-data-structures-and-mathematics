using System;

namespace mastermind
{
    class Node
    {
        public string[] data;
        public Node next;
    }

    class Queue
    {
        public Node front = null;                   // First item
        public Node rear = null;                   // Last item
        public int size = 0;
    }

    class Program
    {
        public enum States {START, NUMBERS, POSITIONS, PLAYING, COMPARISON, END, QUIT};
        public static States currentState = States.START;

        public int numbersMax = 0;              // Upper limit of the range of numbers the pattern can include
        public int positions = 0;               // Size of the pattern

        public int[] pattern = new int[0];      // Array for storing random pattern
        public int[] guess = new int[0];        // Array for storing user's guess
        public int guessCount = 0;              // Keeps track of the digit position that the user is guessing

        public bool askInput = true;            // Due to program structure, sometimes an input does not need to be requested, so this bool controls that

        Queue history = new Queue();

        // Add an element to the queue
        public void Enqueue(Queue q, String[] item)
        {

            Node addItem = new Node();
            addItem.data = item;
            addItem.next = null;

            // If the front is null then the queue is empty and therefore when we add our first item it will also be the last
            if (q.front == null)
            {
                q.front = addItem;
                q.rear = addItem;
            }

            else
            {
                q.rear.next = addItem;
                q.rear = addItem;
            }

            q.size++;

        }

        // Pop the first element from the queue
        public void Dequeue(Queue q)
        {
            if (q.front == null)
            {
                Console.WriteLine("empty");
            }

            Object item = q.front.data;
            q.front = q.front.next;
            q.size--;
        }

        // Print first element without removing it
        public void Peek(Queue q, int i)
        {
            if (q.front == null)
            {
                Console.WriteLine("empty");
            }

            Console.WriteLine(" {0}.   {1}               B: {2}  W: {3}", i, q.front.data[0], q.front.data[1], q.front.data[2]);
        }

        // Print entire queue without deleting it
        public void Traversal(Queue q)
        {
            Node temp = new Node();
            temp.data = q.front.data;
            temp.next = q.front.next;

            int i = 1;
            while (temp != null)
            {
                Console.WriteLine(" {0}.   {1}               B: {2}  W: {3}", i, temp.data[0], temp.data[1], temp.data[2]);
                temp = temp.next;
                i++;
            }

        }

        // Program will loop until the program is closed or exited
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
                if (app.askInput)
                {
                    string input = app.readInput();
                    app.processInput(input);
                }
    
                app.AppLoop();
            }
        }

        // Perform a specific action which is determined by the app state
        public void AppLoop()
        {
            askInput = true;

            if (currentState == States.START)
            {
                Console.WriteLine("Inputs:");
                Console.WriteLine(" 'rules' - to learn how to play");
                Console.WriteLine(" 'play'  - to start playing");
                Console.WriteLine(" 'quit'  - to exit the program");
            }

            else if (currentState == States.NUMBERS)
            {
                Console.WriteLine("\nRange of numbers   : 1 to -");
                Console.WriteLine("Amount of positions: -\n");

                Console.WriteLine("Choose the amount of possible NUMBERS for the pegs to be chosen from.");
                Console.WriteLine(" - Input must be an integer between 1 and 8");
            }

            else if (currentState == States.POSITIONS)
            {
                Console.WriteLine("\nRange of numbers   : 1 to {0}", numbersMax);
                Console.WriteLine("Amount of positions: -\n");

                Console.WriteLine("Choose the amount of POSITIONS within the secret pattern.");
                Console.WriteLine(" - Input must be an integer greater than 1");                
            }

            // Calculate the total amount of black and white pegs found in the user's guess
            else if (currentState == States.COMPARISON)
            {
                int whitePegs = 0;
                int blackPegs = 0;

                string[] historyItem = new string[3];
                string guessString = string.Empty;

                // Convert the guess from an int array to a string and then store in string array
                for (int i = 0; i < positions; i++)
                {
                    guessString = guessString + guess[i];
                }

                historyItem[0] = guessString;


                // Copy the pattern into another array so that the array can be modified without losing the original pattern
                int[] posCopy = new int[positions];

                for (int i = 0; i < positions; i++)
                {
                    posCopy[i] = pattern[i];
                }

                // Firstly, check for black pegs by checking if the digits at the same indexes of both arrays are the exact same (ignoring zeros)
                // If they are, replace the digits in the arrays with zeros
                for (int i = 0; i < positions; i++)
                {
                    if (guess[i] == posCopy[i] && posCopy[i] != 0)
                    {
                        blackPegs++;
                        posCopy[i] = 0;
                    }
                }

                // Then check for white pegs by comparing each digit within the guess to each index of the pattern (checking if the same digit exists but not in the same index)
                // If the digit is found then replace both with zeros
                for (int i = 0; i < positions; i++)
                { 
                    for (int j = 0; j < positions; j++)
                    {
                        if (posCopy[j] == guess[i] && posCopy[j] != 0)
                        {
                            whitePegs++;
                            posCopy[i] = 0;
                        }
                    } 
                }

                Console.WriteLine("Black pegs: {0}", blackPegs);
                Console.WriteLine("White pegs: {0}", whitePegs);

                // Convert pegs from int to string via concatentation
                historyItem[1] = string.Empty + blackPegs;
                historyItem[2] = string.Empty + whitePegs;

                // Add the guess + black and white pegs to the queue
                Enqueue(history, historyItem);

                askInput = false;

                // If this statement is true then the player has correctly guessed the pattern
                if (positions == blackPegs) 
                { 
                    currentState = States.END;
                    guessCount = 0;
                    ClearGuesses();
                }

                // Else they should make another guess
                else 
                {
                    Console.WriteLine("\n--------------");
                    Console.WriteLine("Guess again...");
                    Console.WriteLine("--------------\n");

                    currentState = States.PLAYING;
                    guessCount = 0;
                    ClearGuesses();
                }
            }
            
            // Ask the user if they want to replay
            else if (currentState == States.END)
            {
                Console.WriteLine("\n------------");
                Console.WriteLine("GAME OVER...");
                Console.WriteLine("------------\n");

                Console.WriteLine("The secret pattern was: ");

                for (int i = 0; i < positions; i++)
                {
                    Console.Write("{0} ", pattern[i]);
                }

                Console.WriteLine("\n\nGuess History:");

                int j = 1;
                while (history.size > 0)
                {
                    Peek(history, j);
                    Dequeue(history);
                    j++;
                }

                Console.WriteLine("\n\nDo you want to replay?");
                Console.WriteLine(" 1. Yes");
                Console.WriteLine(" 2. No");

                askInput = false;

                if (Confirm() == true)
                {
                    currentState = States.START;
                }

                else
                {
                    currentState = States.QUIT;
                }
            }

            // Writing text to the screen to display necessary information whilst the player is playing
            else if (currentState == States.PLAYING)
            {
                if (guessCount < positions) // Player is still guessing
                {
                    Console.WriteLine("\nRange of numbers   : 1 to {0}", numbersMax);
                    Console.WriteLine("Amount of positions: {0}\n", positions);

                    if (history.size == 0)
                    {
                        Console.WriteLine("Player A has created a pattern of {0} size...\n", positions);
                    }

                    if (history.size > 0)
                    {
                        Console.WriteLine("Previous guesses: ");
                        Traversal(history);// write all of the contents of the queue
                        Console.WriteLine();
                    }

                    Console.Write("Position  : ");
                    for (int i = 0; i < positions; i++)
                    {
                        Console.Write("{0} ", i + 1);
                    }

                    Console.Write("\nGuess     : ");
                    for (int j = 0; j < positions; j++)
                    {
                        Console.Write("{0} ", guess[j]);
                    }

                    Console.WriteLine("\n\nMake guess for position: {0}", guessCount + 1);
                }

                else // Player has finished guessing
                {
                    Console.Clear();

                    Console.Write("Position  : ");
                    for (int i = 0; i < positions; i++)
                    {
                        Console.Write("{0} ", i + 1);
                    }

                    Console.Write("\nGuess     : ");
                    for (int j = 0; j < positions; j++)
                    {
                        Console.Write("{0} ", guess[j]);
                    }

                    Console.WriteLine("\n\nConfirm pattern?");
                    Console.WriteLine(" 1. Yes");
                    Console.WriteLine(" 2. No");

                    askInput = false;

                    // Submit the guess
                    if(Confirm() == true)
                    {
                        currentState = States.COMPARISON;
                    }

                    // Restart guessing and reset game state and clear arrays
                    else
                    {
                        Console.Clear();
                        currentState = States.PLAYING;
                        guessCount = 0;
                        ClearGuesses();
                    }
                }
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
        // Primarily used to see if the input meets a specific criteria: such as being an integer or between a specific range
        public void processInput(string input)
        {
            if (input == "rules" && currentState == States.START) { printRules(); }

            else if (input == "quit") { currentState = States.QUIT; }

            else if (input == "play" && currentState == States.START) 
            {
                currentState = States.NUMBERS;
                Console.Clear();
            }

            // Ensure that the user inputs a number within the control criteria
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

            // Ask user for pattern size and then generate a random pattern based on size
            else if (currentState == States.POSITIONS)
            {
                int num;
                bool isNumber = Int32.TryParse(input, out num);

                if (isNumber)
                {
                    if (num > 0)
                    {
                        positions = num;
                        pattern = new int[positions];
                        guess = new int[positions];

                        GetPattern();

                        currentState = States.PLAYING;
                    }

                    else {Console.WriteLine("\nINVALID INPUT - Input must be a number and greater than 1\n");}
                }

                else {Console.WriteLine("\nINVALID INPUT - Input must be a number and greater than 1\n");}
            }

            // Ensure guesses are greater than 0 and less than the upper limit digit
            else if (currentState == States.PLAYING)
            {
                int num;
                bool isNumber = Int32.TryParse(input, out num);

                if (guessCount <= positions)
                {
                    if (isNumber && num > 0 && num <= numbersMax)
                    {
                        guess[guessCount] = num;
                        guessCount++;
                    }

                    else
                    {
                        Console.WriteLine("Input must be a NUMBER between the range of 1 and {0}", numbersMax);
                    }

                }
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

        // Use random class to get a random number
        public int getNumber()
        {
            Random rand = new Random();
            int number = rand.Next(1, numbersMax+1);

            return number;
        }

        // Make random pattern of N size
        public void GetPattern()
        {
            for(int i=0; i<positions; i++)
            {
                int randomNumber = getNumber();
                pattern[i] = randomNumber;
            }
        }

        // Clear the array by setting all values within to 0
        public void ClearGuesses()
        {
            for (int i = 0; i < positions; i++)
            {
                guess[i] = 0;
            }

        }

        // Reusable code to take an input and use to assess a yes or no input
        public bool Confirm()
        {
            while (true)
            {
                string input = readInput();

                if (input == "yes" || input == "1")
                {
                    return true;
                }

                else if (input == "no" || input == "2")
                {
                    return false;
                }

                else
                {
                    Console.WriteLine("Invalid Input...");
                }
            }
        }
    }
}
