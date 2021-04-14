using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZConsole
{
    public static class Console
    {
        private static Regex _consoleColorMatch = new Regex("(?<=[^\\\\]|^)(\\[([a-zA-Z]*):([^\\[\\]]*)\\])");

        public static void WriteLine(string text)
        {
            Write(text + "\n");
        }
        public static void WriteLine(params string[] text)
        {
            foreach (var s in text)
            {
                Write(s + "\n");
            }
        }
        
        public static void Write(string text)
        {
            if (!_consoleColorMatch.IsMatch(text))
            {
                System.Console.Write(text);
            }
            else
            {
                ProcessTextToWrite(text);
            }
        }
        
        private static void ProcessTextToWrite(string text)
        {
            // Keep a back-up of the current foreground colour, as we'll revert this page.
            ConsoleColor resetColor = System.Console.ForegroundColor;

            // Pattern to be used to match the {Color:} format
            
            // Find the first match of the pattern, if any.
            Match match = _consoleColorMatch.Match(text);

            int lastStringEnd = 0;

            while (match.Success)
            {
                // If we have some text to write (inbetween detections) we can use Console.Write
                if (text.Length > lastStringEnd)
                {
                    Console.Write(text.Substring(lastStringEnd, match.Index - lastStringEnd));
                    lastStringEnd = match.Index + match.Length;
                }

                // Strip the curly brackets.
                string stringWithTrimmedBrackets = match.Value.Substring(1, match.Value.Length - 2);

                // Check to see if there is a colon present.
                int indexOfColon = stringWithTrimmedBrackets.IndexOf(':');

                if (indexOfColon > -1)
                {
                    /*
                     * Logic
                     * 1) Separate the colour from the word/phrase.
                     * 2) back-up the last foreground colour.
                     * 3) override the foreground with the colour which was specified in the curly brackets (after parsing it).
                     * 4) write the text.
                     * 5) revert colour.
                     */

                    string colorCode = stringWithTrimmedBrackets.Substring(0, indexOfColon);
                    string sentenceToColor = stringWithTrimmedBrackets.Substring(indexOfColon + 1);

                    ConsoleColor temp = System.Console.ForegroundColor;
                    Enum.TryParse(colorCode, out ConsoleColor colour);

                    System.Console.ForegroundColor = colour;
                    Console.Write(sentenceToColor);
                    System.Console.ForegroundColor = temp;
                }
                else
                {
                    // If there isn't a colon.

                    // Parse the colour.
                    Enum.TryParse(stringWithTrimmedBrackets, out ConsoleColor colour);

                    // Set the colour.
                    System.Console.ForegroundColor = colour;
                }

                // Check to see if there are any more matches.
                match = match.NextMatch();
            }

            // Print out any remaining characters.
            System.Console.Write(text.Substring(lastStringEnd));

            // Put the original colour back.
            System.Console.ForegroundColor = resetColor;
        }
    }
}