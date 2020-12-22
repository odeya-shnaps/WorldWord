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

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Games games;
        private Players players;
        private SignUp sign;

        //private Frame frame;

        public MainWindow()
        {
            InitializeComponent();
            //frame = new Frame();
            games = new Games();
            players = new Players();
            sign = new SignUp();

            sign.setMainWindow(this);
            games.setMainWindow(this);
            players.setMainWindow(this);
            DataBaseConnector.EstablishConnection("127.0.0.1", "root", "987654", "world_word_db");
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
                sign.ShowMessage = false;
                sign.Close();
                // Disconnect from the server.
            }
        }

        private void startPlaying_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            sign.ShowDialog();
        }

        private void seePlayers_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            players.ShowDialog();
        }

        private void seeGames_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            games.ShowDialog();
        }


        // need to change this function - it's just the base
   /*     private void Login(string username, string userId)
        {
            List<Player> aUser = PlayersDA.RetrieveUser(username);
            bool exist = false;
            foreach (var player in aUser)
            {
                if (player.UserId.Equals(userId))
                {
                    MessageBox.Show("Login Success");
                    *//*View_Layer.MainMenu m = new View_Layer.MainMenu();
                    m.Show();*//*
                    exist = true;
                    break;
                }
            }
            if (exist == false)
            {
                MessageBox.Show("New User");
                PlayersDA.InsertNewUser(username, userId);

            }
        }*/


    }
}
