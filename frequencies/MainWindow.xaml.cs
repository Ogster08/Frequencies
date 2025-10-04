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

        private void Affine(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            variables.cipher = Cipher.AFFINE;
        }

        private void Atbash(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            variables.cipher = Cipher.ATBASH;
        }

        private void Caesar(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            variables.cipher = Cipher.CAESAR;
        }

        private void Rail_Fence(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            variables.cipher = Cipher.RAIL_FENCE;
        }

        private void Substituion(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            variables.cipher = Cipher.SUBSTITUTION;
        }

        private void Vigenere(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            variables.cipher = Cipher.VIGENERE;
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
        }
    }
}
