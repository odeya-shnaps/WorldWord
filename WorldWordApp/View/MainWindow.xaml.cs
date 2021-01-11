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
using System.Windows.Threading;
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
        public bool isConnect;
        private DispatcherTimer timer;

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
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.5);
            timer.Tick += Timer_Tick;

            // connect to the db, if can't connect show message to user and ask him to reconnect.
            isConnect = gameLogic.Connect();
            if (!isConnect)
            {
                ConnectionFailed();
            }
        }
        
        public void ConnectionFailed()
        {
            seePlayers.IsEnabled = false;
            seeRecords.IsEnabled = false;
            startPlaying.IsEnabled = false;
            failedConnect.Text = "Failed Connecting To DB,\n please try again...";
            reconnect.IsEnabled = true;
            reconnect.Visibility = Visibility.Visible;
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
            try
            {
                // set the player list field in players window.
                players.SetAllPlayers(gameLogic.GetAllPlayers());
                players.SetPlayersList();
                this.Hide();
                players.ShowDialog();
            }
            catch(Exception)
            {
                isConnect = false;
                ConnectionFailed();
            }  
        }
        // move to records window.
        private void seeRecords_Click(object sender, RoutedEventArgs e)
        {
            // set the high scores list in records window.
            try
            {
                records.SetAllScores(gameLogic.GetHighScores());
                this.Hide();
                records.ShowDialog();
            }
            catch(Exception)
            {
                isConnect = false;
                ConnectionFailed();
            }
        }

        // tring to reconnect to the db, if can't show message to user and ask him to reconnect.
        private void reconnect_Click(object sender, RoutedEventArgs e)
        {
            isConnect = gameLogic.Connect();
            failedConnect.Text = "Trying To Connect...";
            reconnect.IsEnabled = false;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            if (!isConnect)
            {
                reconnect.IsEnabled = true;
                failedConnect.Text = "Failed Connecting To DB,\n please try again...";
                reconnect.Visibility = Visibility.Visible;
            }
            else
            {
                seePlayers.IsEnabled = true;
                seeRecords.IsEnabled = true;
                startPlaying.IsEnabled = true;
                failedConnect.Text = "";
                reconnect.Visibility = Visibility.Hidden;
            }
        }
    }
}
