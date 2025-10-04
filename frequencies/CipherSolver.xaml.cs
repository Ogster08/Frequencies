using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace frequencies
{
    /// <summary>
    /// Interaction logic for generic.xaml
    /// </summary>
    public partial class CipherSolver : Page
    {
        public CipherSolver()
        {
            DataContext = this;
            InitializeComponent();
            PlaceHolder = "Enter cipher text: ";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private string placeHolder;

        public string PlaceHolder
        {
            get { return placeHolder; }
            set
            {
                placeHolder = value;
                tbPlaceHolder.Text = placeHolder;
            }
        }

        private string pageName;

        public string PageName
        {
            get { return pageName; }
            set
            {
                pageName = value;
                MenuBar.Name = pageName;
            }
        }

        private string cipherText;

        public string CipherText
        {
            get { return cipherText; }
            set
            {
                cipherText = value;
                OnPropertyChanged();
            }
        }

        private string keyText;

        public string KeyText
        {
            get { return keyText; }
            set
            {
                keyText = value;
                KeyOutput.Text = keyText;
            }
        }

        private string solutionText;

        public string SolutionText
        {
            get { return solutionText; }
            set
            {
                solutionText = value;
                SolutionOutput.Text = solutionText;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                tbPlaceHolder.Visibility = Visibility.Visible;
            }
            else
            {
                tbPlaceHolder.Visibility = Visibility.Hidden;
            }
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string text = txtInput.Text;
            string lettersText = string.Join("", text.Where(char.IsLetter).ToArray()).ToLower();

            List<string[]> nonLetters = new();

            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsLetter(text[i]))
                {
                    string[] nonLetterIndex = { text[i].ToString(), i.ToString() };
                    nonLetters.Add(nonLetterIndex);
                }
            }

            switch (variables.cipher)
            {
                case Cipher.AFFINE:
                    affine affinesolver = new affine(lettersText);
                    affinesolver.solve();
                    string affinesolution = affinesolver.Decryption;
                    foreach (var item in nonLetters) { affinesolution = affinesolution.Insert(Convert.ToInt32(item[1]), item[0].ToString()); }
                    SolutionText = affinesolution;
                    KeyText = affinesolver.Key;
                    break;

                case Cipher.ATBASH:
                    atbash atbashsolver = new atbash(lettersText);
                    atbashsolver.solve();
                    string atbashsolution = atbashsolver.Decryption;
                    foreach (var item in nonLetters) { atbashsolution = atbashsolution.Insert(Convert.ToInt32(item[1]), item[0].ToString()); }
                    SolutionText = atbashsolution;
                    break;

                case Cipher.CAESAR:
                    CaesarSolver caesarsolver = new CaesarSolver(lettersText);
                    caesarsolver.solve();
                    string caesarsolversolution = caesarsolver.Decryption;
                    foreach (var item in nonLetters) { caesarsolversolution = caesarsolversolution.Insert(Convert.ToInt32(item[1]), item[0].ToString()); }
                    SolutionText = caesarsolversolution;
                    KeyText = caesarsolver.Key[0].ToString();
                    break;

                case Cipher.RAIL_FENCE:
                    railFence railfencesolver = new railFence(text);
                    railfencesolver.solve();
                    SolutionText = railfencesolver.Decryption;
                    KeyText = railfencesolver.Key;
                    break;

                case Cipher.SUBSTITUTION:
                    substitutian substitutiansolver = new substitutian(lettersText);
                    substitutiansolver.solve();
                    string substitutionsolution = substitutiansolver.Decryption;
                    foreach (var item in nonLetters) { substitutionsolution = substitutionsolution.Insert(Convert.ToInt32(item[1]), item[0].ToString()); }
                    SolutionText = substitutionsolution;
                    KeyText = substitutiansolver.Key;
                    break;

                case Cipher.VIGENERE:
                    vigenere vigeneresolver = new vigenere(lettersText);
                    vigeneresolver.solve();
                    string vigeneresolution = vigeneresolver.Decryption;
                    foreach (var item in nonLetters) { vigeneresolution = vigeneresolution.Insert(Convert.ToInt32(item[1]), item[0].ToString()); }
                    SolutionText = vigeneresolution;
                    KeyText = vigeneresolver.Key;
                    break;

                default:
                    break;
            }
        }
    }
}
