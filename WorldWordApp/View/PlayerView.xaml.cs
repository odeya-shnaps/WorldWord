using System;
using System.Collections.Generic;
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

namespace WorldWordApp.View
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class PlayerView : Page
    {
        private NavigationService ns;

        public PlayerView()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }
    }
}
