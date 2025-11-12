using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace frequencies
{
    internal class Vigenere
    {

        private string text;
        private string decryption = "";
        private StringBuilder key = new StringBuilder();

        public string Decryption { get { return decryption; } }
        public string Key { get { return key.ToString(); } }

        public Vigenere(string text)
        {
            this.text = text;
        }

        public void solve()
        {
            //////////get key length//////////
            float[] keyScores = new float[14];

            //try key lengths from 2 to 15
            for (int n = 2; n <= 15; n++)
            {
                //index of coincidence calculation
                string[] sequences = TextToArray(text, n);
                float[] scores = new float[sequences.Length];
                foreach (var item in sequences)
                {
                    Dictionary<string, int> frequencies = CaesarSolver.TextFrequency(item);
                    int[] occurances = new int[26];
                    for (int i = 0; i < occurances.Length; i++)
                    {
                        int f = frequencies.Values.ToArray()[i];
                        occurances[i] = f * (f - 1);
                    }
                    float occurancesSum = occurances.Sum();
                    float lengthCalc = item.Length * (item.Length - 1);
                    scores[Array.IndexOf(sequences, item)] = occurancesSum / lengthCalc;
                }
                //add to array
                keyScores[n - 2] = scores.Sum() / scores.Length;
            }

            List<float> possibleKeys = new();
            foreach (var key in keyScores) { if (key >= 0.055) { possibleKeys.Add(key + Array.IndexOf(keyScores, key) + 2); } }
            int keyLength = Array.IndexOf(keyScores, keyScores.Max()) + 2;

            //////////list text//////////

            string[] solveArray = TextToArray(text, keyLength);

            //////////solve cipthers//////////
            //create arrays for the possible keys and  decryption
            int[][] keysPos = new int[keyLength][]; //each part of array will contain 2 possible keys for that part of the cipher
            string[] decryptionArray = new string[keyLength];

            //looping through the text array to solve each caesar
            for (int i = 0; i < keyLength; i++)
            {
                CaesarSolver ceaserSolver = new(solveArray[i]);
                ceaserSolver.solve();

                //add possible keys and the decryprtion to the relavent arrays
                keysPos[i] = ceaserSolver.Key;
                decryptionArray[i] = ceaserSolver.Decryption;
            }

            //creating every permutation of each possible key for each part of the cipher
            int[][] keysPermutations = new int[Convert.ToInt32(Math.Pow(2, keyLength))][];
            for (int i = 0; i <= ~(-1 << keyLength); i++)
            {
                //using binary indexes for permutations
                string s = Convert.ToString(i, 2).PadLeft(keyLength, '0');
                int[] permutation = new int[s.Length];
                for (int x = 0; x < s.Length; x++)
                {
                    int index = int.Parse(s[x].ToString());
                    permutation[x] = keysPos[x][index];
                }
                keysPermutations[i] = permutation;
            }

            //setting the keys that are the most likely to be right
            int[] keys = new int[keyLength];
            for (int i = 0; i < keysPos.Length; i++) { keys[i] = keysPos[i][0]; }

            ////////combine the decryption arrays together e.g. 1,4,7  2,5,8  3,6,9  => 1,2,3,4,5,6,7,8,9////////
            List<string> fullDecryption = new();

            for (int i = 0; i < decryptionArray[0].Length; i++)
            {
                foreach (var item in decryptionArray)
                {
                    //try needed because not all decryption parts are the same size
                    try { fullDecryption.Add(item[i].ToString()); }
                    catch (IndexOutOfRangeException) { break; }
                }
            }

            foreach (var item in keys) { key.Append(Convert.ToChar(item + 97).ToString()); } //keys as letters
            decryption = string.Join("", fullDecryption); //the decryption

        }

        //turns the vigenere cipher into an array of caesar ciphers
        public static string[] TextToArray(string text, int keyLength)
        {
            string[] textArray = new string[keyLength];

            for (int x = 1; x < keyLength + 1; x++)
            {
                List<string> textList = new();

                for (int l = x; l < text.Length + 1; l += keyLength)
                {
                    textList.Add(Convert.ToString(text.Substring(l - 1, 1)));
                }
                textArray[x - 1] = string.Join("", textList);
            }
            //returning the array created
            return textArray;

        }
    }
}