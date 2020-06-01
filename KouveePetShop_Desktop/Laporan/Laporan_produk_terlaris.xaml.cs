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
    /// Interaction logic for Laporan_produk_terlaris.xaml
    /// </summary>
    public partial class Laporan_produk_terlaris : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;
        public Laporan_produk_terlaris()
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("SELECT DATE_FORMAT(date, '%M') AS month, NamaProduk, SUM(total) AS JumlahPenjualan FROM( SELECT tr.createLog_at AS date, p.NAMA_PRODUK as NamaProduk, dt.jml_transaksi_produk as total from transaksipenjualanproduks tr JOIN detailtransaksiproduks dt ON tr.kode_penjualan_produk = dt.kode_penjualan_produk JOIN produkhargas ph ON dt.id_produkHarga = ph.id_produkHarga JOIN produks p ON ph.id_produk = p.id_produk WHERE YEAR(tr.createLog_at) = YEAR(Now()) AND tr.status_transaksi = 'Lunas' UNION SELECT '2020-01-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-02-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-03-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-04-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-05-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-06-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-07-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-08-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-09-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-10-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-11-01' AS date, '-' AS NamaProduk, 0 AS total UNION SELECT '2020-12-01' AS date, '-' AS NamaProduk, 0 AS total) AS merged GROUP BY DATE_FORMAT(date, '%Y%m') ORDER BY date ASC", conn);
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

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            
            TampilDataGrid();
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
