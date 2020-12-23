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
            this.Hide();
            this.game.ShowDialog();
        }
    }
}
