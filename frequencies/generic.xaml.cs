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

namespace frequencies
{
    /// <summary>
    /// Interaction logic for generic.xaml
    /// </summary>
    public partial class generic : Page
    {
        public generic()
        {
            DataContext = this;
            InitializeComponent();
            PlaceHolder = "Enter cipher text: ";
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

        }


    }
}
