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

namespace KouveePetShop_Desktop
{
    /// <summary>
    /// Interaction logic for Layanan_KouveePetShop_Update.xaml
    /// </summary>
    public partial class Layanan_KouveePetShop_Update : Window
    {
        public Layanan_KouveePetShop_Update()
        {
            InitializeComponent();
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            var TambahLayanan = new Layanan_KouveePetShop();
            TambahLayanan.Show();
            this.Close();
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            var HapusLayanan = new Layanan_KouveePetShop_Delete();
            HapusLayanan.Show();
            this.Close();
        }
    }
}
