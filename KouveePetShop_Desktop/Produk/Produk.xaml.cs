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

namespace KouveePetShop_Desktop.Produk
{
    /// <summary>
    /// Interaction logic for Produk.xaml
    /// </summary>
    public partial class Produk : Window
    {
        MySqlConnection conn;
        DataTable dt;
        public Produk()
        {
            InitializeComponent();

            conn = new MySqlConnection();
            conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";
            BindGrid();
            BindGridPegawai();
        }

        private void BindGrid()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT id_produk AS 'ID Produk', nama_produk AS 'Nama Produk', harga_produk AS 'Harga Produk', stok_produk AS 'Stok Produk', min_stok_produk AS 'Min Stok Produk', satuan_produk AS 'Satuan Produk', gambar AS 'Gambar', updateLog_by as 'NIP' FROM produks";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            produkDT.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                LabelCount.Visibility = System.Windows.Visibility.Hidden;
                produkDT.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                LabelCount.Visibility = System.Windows.Visibility.Visible;
                produkDT.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void BindGridPegawai()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT NIP AS 'NIP', nama_pegawai AS 'Nama Pegawai' FROM pegawais";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            nipDT.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                LabelCount.Visibility = System.Windows.Visibility.Hidden;
                nipDT.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                LabelCount.Visibility = System.Windows.Visibility.Visible;
                nipDT.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ClearAll()
        {
            idprodukTxt.Text = "";
            namaprodukTxt.Text = "";
            hargaprodukTxt.Text = "";
            stokprodukTxt.Text = "";
            minimalstokTxt.Text = "";
            satuanprodukTxt.Text = "";
            updatelogbyTxt.Text = "";
            tambahBtn.Content = "Tambah";
            idprodukTxt.IsEnabled = true;
        }


        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

        private void GambarBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
