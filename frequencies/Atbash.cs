using System.Linq;
using System;
using System.Text;

namespace frequencies
{
    internal class Atbash
    {
        private string text;
        private StringBuilder decryption = new StringBuilder();

        public string Decryption { get { return decryption.ToString(); } }

        public Atbash(string Text)
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
}