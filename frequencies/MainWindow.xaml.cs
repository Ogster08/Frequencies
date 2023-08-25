using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace frequencies
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("caesar.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double h = ((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualHeight;
            box1.MaxHeight = h / 7;
            box2.MaxHeight = h / 7;
            box3.MaxHeight = h / 7;
            box4.MaxHeight = h / 7;
            box5.MaxHeight = h / 7;
            box6.MaxHeight = h / 7;
            box7.MaxHeight = h / 7;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double h = ((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualHeight;
            box1.MaxHeight = h / 7;
            box2.MaxHeight = h / 7;
            box3.MaxHeight = h / 7;
            box4.MaxHeight = h / 7;
            box5.MaxHeight = h / 7;
            box6.MaxHeight = h / 7;
            box7.MaxHeight = h / 7;
        }
    }
}
