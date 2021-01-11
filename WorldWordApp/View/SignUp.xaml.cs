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

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private MainWindow mainWindow;
        private PlayGame game;
        public bool ShowMessage { get; set; }
        public bool IsClosed { get; private set; }
        private DispatcherTimer timer;
        private DispatcherTimer reconnectTimer;
        private bool isConnect;

        public SignUp()
        {
            InitializeComponent();
            ShowMessage = true;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            reconnectTimer = new DispatcherTimer();
            reconnectTimer.Interval = TimeSpan.FromSeconds(1.5);
            reconnectTimer.Tick += Reconnect_Timer_Tick;
            isConnect = true;
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

        // reset for a new game
        public void ResetDetailes()
        {
            button1.IsEnabled = true;
            wait.Text = "";
            name1.Text = "";
            name2.Text = "";
            olympics.IsChecked = false;
            demography.IsChecked = false;
            geography.IsChecked = false;
            people.IsChecked = false;
            passwordBlock.Visibility = Visibility.Hidden;
            password.Visibility = Visibility.Hidden;
            incorrect.Text = "";
            isAdmin.IsChecked = false;
            reconnect.Visibility = Visibility.Hidden;

        }

        // back to main window
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnect)
            {
                mainWindow.ConnectionFailed();
            }
            this.Hide();
            this.mainWindow.ShowDialog();
            
        }
        // submit the details that the user enter.
        // if they are valid starting the game, if not - message to inform the user.
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool correctPass;
            // if want admit permission, check if the password correct
            if (isAdmin.IsChecked == true)
            {
                if (password.Password.Equals("1111"))
                {
                    incorrect.Text = "Correct Password";
                    correctPass = true;
                }
                else
                {
                    incorrect.Visibility = Visibility.Visible;
                    incorrect.Text = "Incorrect Password";
                    correctPass = false;
                }
            }
            else
            {
                correctPass = true;
            }
            // continue to the game only if there is 2 names and if admin - password correct.
            if (name1.Text != "" && name2.Text != "" && correctPass)
            {
                wait.Text = "The Game will Start in a few Seconds...";
                GameLogic gl = mainWindow.gameLogic;
                // if disconnect, show message to user and ask him to reconnect
                if (gl.StartGame(name1.Text, name2.Text, GetCategories()))
                {
                    game = new PlayGame();
                    game.SetDataContext(gl);
                    game.setMainWindow(mainWindow);
                    // set the admin permission
                    if (isAdmin.IsChecked == true)
                    {
                        game.SetAdminPermission();
                    }
                    this.Hide();
                    this.game.Show();
                    game.NextRound();
                }
                else
                {
                    mainWindow.isConnect = false;
                    isConnect = false;
                    button1.IsEnabled = false;
                    wait.Text = "Failed Connecting To DB, please try again...";
                    reconnect.Visibility = Visibility.Visible;
                    reconnect.IsEnabled = true;
                }
            }
            else
            {
                if (name1.Text == "" || name2.Text == "")
                {
                    wait.Text = "Fill Names For Both Players";
                    timer.Start();
                }
            }
        }

        // create list with the categories that the user choose, if didn't choose any we put all of them.
        private string[] GetCategories()
        {
            List<string> categories = new List<string>();
            if (olympics.IsChecked == true)
            {
                categories.Add("1");
            }
            if (demography.IsChecked == true)
            {
                categories.Add("2");
            }
            if (geography.IsChecked == true)
            {
                categories.Add("3");
            }
            if (people.IsChecked == true)
            {
                categories.Add("4");
            }
            if (categories.Count == 0)
            {
                categories.Add("1");
                categories.Add("2");
                categories.Add("3");
                categories.Add("4");
            }
            return categories.ToArray();
        }

        // timer for the message to the user.
        private void Timer_Tick(object sender, EventArgs e)
        {
            wait.Text = "";
            timer.Stop();
        }

        // show password text block and text box to enter the password
        private void isAdmin_Checked(object sender, RoutedEventArgs e)
        {
            passwordBlock.Visibility = Visibility.Visible;
            password.Visibility = Visibility.Visible;
        }

        // hide password text block and text box
        private void isAdmin_Unchecked(object sender, RoutedEventArgs e)
        {
            password.Password = "";
            passwordBlock.Visibility = Visibility.Hidden;
            password.Visibility = Visibility.Hidden;
            incorrect.Text = "";
        }

        // tring to reconnect to the db, if can't show message to user and ask him to reconnect.
        private void reconnect_Click(object sender, RoutedEventArgs e)
        {
            wait.Text = "Trying To Connect...";
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
                wait.Text = "Failed Connecting To DB, please try again...";
                reconnect.Visibility = Visibility.Visible;
            }
            // succeed reconnect
            else
            {
                mainWindow.isConnect = true;
                button1.IsEnabled = true;
                wait.Text = "Connected! press SUBMIT to start play";
                reconnect.Visibility = Visibility.Hidden;
            }
        }
    }
}
