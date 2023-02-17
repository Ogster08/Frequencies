using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace frequencies.View.UserControls
{
    /// <summary>
    /// Interaction logic for clearableTextBox.xaml
    /// </summary>
    public partial class clearableTextBox : UserControl, INotifyPropertyChanged
    {
        public clearableTextBox()
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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        { 
            txtInput.Clear();
            txtInput.Focus();
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

        }
    }
}
