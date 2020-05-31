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
using System.Windows.Shapes;

namespace KouveePetShop_Desktop.Menu
{
    /// <summary>
    /// Interaction logic for Menu_Kasir.xaml
    /// </summary>
    public partial class Menu_Kasir : Window
    {
        public Menu_Kasir()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SendValueRole(RoleText.Text);
        }

        public void GetValueRole(string value)
        {
            RoleText.Text = value;
            // roleValue = RoleText.Text;           
        }

        public string SendValueRole(string value)
        {
            string roleValue;

            roleValue = value;

            return roleValue;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            string message = "Apakah anda ingin keluar?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                var Logout = new Login_KouveePetShop();
                Logout.Show();
                this.Close();
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Layanan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Produk_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
