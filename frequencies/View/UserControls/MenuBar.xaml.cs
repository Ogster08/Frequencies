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
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            DataContext = this;
            InitializeComponent();
            string cipherName = variables.cipher.ToString().ToLower().Replace("_", " ");
            Name = char.ToUpper(cipherName[0]) + cipherName.Substring(1);
        }

        private string name;

        public string Name 
        {
            get { return name; }
            set 
            { 
                name = value; 
                ToolName.Text = Name;
            }
        }
        
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Content = null;
        }

    }


}
