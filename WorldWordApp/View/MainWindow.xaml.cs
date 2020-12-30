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
            //gameLogic.Connect();
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
                    Close_Game();
                }
            }
        }

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
            // add **all** windows
            //DataBaseConnector.close();
            // Disconnect from the server.
        }

        private void startPlaying_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            sign.ResetDetailes();
            sign.ShowDialog();
        }

        private void seePlayers_Click(object sender, RoutedEventArgs e)
        {
            players.SetAllPlayers(gameLogic.GetAllPlayers());
            this.Hide();
            players.ShowDialog();
        }

        private void seeRecords_Click(object sender, RoutedEventArgs e)
        {
            records.SetAllScores(gameLogic.GetHighScors());
            this.Hide();
            records.ShowDialog();
        }
    }
}
