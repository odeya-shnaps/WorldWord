using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldWordApp.DB;
using WorldWordApp.Game_Logic;

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Records records;
        private Players players;
        private SignUp sign;
        public bool ShowMessage { get; set; }
        public GameLogic gameLogic;

        public MainWindow()
        {
            InitializeComponent();
            records = new Records();
            players = new Players();
            sign = new SignUp();
            gameLogic = new GameLogic();
            ShowMessage = true;

            sign.setMainWindow(this);
            records.setMainWindow(this);
            players.setMainWindow(this);
            // connect to the db
            gameLogic.Connect();
        }
        // message box pop when closing, if just moving to another window don't pop.
        void Window_Closing(object sender, CancelEventArgs e)
        {
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
                    // If user doesn't want to close, cancel close.
                    e.Cancel = true;
                }
                else
                {
                    // closing all the open windows and the connection to the db.
                    Close_Game();
                }
            }
        }

        //close all the open windows and connection to db.
        public void Close_Game()
        {
            if (!sign.IsClosed)
            {
                sign.ShowMessage = false;
                sign.Close();
            }
            if (!records.IsClosed)
            {
                records.ShowMessage = false;
                records.Close();
            }
            if (!players.IsClosed)
            {
                players.ShowMessage = false;
                players.Close();
            }
            // Disconnect from the server
            gameLogic.CloseConnection();
        }

        // move to sign up window to start playing.
        private void startPlaying_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            sign.ResetDetailes();
            sign.ShowDialog();
        }
        // move to players window
        private void seePlayers_Click(object sender, RoutedEventArgs e)
        {
            // set the player list field in players window.
            players.SetAllPlayers(gameLogic.GetAllPlayers());
            players.SetPlayersList();
            this.Hide();
            players.ShowDialog();
        }
        // move to records window.
        private void seeRecords_Click(object sender, RoutedEventArgs e)
        {
            // set the high scores list in records window.
            records.SetAllScores(gameLogic.GetHighScores());
            this.Hide();
            records.ShowDialog();
        }
    }
}
