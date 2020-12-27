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
using WorldWordApp.Data_Access_Layer;
using WorldWordApp.DB;
using WorldWordApp.Objects;

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {

        private MainWindow mainWindow;
        private PlayGame game;

        private QueryDA queryDA;
        private PlayersDA playerDA;

        private DataBaseConnector dbConnector;

        public bool ShowMessage { get; set; }

        public SignUp()
        {
            InitializeComponent();
            ShowMessage = true;
            game = new PlayGame();
        }

        public void setMainWindow(MainWindow mainWin)
        {
            this.mainWindow = mainWin;
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
                    // Disconnect from the server.
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.mainWindow.ShowDialog();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           
           /* this.dbConnector = new DataBaseConnector();
            dbConnector.EstablishConnection("127.0.0.1", "root", "987654", "world_word_db");

            this.queryDA = new QueryDA(dbConnector);
            this.playerDA = new PlayersDA(dbConnector);

            Player player1 = null; //checkPlayer();
            Player player2 = null; //checkPlayer();
            this.playerDA.InsertNewUser("bananot bepigamot");
            //checkPlayer();  PlayersDA.RetrieveUser("firstPlayer")
            string[] categories = { "1","2" ,"3","4"};
            List<Question> questions = this.queryDA.QuestionsGeneration(categories);
            this.playerDA.DeleteUser("456");
            this.playerDA.UpdateHighScore("46", 20000);


            Game game = new Game(player1, player2, questions);
            Console.WriteLine("hereeeeeeeeeeee" + game.Questions.Count);*/

            this.Hide();
            this.game.ShowDialog();

        }
    }
}
