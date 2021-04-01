using System;

namespace mastermind
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MASTERMIND\n");

            Console.WriteLine("Rules:");
            Console.WriteLine(" - Player A will secretly select N pegs from a selection of M colours and position them in a set order to create a hidden pattern.");
            Console.WriteLine(" - Player B will then make a series of guesses to try to find out Player A's pattern.\n");

            Console.WriteLine(" - After confirming their turn, Player B will be able to see which of their guesses are correct via black and white pegs.");
            Console.WriteLine("    - Black: position and colour are both correct");
            Console.WriteLine("    - White: position is incorrect, but the colour exists somewhere within the pattern\n");


            Console.WriteLine(" - The program will take the role of Player A, and therefore the user is Player B.");
            Console.WriteLine(" - The game will end when either:");
            Console.WriteLine("    a. The user forfeits or exits the program.");
            Console.WriteLine("    b. The user wins the game.\n");


            Console.WriteLine("Input 'play' to begin...");
        }
    }
}
