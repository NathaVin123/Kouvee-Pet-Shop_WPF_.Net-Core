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
using System.Data;
using MySql.Data.MySqlClient;

namespace KouveePetShop_Desktop.Ukuran_Hewan
{
    /// <summary>
    /// Interaction logic for Ukuran_Hewan.xaml
    /// </summary>
    public partial class Ukuran_Hewan : Window
    {
        public Ukuran_Hewan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }
    }
}
