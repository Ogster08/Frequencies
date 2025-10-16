using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks.Sources;
using System.IO;
using System.Diagnostics;

namespace frequencies
{
    internal class variables
    {
        static public Cipher cipher;
    }

    enum Cipher
    {
        AFFINE,
        ATBASH,
        CAESAR,
        RAIL_FENCE,
        SUBSTITUTION,
        VIGENERE
    }

    internal class CaesarSolver
    {
        private string text;
        private int[] key = new int[2];
        private string decryption;

        public int[] Key { get { return key; } }
        public string Decryption { get { return decryption; } }

        public CaesarSolver(string Text)
        {
            text = Text;
        }

        //function to create a dictionary of the count frequency of each character in the testText given
        public static Dictionary<string, int> TextFrequency(string testText)
        {
            //dictionary for counting letters
            var characterCount = new Dictionary<string, int>() { { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }, { "i", 0 }, { "j", 0 }, { "k", 0 }, { "l", 0 }, { "m", 0 }, { "n", 0 }, { "o", 0 }, { "p", 0 }, { "q", 0 }, { "r", 0 }, { "s", 0 }, { "t", 0 }, { "u", 0 }, { "v", 0 }, { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 } };

            foreach (char c in testText) { characterCount[c.ToString()]++; } //increse the count of that character by 1
            return characterCount;
        }

        //return a value that shows the testText's distribution compared to normal english  link to formula => https://g.co/kgs/si6Yta
        public static double ChiSquareTest(string testText)
        {
            //a dictionary with the excpected frequency of each letter
            var exspectedFrequencies = new Dictionary<string, double>() { { "e", 11.1607 }, { "a", 8.4966 }, { "r", 7.5809 }, { "i", 7.5448 }, { "o", 7.1635 }, { "t", 6.9509 }, { "n", 6.6544 }, { "s", 5.7351 }, { "l", 5.4893 }, { "c", 4.5388 }, { "u", 3.6308 }, { "d", 3.3844 }, { "p", 3.1671 }, { "m", 3.0129 }, { "h", 3.0034 }, { "g", 2.4705 }, { "b", 2.0720 }, { "f", 1.8121 }, { "y", 1.7779 }, { "w", 1.2899 }, { "k", 1.1016 }, { "v", 1.0074 }, { "x", 0.2902 }, { "z", 0.2722 }, { "j", 0.1965 }, { "q", 0.1962 } };

            //getting a dictionary of the observed counts of each letter
            Dictionary<string, int> textFrequencies = TextFrequency(testText);

            //chisquare test
            double score = 0;

            foreach (string d in textFrequencies.Keys)
            {
                string s = d.ToLower();
                double exspectedCount = exspectedFrequencies[s] / 100 * testText.Length;
                score += Math.Pow(textFrequencies[s] - exspectedCount, 2) / exspectedCount;
            }

            //return the score given by the chi square test
            return score;
        }

        //solve the caesar by bruteforcing it and doing the chisquare test for each to check which is english
        public void solve()
        {
            //create an array for the chi square score for each possible caeser key 
            double[] scores = new double[26];

            //for each caeser key, 'decrypt' the text with the key and then get the chi square score for that key
            for (int i = 0; i < 26; i++)

            {
                string[] ceaser = new string[text.Length];

                //loop over each character in the text and take away the offset and then add it to the caeser array
                for (int p = 0; p < text.Length; p++)
                {
                    //convert the character to ascii then - 97 so a = 0 and z = 25 then take off the caeser offset
                    //then find the remainder of that divided by 26 so like a round robin to keep it in the range of 0 - 25
                    int x = (Convert.ToInt32(text[p]) - i - 97) % 26;
                    //add 26 to x if x is less than 0 to keep it within the range of 0 - 25
                    if (x < 0) { x += 26; }
                    //add 97 to x to make it the right number for ascii conversion then convert it back to a character and then add it to the caeser array
                    ceaser[p] = Convert.ToChar(x + 97).ToString();
                }
                //getting score for that key
                scores[i] = ChiSquareTest(string.Join("", ceaser));
            }
            //sets keys to the possible ceaser keys that got the best scores
            double[] secondLowest = new double[26];
            scores.CopyTo(secondLowest, 0);
            secondLowest[Array.IndexOf(secondLowest, secondLowest.Min())] = 9999;

            key = new int[] { Array.IndexOf(scores, (secondLowest.Min() - scores.Min() < 15 && scores.Min() > 50) ? secondLowest.Min() : scores.Min()), Array.IndexOf(scores, (secondLowest.Min() - scores.Min() < 15 && scores.Min() > 50) ? scores.Min() : secondLowest.Min()) };// english language is weird - replacement of if else

            Decrypt();
        }

