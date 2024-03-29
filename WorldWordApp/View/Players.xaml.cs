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
using WorldWordApp.Objects;

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for Players.xaml
    /// </summary>
    public partial class Players : Window
    {
        private MainWindow mainWindow;
        public bool ShowMessage { get; set; }
        public bool IsClosed { get; private set; }
        private List<Player> players;

        public Players()
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

        public void SetAllPlayers(List<Player> allPlayers)
        {
            this.players = allPlayers;
        }

        public void SetPlayersList()
        {
            List<Person> people = new List<Person>();
            for (int i = 0; i < players.Count(); i++)
            {
                people.Add(new Person() { High_score = players[i].UserHighScore, Name = players[i].PlayerName });
            }
            dgSimple.ItemsSource = people;
        }
    }


    public class Person
    {
        public string Name { get; set; }
        public int High_score { get; set; }

    }
}
