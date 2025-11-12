using System.Collections.Generic;
using System;

namespace frequencies
{
    internal class Substitutian
    {
        private string text;
        private string decryption = "";
        private string key = "";
        private Ngrams ngrams;

        public string Decryption { get { return decryption; } }
        public string Key { get { return key; } }

        public Substitutian(string Text)
        {
            text = Text;
            ngrams = new("english_quadgrams.txt");

        }

        public Substitutian(string Text, Ngrams ngrams)
        {
            text = Text;
            this.ngrams = ngrams;
        }

        public void solve()
        {
            Byte[] textBytes = new Byte[text.Length];
            for (int i = 0; i < text.Length; i++) { textBytes[i] = (Byte)(text[i] - 'a'); }
            Byte[] alphabet = new byte[26];
            for (Byte i = 0; i < 26; i++) { alphabet[i] = i; }
            int n = 50_000;
            int r = 3;
            int repeats = 0;
            int x = 0;


            char[] bestDecryption = text.ToCharArray();
            Byte[] CurrentDecryption = new Byte[text.Length];
            Byte[] possibleDecryption = new Byte[text.Length];
            Byte[] bestKey = (Byte[])alphabet.Clone();
            Byte[] currentKey = (Byte[])alphabet.Clone();
            Byte[] possibleKey = (Byte[])alphabet.Clone();
            
            Random rnd = new Random();

            int[][] charPositions = new int[26][];
            for (int i = 0; i < 26; i++) 
            {
                List<int> positions = new();
                for (int j = 0; j < text.Length; j++)
                {
                    if (textBytes[j] == i) { positions.Add(j); }
                }
                charPositions[i] = positions.ToArray();
            }


            double CurrentScore;
            double possibleScore = -9999999999;
            double bestScore = possibleScore;


            while ((repeats < r) && (x < n))
            {
                possibleKey = (Byte[])alphabet.Clone();
                rnd.Shuffle(possibleKey);
                for (int i = 0; i < possibleDecryption.Length; i++)
                {
                    possibleDecryption[i] = possibleKey[textBytes[i]];
                }
                CurrentScore = ngrams.score(possibleDecryption);
                bool improvemnt = true;
                while (improvemnt)
                {
                    improvemnt = false;

                    for (int a = 0; a < 25; a++)
                    {
                        for (int b = a+1; b < 26; b++)
                        {
                            (possibleKey[a], possibleKey[b]) = (possibleKey[b], possibleKey[a]);

                            foreach (int pos in charPositions[a]) { possibleDecryption[pos] = possibleKey[a]; }
                            foreach (int pos in charPositions[b]) { possibleDecryption[pos] = possibleKey[b]; }

                            possibleScore = ngrams.score(possibleDecryption);
                            if (possibleScore > CurrentScore)
                            {
                                CurrentScore = possibleScore;
                                Array.Copy(possibleDecryption, CurrentDecryption, text.Length);
                                Array.Copy(possibleKey, currentKey, 26);
                                improvemnt = true;
                            }
                            else
                            {
                                (possibleKey[a], possibleKey[b]) = (possibleKey[b], possibleKey[a]);
                                foreach (int pos in charPositions[a]) { possibleDecryption[pos] = possibleKey[a]; }
                                foreach (int pos in charPositions[b]) { possibleDecryption[pos] = possibleKey[b]; }
                            }
                            x++;
                        }
                    }  
                }

                if (CurrentScore > bestScore) 
                { 
                    bestScore = CurrentScore;
                    repeats = 0;
                    Array.Copy(currentKey, bestKey, 26);
                    Array.Copy(CurrentDecryption, bestDecryption, text.Length);

                }
                else if (CurrentScore == bestScore)
                {
                    repeats++;
                }

            }
            char[] oppositeKey = new char[26];
            for (int i = 0; i < 26; i++) { oppositeKey[bestKey[i]] = Convert.ToChar(i + 'a'); }

            for (int index = 0; index < text.Length; index++)
            {
                bestDecryption[index] = (char)(bestKey[textBytes[index]] + 'a');
            }

            this.decryption = new string(bestDecryption);
            this.key = new string(oppositeKey) + (ngrams.score(this.decryption) / text.Length).ToString();
        }
    }
}