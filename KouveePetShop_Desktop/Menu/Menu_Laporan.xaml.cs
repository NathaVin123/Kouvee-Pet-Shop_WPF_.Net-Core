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
    /// Interaction logic for Menu_Laporan.xaml
    /// </summary>
    public partial class Menu_Laporan : Window
    {
        public Menu_Laporan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void PengadaanBulanan_Click(object sender, RoutedEventArgs e)
        {
            var PengadaanBulanan = new Laporan.Laporan_pengadaan_bulanan();
            PengadaanBulanan.Show();
            
        }

        private void PendapatanBulanan_Click(object sender, RoutedEventArgs e)
        {
            var PendapatanBulanan = new Laporan.Laporan_pendapatan_bulanan();
            PendapatanBulanan.Show();
            
        }

        private void ProdukTerlaris_Click(object sender, RoutedEventArgs e)
        {
            var ProdukTerlaris = new Laporan.Laporan_produk_terlaris();
            ProdukTerlaris.Show();
            
        }

        private void PengadaanTahunan_Click(object sender, RoutedEventArgs e)
        {
            var PengadaanTahunan = new Laporan.Laporan_pengadaan_tahunan();
            PengadaanTahunan.Show();
            
        }

        private void PendapatanTahunan_Click(object sender, RoutedEventArgs e)
        {
            var PendapatanTahunan = new Laporan.Laporan_pendapatan_tahunan();
            PendapatanTahunan.Show();
            
        }

        private void LayananTerlaris_Click(object sender, RoutedEventArgs e)
        {
            var LayananTerlaris = new Laporan.Laporan_layanan_terlaris();
            LayananTerlaris.Show();
            
        }
    }
}
