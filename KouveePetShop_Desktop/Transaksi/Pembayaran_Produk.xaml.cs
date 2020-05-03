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
    /// Interaction logic for Pembayaran_Produk.xaml
    /// </summary>
    public partial class Pembayaran_Produk : Window
    {
        MySqlConnection conn;
        DataTable dt;
        public Pembayaran_Produk()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";
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
                cmd.CommandText = "SELECT dp.kode_penjualan AS 'Kode Penjualan', dp.id_produk AS 'ID Produk',dp.tgl_transaksi_produk AS 'Tanggal Transaksi Produk', tp.tgl_transaksi_penjualan AS 'Tanggal Transaksi Penjualan',dp.jml_transaksi_produk AS 'Jumlah Transaksi Produk', tp.nama_kasir AS 'Nama Kasir', dp.subtotal AS Subtotal, tp.status_transaksi AS 'Status Transaksi', tp.status_pembayaran AS 'Status Pembayaran', tp.id_customer AS 'ID Costumer', tp.id_CS AS 'ID CS', tp.id_Kasir AS 'ID Kasir', tp.total AS 'Total' FROM detailproduks dp JOIN transaksipenjualans tp ON dp.kode_penjualan = tp.kode_penjualan";
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
                cmd.CommandText = "SELECT dp.kode_penjualan AS 'Kode Penjualan', dp.id_produk AS 'ID Produk',dp.tgl_transaksi_produk AS 'Tanggal Transaksi Produk', tp.tgl_transaksi_penjualan AS 'Tanggal Transaksi Penjualan',dp.jml_transaksi_produk AS 'Jumlah Transaksi Produk', tp.nama_kasir AS 'Nama Kasir', dp.subtotal AS Subtotal, tp.status_transaksi AS 'Status Transaksi', tp.status_pembayaran AS 'Status Pembayaran', tp.id_customer AS 'ID Costumer', tp.id_CS AS 'ID CS', tp.id_Kasir AS 'ID Kasir', tp.total AS 'Total' FROM detailproduks dp JOIN transaksipenjualans tp ON dp.kode_penjualan = tp.kode_penjualan WHERE nama_kasir = @nama_kasir";

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
