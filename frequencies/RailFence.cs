using System.Collections.Generic;
using System.Linq;
using System;

namespace frequencies
{
    internal class RailFence
    {
        private string text;
        private string decryption = "";
        private string key = "";

        public string Decryption { get { return decryption; } }
        public string Key { get { return key; } }

        public RailFence(string Text)
        {
            text = Text;
        }

        public void solve()
        {
            List<Tuple<string, int>> possibleDecryptions = new();
            for (int keyLength = 2; keyLength <= 15; keyLength++)
            {
                char[][] railFence = new char[keyLength][];
                for (int i = 0; i < railFence.Length; i++) { railFence[i] = new char[text.Length]; }
                int[] indexes = new int[] { -1, -1 };
                bool countUp = true;
                Tuple<int[], bool> nextSet;
                for (int i = 0; i < text.Length; i++)
                {
                    nextSet = NextRailFence(indexes, keyLength, countUp);
                    indexes = nextSet.Item1;
                    countUp = nextSet.Item2;
                    railFence[indexes[0]][indexes[1]] = Convert.ToChar("|");
                }

                int count = 0;
                for (int x = 0; x < keyLength; x++)
                {
                    for (int y = 0; y < text.Length; y++)
                    {
                        if (railFence[x][y] == Convert.ToChar("|"))
                        {
                            railFence[x][y] = text[count++];
                        }
                    }
                }

                char[] possibleDecryption = new char[text.Length];
                indexes = new int[] { -1, -1 };
                countUp = true;
                for (int i = 0; i < text.Length; i++)
                {
                    nextSet = NextRailFence(indexes, keyLength, countUp);
                    indexes = nextSet.Item1;
                    countUp = nextSet.Item2;
                    possibleDecryption[i] = railFence[indexes[0]][indexes[1]];
                }

                Tuple<string, int> toAdd = new Tuple<string, int>(string.Join("", possibleDecryption), keyLength);
                possibleDecryptions.Add(toAdd);
            }

            Ngrams ngrams = new("english_quadgrams.txt");
            double[] scores = new double[possibleDecryptions.Count];

            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = ngrams.score(string.Join("", possibleDecryptions[i].Item1.Where(char.IsLetter).Select(c => char.ToLower(c)).ToArray()));
            }

            Tuple<string, int> decryptionKey = possibleDecryptions[Array.IndexOf(scores, scores.Max())];
            decryption = decryptionKey.Item1;
            key = decryptionKey.Item2.ToString();
        }

        public static Tuple<int[], bool> NextRailFence(int[] currentRailFence, int keyLength, bool countUp)
        {
            int[] railFence = new int[2];
            if (countUp & (currentRailFence[0] + 1 == keyLength)) { countUp = false; }
            if (!countUp & currentRailFence[0] == 0) { countUp = true; }
            if (countUp) { railFence[0] = currentRailFence[0] + 1; } else { railFence[0] = currentRailFence[0] - 1; }
            railFence[1] = currentRailFence[1] + 1;

            return Tuple.Create(railFence, countUp);
        }
    }
}