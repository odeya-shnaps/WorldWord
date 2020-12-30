﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for Games.xaml
    /// </summary>
    public partial class Games : Window
    {
        private MainWindow mainWindow;

        public Games()
        {
            InitializeComponent();
            string name = "name";
            int i;
            for (i = 1; i <=10; i++)
            {
                TextBlock txt = this.FindName(name+i) as TextBlock;
                txt.Text = name + i;
            }
            string score = "score";
            for (i = 1; i <= 10; i++)
            {
                TextBlock txt = this.FindName(score + i) as TextBlock;
                txt.Text = score + i;
            }

        }

        public void setMainWindow(MainWindow mainWin)
        {
            this.mainWindow = mainWin;
        }
        void Window_Closing(object sender, CancelEventArgs e)
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
                // Disconnect from the server.
            }
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.mainWindow.ShowDialog();
        }
    }
}