        //decrypting the ceaser with the right key
        private void Decrypt()
        {
            //create the decryted text list
            string[] decryptionArray = new string[text.Length];

            //cycling through each character in the text to decrypt the character each time
            for (int i = 0; i < text.Length; i++)
            {
                int x = (Convert.ToInt32(text[i]) - key[0] - 97) % 26; //converting ascii of char to int and decrypting the number
                if (x < 0) { x += 26; } //% will keep negative numbers negative but we need the numbers to be positive to convert back to a char
                decryptionArray[i] = Convert.ToChar(x + 97).ToString(); //converting number to char and adding it to the decryption text
            }
            //returning the decryption text as a string
            decryption = string.Join("", decryptionArray);
        }
    }

    internal class vigenere
    {

        private string text;
        private string decryption = "";
        private StringBuilder key = new StringBuilder();

        public string Decryption { get { return decryption; } }
        public string Key { get { return key.ToString(); } }

        public vigenere(string text)
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

    internal class affine
    {
        private string text;
        private string decryption = "";
        private string key = "";

        public string Decryption { get { return decryption; } }
        public string Key { get { return key; } }

        public affine(string Text)
        {
            text = Text;
        }

        public void solve()
        {

            List<string[]> possibleDecryptions = new();
            for (int x = 1; x <= 25; x += 2)
            {
                for (int y = 0; y <= 25; y++)
                {
                    int x_inv = 0;
                    for (int i = 0; i < 26; i++)
                    {

                        if (x * i % 26 == 1)
                        {
                            x_inv = i;
                        }
                    }

                    List<string> decrypted = new();
                    foreach (char letter in text)
                    {
                        int letterNumber = Convert.ToInt32(letter - 97) % 26;
                        int numberDecrypted = ((letterNumber - y) * x_inv) % 26;
                        while (numberDecrypted < 0) { numberDecrypted += 26; }
                        decrypted.Add(Convert.ToChar(numberDecrypted + 97).ToString());
                    }
                    string[] ToAdd = { string.Join("", decrypted), $"{x}, {y}" };
                    possibleDecryptions.Add(ToAdd);
                }
            }
            double[] scores = new double[possibleDecryptions.Count];
            for (int i = 0; i < possibleDecryptions.Count(); i++) { scores[i] = ChiSquareTest(possibleDecryptions[i][0]); }
            string decryptionAndKey = string.Join("", possibleDecryptions[Array.IndexOf(scores, scores.Min())]);
            decryption = decryptionAndKey.Substring(0, decryptionAndKey.Length - 4);
            key = decryptionAndKey.Substring(decryptionAndKey.Length - 4);


        }

        public static Dictionary<string, int> TextFrequency(string testText)
        {
            var characterCount = new Dictionary<string, int>() { { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }, { "i", 0 }, { "j", 0 }, { "k", 0 }, { "l", 0 }, { "m", 0 }, { "n", 0 }, { "o", 0 }, { "p", 0 }, { "q", 0 }, { "r", 0 }, { "s", 0 }, { "t", 0 }, { "u", 0 }, { "v", 0 }, { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 } };

            foreach (char c in testText) { characterCount[c.ToString()]++; }
            return characterCount;
        }

        public static double ChiSquareTest(string testText)
        {
            var exspectedFrequencies = new Dictionary<string, double>() { { "e", 11.1607 }, { "a", 8.4966 }, { "r", 7.5809 }, { "i", 7.5448 }, { "o", 7.1635 }, { "t", 6.9509 }, { "n", 6.6544 }, { "s", 5.7351 }, { "l", 5.4893 }, { "c", 4.5388 }, { "u", 3.6308 }, { "d", 3.3844 }, { "p", 3.1671 }, { "m", 3.0129 }, { "h", 3.0034 }, { "g", 2.4705 }, { "b", 2.0720 }, { "f", 1.8121 }, { "y", 1.7779 }, { "w", 1.2899 }, { "k", 1.1016 }, { "v", 1.0074 }, { "x", 0.2902 }, { "z", 0.2722 }, { "j", 0.1965 }, { "q", 0.1962 } };
            Dictionary<string, int> textFrequencies = TextFrequency(testText);

            double score = 0;

            foreach (string d in textFrequencies.Keys)
            {
                string s = d.ToLower();
                double exspectedCount = exspectedFrequencies[s] / 100 * testText.Length;
                score += Math.Pow(textFrequencies[s] - exspectedCount, 2) / exspectedCount;
            }
            return score;
        }
    }

    internal class railFence
    {
        private string text;
        private string decryption = "";
        private string key = "";

        public string Decryption { get { return decryption; } }
        public string Key { get { return key; } }

        public railFence(string Text)
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
                scores[i] = ngrams.score(string.Join("", possibleDecryptions[i].Item1.Where(char.IsLetter).ToArray()));
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

    internal class Ngrams
    {
        private double[] scoresUsingInt_;
        private int l_;
        private double sum_ = 0;
        private double floor_;

        public Ngrams(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            l_ = lines[0].Split(" ")[0].Length;

            scoresUsingInt_ = new double[(int)Math.Pow(32, l_)];
            Array.Fill(scoresUsingInt_, 0.01);
            
            foreach (string item in lines)
            {
                string[] keyPair = item.Split(" ");
                double value = double.Parse(keyPair[1]);
                string key = keyPair[0];

                scoresUsingInt_[EncodeUpper(key)] = value;
                sum_ += value;
            }

            for (int i = 0; i < scoresUsingInt_.Length; i++)
            {
                scoresUsingInt_[i] = Math.Log10(scoresUsingInt_[i] / sum_);
            }

            floor_ = Math.Log10(0.01 / sum_);
        }

