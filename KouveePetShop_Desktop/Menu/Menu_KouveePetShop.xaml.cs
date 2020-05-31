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

        private void Hewan_Click(object sender, RoutedEventArgs e)
        {
            var Hewan = new Hewan.Hewan();
            Hewan.Show();
            this.Close();
        }

        private void Costumer_Click(object sender, RoutedEventArgs e)
        {
            var Customer = new Customer.Customer();
            Customer.Show();
            this.Close();
        }

        private void Pembayaran_Click(object sender, RoutedEventArgs e)
        {
            var PembayaranProduk = new Transaksi.Pembayaran_Produk();
            PembayaranProduk.Show();
            this.Close();
        }

        private void Layanan_Click(object sender, RoutedEventArgs e)
        {
            var Layanan = new Layanan.Layanan();
            Layanan.Show();
            this.Close();

        }

        private void Pegawai_Click(object sender, RoutedEventArgs e)
        {
            var Pegawai = new Pegawai.Pegawai();
            Pegawai.Show();
            this.Close();
        }

        private void Produk_Click(object sender, RoutedEventArgs e)
        {
            var Produk = new Produk.Produk();
            Produk.Show();
            this.Close();
        }

        private void JenisHewan_Click(object sender, RoutedEventArgs e)
        {
            var JenisHewan = new Jenis_Hewan.Jenis_Hewan();
            JenisHewan.Show();
            this.Close();
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

        private void Supplier_Click(object sender, RoutedEventArgs e)
        {
            var Supplier = new Supplier.Supplier();
            Supplier.Show();
            this.Close();
        }

        private void UkuranHewan_Click(object sender, RoutedEventArgs e)
        {
            var UkuranHewan = new Ukuran_Hewan.Ukuran_Hewan();
            UkuranHewan.Show();
            this.Close();
        }

        private void Pembayaran1_Click(object sender, RoutedEventArgs e)
        {
            var PembayaranLayanan = new Transaksi.Pembayaran_Jasa_Layanan();
            PembayaranLayanan.Show();
            this.Close();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
