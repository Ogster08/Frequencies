using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frequencies
{
    internal class Playfair
    {
        private string text;
        private string decryption = "";
        private string key = "";
        private Ngrams ngrams;

        public string Decryption { get { return decryption; } }
        public string Key { get { return key; } }

        public Playfair(string Text)
        {
            text = Text;
            ngrams = new("english_quadgrams.txt");
        }

        public Playfair(string Text, Ngrams ngrams)
        {
            text = Text;
            this.ngrams = ngrams;
        }

        public void solve()
        {
            if (text.Length % 2 != 0)
            {
                text += "x"; // pad with X if odd length
            }
            Byte[] textAsBytes = new Byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                textAsBytes[i] = (Byte)(text[i] - 'a');
            }
            // Implementation of Playfair cipher solver would go here
            //Byte[,] square = new Byte[5,5];
            Byte[] alphabet = new byte[25];
            for (Byte i = 0; i < 9; i++) { alphabet[i] = i; }
            for (Byte i = 9; i < 25; i++) { alphabet[i] = (Byte)(i + 1); }
            alphabet = new byte[] { 6, 14, 11, 3, 4, 13, 0, 1, 2, 5, 7, 8, 10, 12, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
            Random rnd = new Random();

            int max_repeats = 1000;
            int repeats = 0;
            int max_count = 100000;
            int count = 0;

            double currentScore;
            double possibleScore = -999999999;
            double bestScore = possibleScore;

            Byte[,] bestSquare = new Byte[5, 5];
            Byte[,] possibleSquare = new Byte[5, 5];

            Byte[] currentDecryption = new Byte[text.Length];
            Byte[] possibleDecryption = new Byte[text.Length];
            Byte[] bestDecryption = new Byte[text.Length];

            Byte[,] possibleSquareMapping = new Byte[26, 2];

            while (count < max_count && repeats < max_repeats)
            {
                // get Playfair square
                //rnd.Shuffle(alphabet);
                for (Byte i = 0; i < 5; i++)
                {
                    for (Byte j = 0; j < 5; j++)
                    {
                        Byte letter = alphabet[i * 5 + j];
                        possibleSquare[i, j] = letter;
                        possibleSquareMapping[letter,0] = i;
                        possibleSquareMapping[letter,1] = j;
                    }
                }

                for (int i = 0; i < text.Length; i += 2)
                {
                    Byte x = textAsBytes[i];
                    Byte y = textAsBytes[i + 1];
                    if (possibleSquareMapping[x, 0] == possibleSquareMapping[y, 0])
                    {
                        if (possibleSquareMapping[x, 1] == 0)
                        {
                            currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0], 4];
                        }
                        else
                        {
                            currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0], possibleSquareMapping[x, 1] - 1];
                        }

                        if (possibleSquareMapping[y, 1] == 0)
                        {
                            currentDecryption[i + 1] = possibleSquare[possibleSquareMapping[y, 0], 4];
                        }
                        else
                        {
                            currentDecryption[i + 1] = possibleSquare[possibleSquareMapping[y, 0], possibleSquareMapping[y, 1] - 1];
                        }
                    }
                    else if (possibleSquareMapping[x, 1] == possibleSquareMapping[y, 1])
                    {
                        if (possibleSquareMapping[x, 0] == 0)
                        {
                            currentDecryption[i] = possibleSquare[4, possibleSquareMapping[x, 1]];
                        }
                        else
                        {
                            currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0] - 1, possibleSquareMapping[x, 1]];
                        }

                        if (possibleSquareMapping[y, 0] == 0)
                        {
                            currentDecryption[i + 1] = possibleSquare[4, possibleSquareMapping[y, 1]];
                        }
                        else
                        {
                            currentDecryption[i + 1] = possibleSquare[possibleSquareMapping[y, 0] - 1, possibleSquareMapping[y, 1]];
                        }
                    }
                    else
                    {
                        currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0], possibleSquareMapping[y, 1]];
                        currentDecryption[i + 1] = possibleSquare[possibleSquareMapping[y, 0], possibleSquareMapping[x, 1]];
                    }

                }


                possibleScore = ngrams.score(currentDecryption);
                possibleDecryption = currentDecryption.Clone() as Byte[];

                bool improvement = true;
                while (improvement)
                {
                    improvement = false;
                    for (int a = 0; a < 25; a++)
                    {
                        for (int b = a + 1; b < 25; b++)
                        {
                            count++;
                            (possibleSquare[a/5, a%5], possibleSquare[b/5, b%5]) = (possibleSquare[b / 5, b % 5], possibleSquare[a / 5, a % 5]); // swap
                            // compute decryption
                            for (int i = 0; i < text.Length; i += 2)
                            {
                                Byte x = textAsBytes[i];
                                Byte y = textAsBytes[i + 1];
                                if (possibleSquareMapping[x,0] == possibleSquareMapping[y,0])
                                {
                                    if (possibleSquareMapping[x,1] == 0)
                                    {
                                        currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0], 4];
                                    }
                                    else
                                    {
                                        currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0], possibleSquareMapping[x, 1] - 1];
                                    }

                                    if (possibleSquareMapping[y, 1] == 0)
                                    {
                                        currentDecryption[i + 1] = possibleSquare[possibleSquareMapping[y, 0], 4];
                                    }
                                    else
                                    {
                                        currentDecryption[i + 1] = possibleSquare[possibleSquareMapping[y, 0], possibleSquareMapping[y, 1] - 1];
                                    }
                                }
                                else if (possibleSquareMapping[x, 1] == possibleSquareMapping[y, 1])
                                {
                                    if (possibleSquareMapping[x, 0] == 0)
                                    {
                                        currentDecryption[i] = possibleSquare[4, possibleSquareMapping[x, 1]];
                                    }
                                    else
                                    {
                                        currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0] - 1, possibleSquareMapping[x, 1]];
                                    }

                                    if (possibleSquareMapping[y, 0] == 0)
                                    {
                                        currentDecryption[i + 1] = possibleSquare[4, possibleSquareMapping[y, 1]];
                                    }
                                    else
                                    {
                                        currentDecryption[i + 1] = possibleSquare[possibleSquareMapping[y, 0] - 1, possibleSquareMapping[y, 1]];
                                    }
                                }
                                else
                                {
                                    currentDecryption[i] = possibleSquare[possibleSquareMapping[x, 0], possibleSquareMapping[y, 1]];
                                    currentDecryption[i+1] = possibleSquare[possibleSquareMapping[y, 0], possibleSquareMapping[x, 1]];
                                }
                                
                            }


                            currentScore = ngrams.score(currentDecryption);
                            if (currentScore > possibleScore)
                            {
                                possibleScore = currentScore;
                                possibleSquare = possibleSquare.Clone() as Byte[,];
                                Array.Copy(currentDecryption, possibleDecryption, currentDecryption.Length);
                                improvement = true;
                            }
                            else
                            {
                                // swap back
                                (possibleSquare[a / 5, a % 5], possibleSquare[b / 5, b % 5]) = (possibleSquare[b / 5, b % 5], possibleSquare[a / 5, a % 5]); // swap
                            }
                        }
                    }
                }

                if (possibleScore > bestScore)
                {
                    bestScore = possibleScore;
                    bestSquare = possibleSquare.Clone() as Byte[,];
                    Array.Copy(possibleDecryption, bestDecryption, bestDecryption.Length);
                    repeats = 0;
                }
                else
                {
                    repeats++;
                }
            }
            char[] key = new char[25];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    key[i * 5 + j] = (char)(bestSquare[i, j] + 'a');
                }
            }

            char[] decryptionChars = new char[bestDecryption.Length];
            for (int i = 0; i < bestDecryption.Length; i++)
            {
                decryptionChars[i] = (char)(bestDecryption[i] + 'a');
            }

            Debug.WriteLine(count);
            Debug.WriteLine(repeats);
            Debug.WriteLine(bestScore / (double)text.Length);

            this.key = new string(key);
            this.decryption = new string(decryptionChars);
        }
    }
}
