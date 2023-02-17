using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace frequencies.View.UserControls
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl, INotifyPropertyChanged
    {
        public MenuBar()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private string currentTool;

        public string CurrentTool
        {
            get { return currentTool; }
            set { currentTool = value; OnPropertyChanged(); }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AffineCMenu_Click(object sender, System.Windows.RoutedEventArgs e) => CurrentTool = "Affine Cipher";
        private void AtbashCMenu_Click(object sender, System.Windows.RoutedEventArgs e) => CurrentTool = "Atbash Cipher";
    }


}
