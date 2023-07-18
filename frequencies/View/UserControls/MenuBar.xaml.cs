using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
            CurrentTool = "home";
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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Content = null;
        }

    }


}
