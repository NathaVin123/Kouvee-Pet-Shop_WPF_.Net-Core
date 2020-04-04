using KouveePetShop_Desktop.Jenis_Hewan;
using KouveePetShop_Desktop.Produk;
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
    /// Interaction logic for Menu_KouveePetShop.xaml
    /// </summary>
    public partial class Menu_KouveePetShop : Window
    {
        public Menu_KouveePetShop()
        {
            InitializeComponent();
        }

        private void Hewan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Costumer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pembayaran_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Layanan_Click(object sender, RoutedEventArgs e)
        {
            var Layanan = new Layanan.Layanan();
            Layanan.Show();
            this.Close();

        }

        private void Pegawai_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Produk_Click(object sender, RoutedEventArgs e)
        {
            var Produk = new Produk.Produk();
            Produk.Show();
            this.Close();
        }

        private void JenisHewan_Click(object sender, RoutedEventArgs e)
        {
            var JenisHewan = new JenisHewan_KouveePetShop_Delete();
            JenisHewan.Show();
            this.Close();
        }
    }
}
