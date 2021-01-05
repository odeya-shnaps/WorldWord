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
using WorldWordApp.Game_Logic;
using WorldWordApp.Objects;

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for PlayGame.xaml
    /// </summary>
    public partial class PlayGame : Window
    {
        private MainWindow mainWindow;
        public bool ShowMessage { get; set; }
        private DispatcherTimer timer;
        private int seconds;
        private GameLogic gameLogic;
        private int round;

        public PlayGame()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            round = 0;
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

        //player wants to submit an anser
        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool ans = gameLogic.IsCorrectAnswer(answer.Text, seconds);
            if (ans == true)
            {
                timer.Stop();
                EndRound();
            }
        }

        //player asked to change a question
        private void help_clicked(object sender, RoutedEventArgs e)
        {
            if (gameLogic.Life <= 0)
            {
                change_question.IsEnabled  = false;
            }
            else
            {
                gameLogic.ChangeQuestion();
            }
        }

        public void SetDataContext(GameLogic gl)
        {
            DataContext = gl;
            gameLogic = gl;
        }

        public void setMainWindow(MainWindow mainWin)
        {
            this.mainWindow = mainWin;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (seconds >= 0)
            {
                //time.Content = (DateTime.Now - start_time).ToString("ss");
                time.Text = seconds.ToString();
                seconds = seconds - 1;
            }
            else
            {
                timer.Stop();
                EndRound();
            }

        }

        private void seeAnswer_Click(object sender, RoutedEventArgs e)
        {
            true_answer.Visibility = Visibility.Visible;
        }

        public void StartGame()
        {
            round += 1;
            change_question.IsEnabled = true;
            true_answer.Visibility = Visibility.Hidden;
            gameLogic.AskQuestion();
            seconds = 30;
            timer.Start();
        }

        private void EndRound()
        {
            //sleep!!!
            // status, correct answer,...
            round += 1;
            if (round == 4)
            {
                EndGame();
            }
            else
            {
                gameLogic.ChangeTurn();
                change_question.IsEnabled = true;
                true_answer.Visibility = Visibility.Hidden;
                answer.Text = "";
                gameLogic.AskQuestion();
                seconds = 30;
                timer.Start();
            }

        }

        private void EndGame()
        {
            //gameLogic.UpdateOrInsertPlayers();
            Winner winner = new Winner();
            List<Player> order = gameLogic.GetWinnerAndLoser();
            winner.SetWinnerAndScore(order[0], order[1], gameLogic.IsTie());
            winner.setMainWindow(mainWindow);
            ShowMessage = false;
            winner.Show();
            this.Close();
        }


    }
}
