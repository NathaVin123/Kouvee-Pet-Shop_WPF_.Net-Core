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
    /// Interaction logic for Struk_Layanan.xaml
    /// </summary>
    public partial class Struk_Layanan : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public string idTransaksi;

        public Struk_Layanan()
        {
            InitializeComponent();
      
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
            MySqlCommand cmd = new MySqlCommand("SELECT l.nama_layanan, lh.harga, dl.jml_transaksi_layanan, dl.total_harga from detailtransaksilayanans dl JOIN layananhargas lh ON dl.id_layananHarga = lh.id_layananHarga JOIN layanans l ON lh.id_layanan = l.id_layanan JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "' ", conn);
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
            string Query = "SELECT c.nama_customer FROM customers c JOIN hewans h on c.id_customer = h.id_customer JOIN transaksipenjualanlayanans tl ON h.id_hewan = tl.id_hewan where tl.kode_penjualan_layanan = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;

            // query nama hewan
            string Query2 = "SELECT h.nama_hewan FROM hewans h JOIN transaksipenjualanlayanans tl ON h.id_hewan = tl.id_hewan where tl.kode_penjualan_layanan = '" + idTransaksi + "'";
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
            string Query = "SELECT pg1.nama_pegawai as nama1, pg2.nama_pegawai as nama2 FROM transaksipenjualanlayanans tl JOIN pegawais pg1 ON tl.id_cs = pg1.NIP JOIN pegawais pg2 ON tl.id_kasir = pg2.NIP where tl.kode_penjualan_layanan = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;


            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    CustomerServiceText.Text = (reader["nama1"].ToString());
                    KasirText.Text = (reader["nama2"].ToString());
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
            string Query = "SELECT c.noTelp_customer as NOMOR FROM transaksipenjualanlayanans tl JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer where tl.kode_penjualan_layanan = '" + idTransaksi + "'";
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
            string Query = "SELECT kode_penjualan_layanan FROM transaksipenjualanlayanans where kode_penjualan_layanan = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;


            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    NomorTransaksiText.Text = (reader["kode_penjualan_layanan"].ToString());
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
            string Query = "SELECT createLog_at AS TANGGAL_TRANSAKSI FROM transaksipenjualanlayanans where kode_penjualan_layanan = '" + idTransaksi + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;


            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    TanggalTransaksiText.Text = Convert.ToDateTime(reader["TANGGAL_TRANSAKSI"]).ToString("dd MMMM yyyy");
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
            MySqlCommand cmd = new MySqlCommand("SELECT sum(dl.total_harga) as SUBTOTAL from detailtransaksilayanans dl JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "'", conn);
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
            MySqlCommand cmd = new MySqlCommand("SELECT sum(tl.diskon) as DISKON from transaksipenjualanlayanans tl JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "'", conn);
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
            MySqlCommand cmd = new MySqlCommand("SELECT sum(dl.total_harga) as TOTAL from detailtransaksilayanans dl JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "'", conn);
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

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnRefresh_Click(sender, e);
        }
    }
}
