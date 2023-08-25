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
    public partial class caesar : Page
    {
        public caesar()
        {
            DataContext = this;
            InitializeComponent();
            PlaceHolder = "Enter cipher text: ";
            KeyText = "Key: ";
            SolutionText = "Solution: ";
            PageName = this.GetType().Name;
            MenuBar.CurrentTool = PageName;
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

            CeaserSolver solver = new CeaserSolver(lettersText);
            solver.Solve();
            string solution = solver.Decryption;
            int length = solution.Length;
            foreach (var item in nonLetters) {solution = solution.Insert(Convert.ToInt32(item[1]), item[0].ToString()); }
            SolutionText = solution;
            KeyText = solver.Keys[0].ToString();

        }


    }
}
