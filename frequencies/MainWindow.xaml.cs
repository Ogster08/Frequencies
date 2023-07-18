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
            frame.Navigate(new Uri("generic.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}
