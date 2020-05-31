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

namespace KouveePetShop_Desktop.Transaksi
{
    /// <summary>
    /// Interaction logic for Pembayaran_Jasa_Layanan.xaml
    /// </summary>
    public partial class Pembayaran_Jasa_Layanan : Window
    {
        MySqlConnection conn;
        DataTable dt;
        public Pembayaran_Jasa_Layanan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshopd;UID=root;PASSWORD=;";
                BindGrid();
            }
            catch
            {
                MessageBox.Show("Tidak ada database...");
            }
        }

        private void BindGrid()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT dl.kode_penjualan AS 'Kode Penjualan', tp.tgl_transaksi_penjualan AS 'Tanggal Transaksi Penjualan', dl.id_layanan AS 'ID Layanan', l.nama_layanan AS 'Nama Layanan', tp.id_customer AS 'ID Costumer', c.nama_customer AS 'Nama Customer', tp.id_Kasir AS 'ID Kasir', tp.nama_kasir AS 'Nama Kasir', tp.status_transaksi AS 'Status Transaksi', tp.status_pembayaran AS 'Status Pembayaran', dl.tgl_transaksi_layanan AS 'Tanggal Transaksi Layanan', dl.jml_transaksi_Layanan AS 'Jumlah Transaksi Produk', dl.subtotal AS Subtotal, tp.total AS 'Total' FROM detaillayanans dl JOIN transaksipenjualans tp ON dl.kode_penjualan = tp.kode_penjualan JOIN layanans l ON dl.id_layanan = l.id_layanan JOIN customers c ON tp.id_customer = c.id_customer";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                PembayaranProdukDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    PembayaranProdukDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    PembayaranProdukDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data Pembayaran Produk...");
            }
        }

        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

        private void CariTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindGrid();
        }

        private void Cari_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string nama_kasir = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_kasir", nama_kasir);
                cmd.CommandText = "SELECT dl.kode_penjualan AS 'Kode Penjualan', tp.tgl_transaksi_penjualan AS 'Tanggal Transaksi Penjualan', dl.id_layanan AS 'ID Layanan', l.nama_layanan AS 'Nama Layanan', tp.id_customer AS 'ID Costumer', c.nama_customer AS 'Nama Customer', tp.id_Kasir AS 'ID Kasir', tp.nama_kasir AS 'Nama Kasir', tp.status_transaksi AS 'Status Transaksi', tp.status_pembayaran AS 'Status Pembayaran', dl.tgl_transaksi_layanan AS 'Tanggal Transaksi Layanan', dl.jml_transaksi_Layanan AS 'Jumlah Transaksi Produk', dl.subtotal AS Subtotal, tp.total AS 'Total' FROM detaillayanans dl JOIN transaksipenjualans tp ON dl.kode_penjualan = tp.kode_penjualan JOIN layanans l ON dl.id_layanan = l.id_layanan JOIN customers c ON tp.id_customer = c.id_customer WHERE nama_kasir = @nama_kasir";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                PembayaranProdukDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data pembayaran produk");
            }
        }
    }
}