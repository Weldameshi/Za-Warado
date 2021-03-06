﻿using System;
using System.Windows;

namespace ZaWarado
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            WindowDisplay.Initialize(this);
            ResumeExistingGame.IsChecked = true;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            WindowDisplay.ResumeExistingGame = (bool)ResumeExistingGame.IsChecked;
            WindowDisplay.ShowGame();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
