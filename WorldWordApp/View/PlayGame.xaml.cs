using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
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
        private DispatcherTimer statusTimer;
        private int seconds;
        private GameLogic gameLogic;
        private int round;
        private bool endRound;
        private const int numRounds = 3;

        public PlayGame()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            statusTimer = new DispatcherTimer();
            statusTimer.Interval = TimeSpan.FromSeconds(2);
            statusTimer.Tick += Status_Timer_Tick;
            round = 0;
            ShowMessage = true;
            cheats.IsEnabled = false;
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

        public void SetAdminPermission()
        {
            cheats.IsEnabled = true;
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

        //player wants to submit an answer
        private void button_Click(object sender, RoutedEventArgs e)
        {
            // check the answer, if correct show appropriate resropnse and move to next question,
            // if not, show show appropriate resropnse for a few second and wait for other answer.
            bool ans = gameLogic.IsCorrectAnswer(answer.Text, seconds);
            if (ans == true)
            {
                timer.Stop();
                endRound = true;
            }
            else
            {
                endRound = false;
            }
            statusTimer.Start();
        }

        //player wants to change a question
        private void help_clicked(object sender, RoutedEventArgs e)
        {
            if (gameLogic.Life <= 0)
            {
                change_question.IsEnabled  = false;
            }
            else
            {
                gameLogic.ChangeQuestion();
                true_answer.Visibility = Visibility.Hidden;
                explain_button.Text = "see answer";
                answer.Text = "";
            }
        }

        // timer that show the time. if time ends show the correct answer and move to the next question.
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (seconds >= 0)
            {
                time.Text = seconds.ToString();
                seconds = seconds - 1;
            }
            else
            {
                timer.Stop();
                gameLogic.Status = "The Correct Answer Is:\n" + true_answer.Text;
                endRound = true;
                statusTimer.Start();
            }
        }

        // timer for showing the appropriate resropnse and if move to the next question if needed.
        private void Status_Timer_Tick(object sender, EventArgs e)
        {
            statusTimer.Stop();
            gameLogic.Status = "";
            if (endRound)
            {
                NextRound();
            }
        }

        // cheat for the admin that show and hide the answer in one click.
        private void seeAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (true_answer.Visibility == Visibility.Hidden)
            {
                true_answer.Visibility = Visibility.Visible;
                explain_button.Text = "hide answer";
            }
            else
            {
                true_answer.Visibility = Visibility.Hidden;
                explain_button.Text = "see answer";
            }
        }

        // move to the next round.
        public void NextRound()
        {
            round += 1;
            if (round == numRounds)
            {
                EndGame();
            }
            else
            {
                roundNum.Text = round.ToString();
                gameLogic.ChangeTurn();
                change_question.IsEnabled = true;
                true_answer.Visibility = Visibility.Hidden;
                explain_button.Text = "see answer";
                answer.Text = "";
                gameLogic.AskQuestion();
                seconds = 30;
                timer.Start();
            }

        }

        // ending the game, find the winner and move to winner window.
        private void EndGame()
        {
            answer.Text = "";
            missing_answer.Text = "";
            Winner winner = new Winner();
            winner.setMainWindow(mainWindow);
            // insert or update players and scores if needed.
            if (!gameLogic.EndeGame())
            {
                winner.isConnect = false;
            }
            List<Player> order = gameLogic.GetWinnerAndLoser();
            winner.SetWinnerAndScore(order[0], order[1], gameLogic.IsTie());
            ShowMessage = false;
            winner.Show();
            this.Close();
        }

        // show explenation of the ? button.
        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            explain_button.Visibility = Visibility.Visible;
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            explain_button.Visibility = Visibility.Hidden;
        }
        // show explenation of the ok button.
        private void submit_MouseEnter(object sender, MouseEventArgs e)
        {
            explain_button2.Visibility = Visibility.Visible;
        }

        private void submit_MouseLeave(object sender, MouseEventArgs e)
        {
            explain_button2.Visibility = Visibility.Hidden;
        }
    }
}
