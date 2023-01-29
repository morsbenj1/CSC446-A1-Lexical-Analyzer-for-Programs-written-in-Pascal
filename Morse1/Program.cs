using System;

namespace LexAZ
{
    public class Program
    {
        static void Main(string[] args)
        {/*     After reading the file and parsing it, the program prompts the user to run another 
          *     file or not. If the user enters "n", the program breaks out of the loop and exits. 
          *     If the user enters "y" or anything else, the loop continues and the program prompts 
          *     the user for another file name. */
            while (true)
            {
                Console.WriteLine("************************************************\n" +
                                  "Default test programs in this project folder: \n" +
                                  "     t1.txt ... basic test\n" +
                                  "     t2.txt ... basic math statements\n" +
                                  "     t3.txt ... string literals\n" +
                                  "     t4.txt ... comments test\n" +
                                  "     t5.txt ... div & mod\n" +
                                  "     t6.txt ... relational operators ( > < <> !=)\n" +
                                  "______________________________________");
                Console.WriteLine("Please enter the name of the test file: ");
                string fileName = Console.ReadLine();
                Console.WriteLine("************************************************");
                FileReader fileReader = new FileReader();
                string source = fileReader.ReadFile(fileName);

                if (string.IsNullOrEmpty(source))
                {
                    Console.WriteLine("No source code found.");
                }
                else
                {
                    LexAZ.LexicalAnalyzer lexer = new LexAZ.LexicalAnalyzer(source);

                    while (lexer.Token != LexicalAnalyzer.TokenType.EOF)
                    {
                        lexer.GetNextToken();
                        Console.WriteLine("Token: ".PadRight(8) + lexer.Token.ToString().PadRight(12) + " Lexeme: ".PadRight(8) + lexer.Lexeme.PadRight(10));
                    }
                }
                Console.WriteLine("Do you want to run another file? (y/n)");
                string choice = Console.ReadLine();
                if (choice == "n")
                {
                    break;
                }
            }
        }
    }
}
