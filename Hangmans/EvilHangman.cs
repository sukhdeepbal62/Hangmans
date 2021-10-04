using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangmans
{
    public class EvilHangman
    {
        public string SecretWord { get; set; }
        public int WordLength { get; set; }
        public int GuessesLeft { get; set; }

        private string original;
        private Random random;
        private List<string> words;

        //filtering words
        public EvilHangman(List<string> wordset, int wordLength, int guessesLeft)
        {
            random = new Random();
            words = wordset;
            WordLength = wordLength;
            SecretWord = GenerateNewWord();
            GuessesLeft = guessesLeft;
        }

        public string GenerateNewWord()
        {
            int index = random.Next(words.Count());
            while (!(words[index].Length >= WordLength && words[index].Length <= WordLength + 5))
            {
                index = random.Next(words.Count());
            }
            original = words[index];
            WordLength = words[index].Length;
            return GetSecretWord(WordLength);
        }

        public bool GuessMade(char guess)
        {
            bool found = false;
            string result = "";
            for(int index = 0; index < original.Length; index++)
            {
                if (SecretWord[index * 2] != '_')
                {
                    result += SecretWord[index * 2];
                }
                else if (original[index] == guess)
                {
                    result += original[index];
                    found = true;
                }
                else
                {
                    result += "_";
                }
                if(index < original.Length - 1 )
                {
                    result += " ";
                }
            }
            if(found)
            {
                SecretWord = result;
            }
            GuessesLeft--;
            return found;
        }

        public string PrepareAnswer()
        {
            string result = "";
            for (int index = 0; index < original.Length; index++)
            {
                result += original[index];
                if (index < original.Length - 1)
                {
                    result += " ";
                }
            }
            return result;
        }

        public string GetSecretWord(int letters)
        {
            string dashes = "";
            for (int i = 0; i < letters - 1; i++)
            {
                dashes += "_ ";
            }
            dashes += "_";
            return dashes;
        }

        public bool IsRight(string result)
        {
            return original.Equals(result);
        }
    }
}