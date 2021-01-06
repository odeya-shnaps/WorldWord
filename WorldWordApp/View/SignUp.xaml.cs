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

        public SignUp()
        {
            InitializeComponent();
            ShowMessage = true;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
        }

        public void setMainWindow(MainWindow mainWin)
        {
            this.mainWindow = mainWin;
        }

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
                    mainWindow.Close_Game();
                    mainWindow.ShowMessage = false;
                    mainWindow.Close();
                }
            }
        }

        public void ResetDetailes()
        {
            wait.Text = "";
            name1.Text = "";
            name2.Text = "";
            olympics.IsChecked = false;
            demography.IsChecked = false;
            geography.IsChecked = false;
            people.IsChecked = false;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.mainWindow.ShowDialog();
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (name1.Text != "" && name2.Text != "")
            {
                wait.Text = "The Game will Start in a few Seconds...";
                GameLogic gl = mainWindow.gameLogic;
                gl.StartGame(name1.Text, name2.Text, GetCategories());
                game = new PlayGame();
                game.SetDataContext(gl);
                game.setMainWindow(mainWindow);
                this.Hide();
                this.game.Show();
                game.NextRound();
            }
            else
            {
                wait.Text = "Fill Names For Both Players";
                timer.Start();
            }
        }

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

        private void Timer_Tick(object sender, EventArgs e)
        {
            wait.Text = "";
            timer.Stop();
        }
    }
}
