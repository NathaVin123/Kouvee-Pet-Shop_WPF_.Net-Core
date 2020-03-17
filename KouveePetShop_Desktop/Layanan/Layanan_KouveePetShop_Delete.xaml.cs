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
    /// Interaction logic for Layanan_KouveePetShop_Delete.xaml
    /// </summary>
    public partial class Layanan_KouveePetShop_Delete : Window
    {
        public Layanan_KouveePetShop_Delete()
        {
            InitializeComponent();
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            var TambahLayanan = new Layanan_KouveePetShop();
            TambahLayanan.Show();
            this.Close();
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            var UpdateLayanan = new Layanan_KouveePetShop_Update();
            UpdateLayanan.Show();
            this.Close();
        }
    }
}