        private static int EncodeLower(ReadOnlySpan<char> gram)
        {
            int value = 0;
            foreach (char c in gram)
            {
                value = (value << 5) | (c - 'a');
            }
            return value;
        }
        private static int EncodeUpper(ReadOnlySpan<char> gram)
        {
            int value = 0;
            foreach (char c in gram)
            {
                value = (value << 5) | (c - 'A');
            }
            return value;
        }

        public double score(ReadOnlySpan<char> text)
        {
            double score = 0;

            int encoded = EncodeLower(text.Slice(0, l_));
            score += scoresUsingInt_[encoded];

            int mask = (1 << (5 * (l_ - 1))) - 1;

            for (int i = l_; i < text.Length - l_ + 1; i++)
            {
                encoded = ((encoded & mask) << 5) | (text[i] - 'a');
                score += scoresUsingInt_[encoded];
            }
            return score;
        }

        public double score(ReadOnlySpan<Byte> text)
        {
            double score = 0;
            int encoded = 0;
            int mask = (1 << (5 * (l_ - 1))) - 1;

            for (int i = 0; i < l_; i++)
            {
                encoded = ((encoded & mask) << 5) | text[i];
            }
            score += scoresUsingInt_[encoded];

            for (int i = l_; i < text.Length - l_ + 1; i++)
            {
                encoded = ((encoded & mask) << 5) | text[i];
                score += scoresUsingInt_[encoded];
            }
            return score;
        }

    }

    internal class atbash
    {
        private string text;
        private StringBuilder decryption = new StringBuilder();

        public string Decryption { get { return decryption.ToString(); } }

        public atbash(string Text)
        {
            text = Text;
        }

        public void solve()
        {
            char[] key = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            key = key.Reverse().ToArray();

            foreach (char letter in text)
            {
                decryption.Append(key[letter - 97]);
            }
        }
    }

    internal class substitutian
    {
        private string text;
        private string decryption = "";
        private string key = "";
        private Ngrams ngrams;

        public string Decryption { get { return decryption; } }
        public string Key { get { return key; } }

        public substitutian(string Text)
        {
            text = Text;
            ngrams = new("english_quadgrams.txt");

        }

        public substitutian(string Text, Ngrams ngrams)
        {
            text = Text;
            this.ngrams = ngrams;
        }

        public void solve()
        {
            char[] alphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
            int n = 2_000;
            int r = 3;
            int repeats = 0;
            
            char[] bestDecryption = text.ToCharArray();
            char[] CurrentDecryption = new char[text.Length];
            char[] possibleDecryption = new char[text.Length];
            char[] bestKey = [.. alphabet];
            char[] currentKey = [.. bestKey];
            char[] possibleKey = [.. bestKey];
            
            Random rnd = new Random();


            double CurrentScore;
            double possibleScore = -9999999999;
            double bestScore = possibleScore;
            CurrentScore = ngrams.score(text);

            

            while (repeats < r)
            {
                possibleKey = [.. alphabet];
                CurrentScore = ngrams.score(text);

                for (int i = 0; i < n; i++)
                {

                    int a = rnd.Next(26);
                    int b = rnd.Next(26);

                    (possibleKey[a], possibleKey[b]) = (possibleKey[b], possibleKey[a]);

                    for (int index = 0; index < text.Length; index++)
                    {
                        possibleDecryption[index] = possibleKey[text[index] - 'a'];
                    }

                    possibleScore = ngrams.score(possibleDecryption);
                    if (possibleScore > CurrentScore)
                    {
                        CurrentScore = possibleScore;
                        Array.Copy(possibleDecryption, CurrentDecryption, text.Length);
                        Array.Copy(possibleKey, currentKey, 26);
                    }
                    else
                    {
                        (possibleKey[a], possibleKey[b]) = (possibleKey[b], possibleKey[a]);
                        Array.Copy(CurrentDecryption, possibleDecryption, text.Length);
                    }
                }

                if (CurrentScore > bestScore) 
                { 
                    bestScore = CurrentScore;
                    repeats = 0;
                    Array.Copy(currentKey, bestKey, 26);
                    Debug.WriteLine(CurrentScore / text.Length); 
                }
                else if (CurrentScore == bestScore)
                {
                    repeats++;
                } 
            }
            char[] oppositeKey = new char[26];
            for (int i = 0; i < 26; i++) { oppositeKey[bestKey[i] - 'a'] = Convert.ToChar(i + 'a'); }

            for (int index = 0; index < text.Length; index++)
            {
                bestDecryption[index] = bestKey[text[index] - 'a'];
            }

            this.decryption = new string(bestDecryption);
            this.key = new string(oppositeKey) + (CurrentScore / text.Length).ToString();

        }
    }
}