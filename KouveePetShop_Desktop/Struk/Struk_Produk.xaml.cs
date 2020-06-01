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

namespace KouveePetShop_Desktop.Struk
{
    
    /// <summary>
    /// Interaction logic for Struk_Produk.xaml
    /// </summary>
    
    public partial class Struk_Produk : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public string idTransaksi;
        public Struk_Produk()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnRefresh_Click(sender, e);
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("SELECT p.nama_produk, ph.harga, dp.jml_transaksi_produk, dp.total_harga from detailtransaksiproduks dp JOIN produkhargas ph ON dp.id_produkHarga = ph.id_produkHarga JOIN produks p ON ph.id_produk = p.id_produk JOIN transaksipenjualanproduks tp ON dp.kode_penjualan_produk = tp.kode_penjualan_produk WHERE tp.kode_penjualan_produk = '" + idTransaksi + "' ", conn);
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

        private void FillTextBoxMember()
        {
            // query nama customer
            string Query = "SELECT c.nama_customer FROM customers c JOIN hewans h on c.id_customer = h.id_customer JOIN transaksipenjualanproduks tp ON h.id_hewan = tp.id_hewan where tp.kode_penjualan_produk = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;

            // query nama hewan
            string Query2 = "SELECT h.nama_hewan FROM hewans h JOIN transaksipenjualanproduks tp ON h.id_hewan = tp.id_hewan where tp.kode_penjualan_produk = '" + idTransaksi + "'";
            MySqlCommand namaCmd2 = new MySqlCommand(Query2, conn);
            MySqlDataReader reader2;

            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    NamaCustomerText.Text = (reader["nama_customer"].ToString());
                    //NamaCustomerText.RefreshCurrent();
                    // NamaCustomerText.Text = reader.GetString("cr.NAMA_CUSTOMER");
                }
                reader.Close();

                reader2 = namaCmd2.ExecuteReader();

                while (reader2.Read())
                {
                    NamaHewanText.Text = (reader2["nama_hewan"].ToString());
                }

                reader2.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void FillTextBoxPegawai()
        {
            // query nama pegawai
            string Query = "SELECT pg1.nama_pegawai as nama_cs, pg2.nama_pegawai as nama_kasir FROM transaksipenjualanproduks tp JOIN pegawais pg1 ON tp.id_cs = pg1.NIP JOIN pegawais pg2 ON tp.id_kasir = pg2.NIP where tp.kode_penjualan_produk = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;


            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    CustomerServiceText.Text = (reader["nama_cs"].ToString());
                    KasirText.Text = (reader["nama_kasir"].ToString());
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void FillTextBoxTelp()
        {
            // query nomor customer
            string Query = "SELECT c.noTelp_customer as NOMOR FROM transaksipenjualanproduks tp JOIN hewans h ON tp.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer where tp.kode_penjualan_produk = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;


            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    TelpText.Text = (reader["NOMOR"].ToString());
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void FillTextBoxNoTransaksi()
        {
            // query nomor transaksi
            string Query = "SELECT kode_penjualan_produk FROM transaksipenjualanproduks where kode_penjualan_produk = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;


            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    NomorTransaksiText.Text = (reader["kode_penjualan_produk"].ToString());
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void FillTextBoxTanggal()
        {
            // query tanggal transaksi
            string Query = "SELECT createLog_at AS TANGGAL_TRANSAKSI FROM transaksipenjualanproduks where kode_penjualan_produk = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;


            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    TanggalTransaksiText.Text = Convert.ToDateTime(reader["TANGGAL_TRANSAKSI"]).ToString("dd MMMM yyyy");
                    //TanggalTransaksiText.Text = (reader["TANGGAL_TRANSAKSI"].ToString());
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void HitungSubTotalHarga()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT sum(dp.total_harga) as SUBTOTAL from detailtransaksiproduks dp JOIN transaksipenjualanproduks tp ON dp.kode_penjualan_produk = tp.kode_penjualan_produk JOIN hewans h ON tp.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer WHERE tp.kode_penjualan_produk = '" + idTransaksi + "'", conn);
            MySqlDataReader reader;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    SubTotalText.Text = (reader["SUBTOTAL"].ToString());
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

        private void HitungTotalDiskon()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT sum(tp.diskon) as DISKON from transaksipenjualanproduks tp JOIN hewans h ON tp.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer WHERE tp.kode_penjualan_produk = '" + idTransaksi + "'", conn);
            MySqlDataReader reader;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    DiskonText.Text = (reader["DISKON"].ToString());
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

        private void HitungTotalHarga()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT sum(dp.total_harga) as TOTAL from detailtransaksiproduks dp JOIN transaksipenjualanproduks tp ON dp.kode_penjualan_produk = tp.kode_penjualan_produk JOIN hewans h ON tp.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer WHERE tp.kode_penjualan_produk = '" + idTransaksi + "'", conn);
            MySqlDataReader reader;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    TotalText.Text = (reader["TOTAL"].ToString());
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

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                printDialog.PageRangeSelection = PageRangeSelection.AllPages;
                printDialog.UserPageRangeEnabled = true;
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

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            FillTextBoxMember();
            FillTextBoxPegawai();
            FillTextBoxTelp();
            FillTextBoxNoTransaksi();
            FillTextBoxTanggal();
            HitungSubTotalHarga();
            HitungTotalDiskon();
            HitungTotalHarga();
            TampilDataGrid();
            conn.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
