/*  CSC 446
    Assignment 1 [  Lexical Analyzer    ]
    Ben Morse
    1/24/2023
    Due: 1/30/2023
    */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LexAZ 
{
    public class LexicalAnalyzer
    {
        // Enumerated data type for tokens
        public enum TokenType
        {
            PROGRAM, PROCEDURE, FUNCTION, VAR, BEGIN, END, IF, THEN, ELSE, WHILE, DO, REAL,
            INTEGER, CHAR, ID, NUM, RELOP, ADDOP, MULOP, ASSIGNOP, LEFT_PAREN, RIGHT_PAREN,
            LEFT_BRACE, RIGHT_BRACE, COMMA, COLON, SEMICOLON, PERIOD, LITERAL, EXCLAM, EOF
        }
        private readonly Dictionary<string, TokenType> ReservedWords = new Dictionary<string, TokenType>
        {
            { "program", TokenType.PROGRAM },
            { "procedure", TokenType.PROCEDURE },
            { "function", TokenType.FUNCTION },
            { "var", TokenType.VAR },
            { "begin", TokenType.BEGIN },
            { "end", TokenType.END },
            { "if", TokenType.IF },
            { "then", TokenType.THEN },
            { "else", TokenType.ELSE },
            { "while", TokenType.WHILE },
            { "do", TokenType.DO },
            { "real", TokenType.REAL },
            { "integer", TokenType.INTEGER },
            { "char", TokenType.CHAR }
        };


        // Global variables
        public TokenType Token { get; private set; }
        public string Lexeme { get; private set; }
        public int Value { get; private set; }
        public double ValueR { get; private set; }
        public string Literal { get; private set; }

        // Input source program
        private string _source;
        private int _index;

        // Constructor
        public LexicalAnalyzer(string source)
        {
            _source = source;
            _index = 0;
        }

        // Method to retrieve the next token from the input source program
        public void GetNextToken()
        {
            // Skip whitespaces and comments
            while (_index < _source.Length && (char.IsWhiteSpace(_source[_index]) || _source[_index] == '{'))
            {
                if (_source[_index] == '{')
                {
                    // Skip comment
                    while (_source[_index] != '}')
                    {
                        _index++;
                    }
                    _index++;
                }
                else
                {
                    _index++;
                }
            }

            // Check for end of file
            if (_index >= _source.Length)
            {
                Token = TokenType.EOF;
                return;
            }            
            // Check for numbers
            else if (char.IsDigit(_source[_index]))
            {
                HandleNumbers();
            }
            // Check for relational operators
            else if (_source[_index] == '=' || _source[_index] == '<' || _source[_index] == '>')
            {
                HandleRelationalOperators();
            }
            // Check for mulop's
            else if (
                        _source[_index] == '*' || _source[_index] == '/' ||
                       (_source[_index] == 'd' && _source[_index + 1] == 'i' && _source[_index + 2] == 'v') ||
                       (_source[_index] == 'm' && _source[_index + 1] == 'o' && _source[_index + 2] == 'd') ||
                       (_source[_index] == 'a' && _source[_index + 1] == 'n' && _source[_index + 2] == 'd')
                     )
            {
                HandleMulOps();
            }
            // Check for reserved words and identifiers
            else if (char.IsLetter(_source[_index]))
            {
                HandleIdentifiers();
            }
            // Check for addop's
            else if (_source[_index] == '+' || _source[_index] == '-' || (_source[_index] == 'o' && _source[_index + 1] == 'r'))
            {
                HandleAddOps();
            }                     
            // Check for assignop
            else if (_source[_index] == ':' && _source[_index + 1] == '=')
            {
                HandleAssignOp();
            }
            // Check for symbols
            else if (
                     _source[_index] == '(' || _source[_index] == ')' || _source[_index] == '{' || _source[_index] == '}' || 
                     _source[_index] == ',' || _source[_index] == ':' || _source[_index] == ';' || _source[_index] == '.' || 
                     _source[_index] == '\'' || _source[_index] == '!'
                    )
            {
                HandleSymbols();
            }
            // Invalid token
            else
            {
                throw new Exception("Invalid token at index " + _index);
            }
        }


        // Methods or properties to handle each type of symbol
        // Example method for handling identifiers
        private void HandleIdentifiers()
        {/*     HandleIdentifiers() method first saves the starting index of the lexeme in the 
          *     input source program. Then, it uses an if statement to check for a letter which 
          *     is the first character of the identifier, if a letter is found then it enters into 
          *     a while loop to check for letters and digits, if any more letters or digits are found 
          *     it continues to increment the index, it stops when there are no more letters or digits 
          *     found. Then it sets the Lexeme to the substring of the input source program starting 
          *     from the starting index and ending at the current index, it makes all letters lower 
          *     case to make the identifier not case sensitive.     */

            // Save starting index for lexeme
            int startIndex = _index;

            // Check for letter
            if (char.IsLetter(_source[_index]))
            {
                _index++;
                // Check for letters and digits
                while (_index < _source.Length && (char.IsLetter(_source[_index]) || char.IsDigit(_source[_index])))
                {
                    _index++;
                }
                Lexeme = _source.Substring(startIndex, _index - startIndex);
                Lexeme = Lexeme.ToLower();
                if (ReservedWords.ContainsKey(Lexeme))
                {
                    Token = ReservedWords[Lexeme];
                }
                else
                {
                    Token = TokenType.ID;
                }
            }
            else
            {
                throw new Exception("Invalid identifier at index " + _index);
            }
        }


        // Example method for handling numbers
        private void HandleNumbers()
        {/*     HandleNumbers() method first saves the starting index of the lexeme in the 
          *         input source program. Then, it uses an if statement to check for a digit 
          *         which is the first character of the number. If a digit is found then it enters 
          *         into a while loop to check for more digits, if any more digits are found it 
          *         continues to increment the index, it stops when there are no more digits found. 
          *         Then it checks for an optional fraction, if it is found it enters into another while 
          *         loop to check for more digits after the fraction point, then the Token      */

            // Save starting index for lexeme
            int startIndex = _index;

            // Check for digits
            if (char.IsDigit(_source[_index]))
            {
                _index++;
                // Check for more digits
                while (_index < _source.Length && char.IsDigit(_source[_index]))
                {
                    _index++;
                }
                // Check for optional fraction
                if (_index < _source.Length && _source[_index] == '.')
                {
                    _index++;
                    // Check for digits after '.'
                    while (_index < _source.Length && char.IsDigit(_source[_index]))
                    {
                        _index++;
                    }
                    Token = TokenType.NUM;
                    ValueR = double.Parse(_source.Substring(startIndex, _index - startIndex));
                    Lexeme = _source.Substring(startIndex, _index - startIndex);
                }
                else
                {
                    Token = TokenType.NUM;
                    Value = int.Parse(_source.Substring(startIndex, _index - startIndex));
                    Lexeme = _source.Substring(startIndex, _index - startIndex);
                }
            }
            else
            {
                throw new Exception("Invalid number at index " + _index);
            }
        }


        // Method or property to handle case-insensitivity for Pascal identifiers
        private string ToLower(string source)
        {/*     This method will convert a given string to lowercase, it accepts a string parameter 
          *        as input and returns the same string in lowercase. I could also use the 
          *     .ToLower() method directly in the HandleIdentifiers() method.   */
            return source.ToLower();
        }




        private void HandleRelationalOperators()
        {/*  HandleRelationalOperators() method first saves the starting index of the lexeme 
         *  in the input source program. Then, it uses a series of if-else statements to check 
         *  for the different relational operators (i.e. '=', '<', '>', '<>', '<=', '>='). If a 
         *  relational operator is found, the method sets the Token to RELOP, and sets the Lexeme 
         *  to the substring of the input source program starting from the starting index and ending 
         *  at the current index.       */

            // Save starting index for lexeme
            int startIndex = _index;

            // Check for '=' or '<' or '>'
            if (_source[_index] == '=')
            {
                _index++;
                Token = TokenType.RELOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == '<')
            {
                _index++;
                if (_source[_index] == '>')
                {
                    _index++;
                    Token = TokenType.RELOP;
                    Lexeme = _source.Substring(startIndex, _index - startIndex);
                }
                else if (_source[_index] == '=')
                {
                    _index++;
                    Token = TokenType.RELOP;
                    Lexeme = _source.Substring(startIndex, _index - startIndex);
                }
                else
                {
                    Token = TokenType.RELOP;
                    Lexeme = _source.Substring(startIndex, _index - startIndex);
                }
            }
            else if (_source[_index] == '>')
            {
                _index++;
                if (_source[_index] == '=')
                {
                    _index++;
                    Token = TokenType.RELOP;
                    Lexeme = _source.Substring(startIndex, _index - startIndex);
                }
                else
                {
                    Token = TokenType.RELOP;
                    Lexeme = _source.Substring(startIndex, _index - startIndex);
                }
            }
        }


        private void HandleAddOps()
        {/*     HandleAddOps() method first saves the starting index of the lexeme in the 
          *     input source program. Then, it uses a series of if-else statements to check 
          *     for the different addop's (i.e. '+', '-', 'or'). If an addop is found, the method 
          *     sets the Token to ADDOP, and sets the Lexeme to the substring of the input source 
          *     program starting from the starting index and ending at the current index. And if the 
          *     addop is not found it throws an exception. */

            // Save starting index for lexeme
            int startIndex = _index;

            // Check for '+' or '-' or 'or'
            if (_source[_index] == '+')
            {
                _index++;
                Token = TokenType.ADDOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == '-')
            {
                _index++;
                Token = TokenType.ADDOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == 'o' && _source[_index + 1] == 'r') /*else if (_source.Substring(_index, 2) == "or")*/
            {
                _index += 2;
                Token = TokenType.ADDOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else
            {
                throw new Exception("Invalid addop at index " + _index);
            }
        }



        private void HandleMulOps()
        {/*     HandleMulOps() method first saves the starting index of the lexeme in the 
          *     input source program. Then, it uses a series of if-else statements to check 
          *     for the different mulop's (i.e. '*', '/', 'div', 'mod', 'and'). If a mulop 
          *     is found, the method sets the Token to MULOP, and sets the Lexeme to the substring 
          *     of the input source program starting from the starting index and ending at the 
          *     current index. And if the mulop is not found it throws an exception.    */

            // Save starting index for lexeme
            int startIndex = _index;

            // Check for '*' or '/' or 'div' or 'mod' or 'and'
            if (_source[_index] == '*')
            {
                _index++;
                Token = TokenType.MULOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == '/')
            {
                _index++;
                Token = TokenType.MULOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == 'd' && _source[_index + 1] == 'i' && _source[_index + 2] == 'v')
            {
                _index += 3;
                Token = TokenType.MULOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == 'm' && _source[_index + 1] == 'o' && _source[_index + 2] == 'd')
            {
                _index += 3;
                Token = TokenType.MULOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == 'a' && _source[_index + 1] == 'n' && _source[_index + 2] == 'd')
            {
                _index += 3;
                Token = TokenType.MULOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else
            {
                throw new Exception("Invalid mulop at index " + _index);
            }
        }



        private void HandleAssignOp()
        {/*     HandleAssignOp() method first saves the starting index of the lexeme in the input 
          *     source program. Then, it uses an if statement to check for the assignop (i.e. ':='). 
          *     If the assignop is found, the method sets the Token to ASSIGNOP, and sets the Lexeme 
          *     to the substring of the input source program starting from the starting index and ending 
          *     at the current index. And if the assignop is not found it throws an exception.  */

            // Save starting index for lexeme
            int startIndex = _index;

            // Check for ':='
            if (_source[_index] == ':' && _source[_index + 1] == '=')
            {
                _index += 2;
                Token = TokenType.ASSIGNOP;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else
            {
                throw new Exception("Invalid assignop at index " + _index);
            }
        }


        private void HandleSymbols()
        {/*     HandleSymbols() method first saves the starting index of the lexeme in the 
          *     input source program. Then, it uses a series of if-else statements to check 
          *     for the different symbols (i.e. '(', ')', '{', '}', ',', ':', ';', '.', '''). 
          *     If a symbol is found, the method sets the Token to the appropriate token type, 
          *     and sets the Lexeme to the substring of the input source program starting from 
          *     the starting index and ending at the current index.     */

            // Save starting index for lexeme
            int startIndex = _index;

            // Check for symbols
            if (_source[_index] == '(')
            {
                _index++;
                Token = TokenType.LEFT_PAREN;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == ')')
            {
                _index++;
                Token = TokenType.RIGHT_PAREN;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == '{')
            {
                _index++;
                Token = TokenType.LEFT_BRACE;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == '}')
            {
                _index++;
                Token = TokenType.RIGHT_BRACE;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == ',')
            {
                _index++;
                Token = TokenType.COMMA;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == ':')
            {
                _index++;
                Token = TokenType.COLON;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == ';')
            {
                _index++;
                Token = TokenType.SEMICOLON;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == '.')
            {
                _index++;
                Token = TokenType.PERIOD;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }
            else if (_source[_index] == '\'')
            {
                Token = TokenType.LITERAL;
                Lexeme = "'";
                _index++;
                while (_source[_index] != '\'')
                {                    
                    Lexeme = string.Concat(Lexeme, _source[_index]);
                    _index++;
                }
                _index++;
                Lexeme = string.Concat(Lexeme, "'");
            }
           /* else if (_source[_index] == '!')
            {
                _index++;
                Token = TokenType.EXCLAM;
                Lexeme = _source.Substring(startIndex, _index - startIndex);
            }*/
            else
            {
                throw new Exception("Invalid symbol at index " + _index);
            }
        }

    }

}

