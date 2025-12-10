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
            Variables.cipher = Cipher.AFFINE;
        }

        private void Atbash(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            Variables.cipher = Cipher.ATBASH;
        }

        private void Caesar(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            Variables.cipher = Cipher.CAESAR;
        }

        private void Playfair(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            Variables.cipher = Cipher.PLAYFAIR;
        }

        private void Rail_Fence(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            Variables.cipher = Cipher.RAIL_FENCE;
        }

        private void Substitian(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            Variables.cipher = Cipher.SUBSTITUTION;
        }

        private void Vigenere(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            Variables.cipher = Cipher.VIGENERE;
        }

        private void Transposition_Row_By_Row(object sender, RoutedEventArgs e)
        {
            Frame frame = (Application.Current.MainWindow as MainWindow).Main;
            frame.Navigate(new Uri("/CipherSolver.xaml", UriKind.RelativeOrAbsolute));
            Variables.cipher = Cipher.TRANSPOSITION_ROW_BY_ROW;
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
            box8.MaxHeight = h / 7;

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
            box8.MaxHeight = h / 7;
        }
    }
}
