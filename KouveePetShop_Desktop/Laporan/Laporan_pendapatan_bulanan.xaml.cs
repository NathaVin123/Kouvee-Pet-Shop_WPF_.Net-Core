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
using MySql.Data.MySqlClient;
using System.Data;

namespace KouveePetShop_Desktop.Laporan
{
    /// <summary>
    /// Interaction logic for Laporan_pendapatan_bulanan.xaml
    /// </summary>
    public partial class Laporan_pendapatan_bulanan : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public string idTransaksi;

        public Laporan_pendapatan_bulanan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            try
            {
                connection = "Server=localhost; User Id=root;Password=;Database=petshopd;Allow Zero Datetime=True";
                conn = new MySqlConnection(connection);

                Loaded += Window_Loaded;
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Warning");
            }
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("SELECT JasaLayanan, SUM(harga) AS Harga FROM ( SELECT tr.createLog_at AS date, l.nama_layanan AS JasaLayanan, SUM(dt.total_harga) AS harga from transaksipenjualanlayanans tr JOIN detailtransaksilayanans dt ON tr.kode_penjualan_layanan = dt.kode_penjualan_layanan JOIN layananhargas lh ON dt.id_layananHarga = lh.id_layananHarga JOIN layanans l ON lh.id_layanan = l.id_layanan WHERE MONTH(tr.createLog_at) = 5 AND YEAR(tr.createLog_at) = YEAR(NOW()) AND tr.status_transaksi = 'Lunas' GROUP BY tr.kode_penjualan_layanan ) AS m GROUP BY JasaLayanan", conn);
            try
            {
                //conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                DataGrid.DataContext = dt;
            }
            catch (MySqlException d)
            {
                MessageBox.Show(d.Message);
                conn.Close();
            }
        }

        private void TampilDataGrid2()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("SELECT Produk, SUM(harga) AS Harga FROM ( SELECT tr.createLog_at AS date, p.nama_produk AS Produk, SUM(dt.total_harga) AS harga from transaksipenjualanproduks tr JOIN detailtransaksiproduks dt ON tr.kode_penjualan_produk = dt.kode_penjualan_produk JOIN produkhargas ph ON dt.id_produkHarga = ph.id_produkHarga JOIN produks p ON ph.id_produk = ph.id_produk WHERE MONTH(tr.createLog_at) = 5 AND YEAR(tr.createLog_at) = YEAR(NOW()) AND tr.status_transaksi = 'Lunas' AND tr.kode_penjualan_produk = dt.kode_penjualan_produk GROUP BY tr.kode_penjualan_produk, p.nama_produk) AS m GROUP BY Produk", conn);
            try
            {
                //conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                DataGrid2.DataContext = dt;
            }
            catch (MySqlException d)
            {
                MessageBox.Show(d.Message);
                conn.Close();
            }
        }

        private void FillTotalLayanan()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT sum(dt.total_harga) as TOTAL from detailtransaksilayanans dt JOIN transaksipenjualanlayanans tr ON dt.kode_penjualan_layanan = tr.kode_penjualan_layanan JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer WHERE tr.kode_penjualan_layanan = dt.kode_penjualan_layanan AND tr.status_transaksi = 'Lunas' AND MONTH(tr.createLog_at) = 5", conn);
            MySqlDataReader reader;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    TotalLayanan.Text = (reader["TOTAL"].ToString());
                    // NamaCustomerText.Text = reader.GetString("cr.NAMA_CUSTOMER");
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }
        }

        private void FillTotalProduk()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT sum(dt.total_harga) as TOTAL from detailtransaksilayanans dt JOIN transaksipenjualanlayanans tr ON dt.kode_penjualan_layanan = tr.kode_penjualan_layanan JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer WHERE tr.kode_penjualan_layanan = dt.kode_penjualan_layanan AND tr.status_transaksi = 'Lunas' AND MONTH(tr.createLog_at) = 5", conn);
            MySqlDataReader reader;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    TotalProduk.Text = (reader["TOTAL"].ToString());
                    // NamaCustomerText.Text = reader.GetString("cr.NAMA_CUSTOMER");
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            
            TampilDataGrid();
            TampilDataGrid2();
            FillTotalLayanan();
            FillTotalProduk();
            conn.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnRefresh_Click(sender, e);
        }

        private void BtnPrint_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "invoice");
                }
                this.Close();
            }
            finally
            {
                this.IsEnabled = true;
            }
        }
    }
}
