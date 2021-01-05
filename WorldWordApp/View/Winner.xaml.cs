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

        public Winner()
        {
            InitializeComponent();
            ShowMessage = true;
        }

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
                    mainWindow.Close_Game();
                    mainWindow.ShowMessage = false;
                    mainWindow.Close();
                }
            }
        }

        public void SetWinnerAndScore(Player win, Player lose, bool tie)
        {
            winner = win;
            loser = lose;
            isTie = tie;
            if (tie == true)
            {
                theWinner.Text = "It's a tie!!!";
            } else {
                theWinner.Text = "And the winner is " + win.PlayerName + "!!!";
            }
            player1.Text = win.PlayerName;
            player2.Text = lose.PlayerName;
            score1.Text = win.CurrentScore.ToString()+ " points";
            score2.Text = lose.CurrentScore.ToString()+ " points";

        }

        public void setMainWindow(MainWindow mainWin)
        {
            this.mainWindow = mainWin;
        }

        private void toMainWindow_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage = false;
            this.Close();
            mainWindow.Show();
        }

        private void toScores_Click(object sender, RoutedEventArgs e)
        {
            Records records = mainWindow.records;
            records.SetAllScores(mainWindow.gameLogic.GetHighScors());
            ShowMessage = false;
            records.Show();
            this.Close();
        }
    }
}
