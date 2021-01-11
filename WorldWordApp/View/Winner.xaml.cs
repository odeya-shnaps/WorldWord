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
using System.Windows.Shapes;
using System.Windows.Threading;
using WorldWordApp.Objects;

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for Winner.xaml
    /// </summary>
    public partial class Winner : Window
    {
        public bool ShowMessage { get; set; }
        private MainWindow mainWindow;
        private Player winner;
        private Player loser;
        private bool isTie;
        public bool isConnect;
        private DispatcherTimer reconnectTimer;
        private DispatcherTimer timer;
        private bool toScore;

        public Winner()
        {
            InitializeComponent();
            ShowMessage = true;
            reconnectTimer = new DispatcherTimer();
            reconnectTimer.Interval = TimeSpan.FromSeconds(1.5);
            reconnectTimer.Tick += Reconnect_Timer_Tick;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            error.Text = "";
            isConnect = true;
            toScore = false;
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
        // enter to view from list
        public void SetWinnerAndScore(Player win, Player lose, bool tie)
        {
            winner = win;
            loser = lose;
            isTie = tie;
            if (tie == true)
            {
                theWinner.Text = "It's a tie!!!";
            } else {
                theWinner.Text = "And the winner is\n" + win.PlayerName + "!!!";
            }
            player1.Text = win.PlayerName;
            player2.Text = lose.PlayerName;
            score1.Text = win.CurrentScore.ToString()+ " points";
            score2.Text = lose.CurrentScore.ToString()+ " points";
            if (!isConnect)
            {
                ConnectionFailed("Connection To DB Failed, can't save game details. try to reconnect...", true);
            }
        }

        // connection failed showing to the user and trying to connect
        private void ConnectionFailed(string message, bool keepMain)
        {
            isConnect = false;
            if (keepMain)
            {
                button.IsEnabled = false;
            }
            button1.IsEnabled = false;
            error.Text = message;
            reconnect.IsEnabled = true;
            reconnect.Visibility = Visibility.Visible;
        }

        public void setMainWindow(MainWindow mainWin)
        {
            this.mainWindow = mainWin;
        }
        // move to main window, show massage if disconnect and try reconnect.
        private void toMainWindow_Click(object sender, RoutedEventArgs e)
        {
            if(!isConnect)
            {
                mainWindow.ConnectionFailed();
            }
            ShowMessage = false;
            this.Close();
            mainWindow.Show();
        }
        // try to move to high scores, if disconnected show message and try reconnect.
        private void toScores_Click(object sender, RoutedEventArgs e)
        {
            Records records = mainWindow.records;
            try
            {
                records.SetAllScores(mainWindow.gameLogic.GetHighScores());
                ShowMessage = false;
                records.Show();
                this.Close();
            }
            catch(Exception)
            {
                ConnectionFailed("Failed Connecting To DB to get high scores. try again...", false);
                toScore = true;
            }
        }

        // tring to reconnect to the db, if can't show message to user and ask him to reconnect.
        private void reconnect_Click(object sender, RoutedEventArgs e)
        {
            error.Text = "Trying To Connect...";
            reconnect.IsEnabled = false;
            isConnect = mainWindow.gameLogic.Connect();
            reconnectTimer.Start();
        }

        private void Reconnect_Timer_Tick(object sender, EventArgs e)
        {
            reconnectTimer.Stop();
            if (!isConnect)
            {
                reconnect.IsEnabled = true;
                error.Text = "Connection To DB Failed.\n try to reconnect...";
                reconnect.Visibility = Visibility.Visible;
            }
            else
            {
                // connected but need to save game details first
                if (!toScore)
                {
                    error.Text = "Connected! saving game details...";
                    if (mainWindow.gameLogic.EndeGame())
                    {
                        timer.Start();
                        button.IsEnabled = true;
                        button1.IsEnabled = true;
                        reconnect.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        isConnect = false;
                        ConnectionFailed("Connection To DB Failed, can't save game details. try to reconnect...", true);
                    }
                }
                // connected, we can press high score now (disconnect after saving when we want to see high scores).
                else
                {
                    button1.IsEnabled = true;
                    error.Text = "Connected! you can press high scores";
                    reconnect.Visibility = Visibility.Hidden;
                    timer.Start();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            error.Text = "";
        }
    }
}
