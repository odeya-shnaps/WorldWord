﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WorldWordApp.Game_Logic;
using WorldWordApp.Objects;

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for Games.xaml
    /// </summary>
    public partial class Records : Window
    {
        private MainWindow mainWindow;
        public bool ShowMessage { get; set; }
        public bool IsClosed { get; private set; }
        List<Score> highScores;

        public Records()
        {
            InitializeComponent();
            ShowMessage = true;
        }

        public void setMainWindow(MainWindow mainWin)
        {
            this.mainWindow = mainWin;
        }

        // message box pop when closing, if just moving to another window don't pop.
        void Window_Closing(object sender, CancelEventArgs e)
        {
            IsClosed = true;
            if (ShowMessage)
            {
                // Notify the user and ask for a response.
                string msg = "Are You Sure You Want To Close The Application?";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "World Word App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    // If user doesn't want to close, cancel closure.
                    e.Cancel = true;
                }
                else
                {
                    // closing all the open windows and the connection to the db.
                    mainWindow.Close_Game();
                    // closing the main window
                    mainWindow.ShowMessage = false;
                    mainWindow.Close();
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.mainWindow.ShowDialog();
        }

        public void SetAllScores(List<Score> scores)
        {
            highScores = scores;
            int i;
            for (i = 1; i <= scores.Count; i++)
            {
                TextBlock txt = this.FindName("name" + i) as TextBlock;
                txt.Text = scores[i-1].Name;
                txt = this.FindName("score" + i) as TextBlock;
                txt.Text = scores[i-1].HighScore.ToString();
            }
        }
    }
}
