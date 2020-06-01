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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace KouveePetShop_Desktop.Kasir
{
    /// <summary>
    /// Interaction logic for Transaksi_Layanan.xaml
    /// </summary>
    public partial class Transaksi_Layanan : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;
        public Transaksi_Layanan()
        {
            InitializeComponent();

            try
            {
                connection = "Server=localhost; User Id=root;Password=;Database=petshopd;Allow Zero Datetime=True";
                conn = new MySqlConnection(connection);
                conn.Open();
                SubTotal();
                Diskon();
                Total();
                TampilDataGrid();
                conn.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("select tl.kode_penjualan_layanan, h.nama_hewan, tl.diskon, sum(dl.total_harga) as TOTAL, tl.proses, tl.status_transaksi, tl.id_cs, pg1.nama_pegawai as nama1, pg2.nama_pegawai as nama2, tl.id_kasir from transaksipenjualanlayanans tl JOIN detailtransaksilayanans dl ON tl.kode_penjualan_layanan = dl.kode_penjualan_layanan JOIN hewans h on tl.id_hewan = h.id_hewan JOIN pegawais pg1 on tl.id_cs = pg1.NIP LEFT JOIN pegawais pg2 on tl.id_kasir = pg2.NIP JOIN customers c ON h.id_customer = c.id_customer where tl.kode_penjualan_layanan = dl.kode_penjualan_layanan AND tl.kode_penjualan_layanan LIKE 'LY%' GROUP BY tl.kode_penjualan_layanan ORDER BY tl.kode_penjualan_layanan DESC", conn);
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
            }
        }

        public void GetRecords()
        {
            conn.Open();
            SubTotal();
            Diskon();
            Total();
            TampilDataGrid();
            conn.Close();
        }

        private void SubTotal()
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE detailtransaksilayanans dl SET dl.total_harga = (SELECT sum(dl.jml_transaksi_layanan*lh.harga) FROM layananhargas lh WHERE dl.id_layananharga = lh.id_layananharga)";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Diskon()
        {
            try
            {
                using (MySqlCommand cmdMember = new MySqlCommand())
                {
                    cmdMember.Connection = conn;
                    cmdMember.CommandText = "UPDATE transaksipenjualanlayanans tl JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer SET tl.diskon = (SELECT sum(dl.total_harga * 10 / 100) FROM detailtransaksilayanans dl WHERE dl.kode_penjualan_layanan = tl.kode_penjualan_layanan) WHERE c.status LIKE 'Member' AND tl.kode_penjualan_layanan LIKE 'LY%'";
                    cmdMember.ExecuteNonQuery();
                }

                using (MySqlCommand cmdNonMember = new MySqlCommand())
                {
                    cmdNonMember.Connection = conn;
                    cmdNonMember.CommandText = "UPDATE transaksipenjualanlayanans tl JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer SET tl.diskon = 0 WHERE c.status LIKE 'Non Member' AND tl.kode_penjualan_layanan LIKE 'LY%'";
                    cmdNonMember.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Total()
        {
            try
            {
                using (MySqlCommand cmdMember = new MySqlCommand())
                {
                    cmdMember.Connection = conn;
                    cmdMember.CommandText = "UPDATE detailtransaksilayanans dl JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer SET dl.total_harga = (SELECT sum(total_harga - (total_harga * 10/100)) FROM detailtransaksilayanans dl2 WHERE dl2.id_detaillayanan = dl.id_detaillayanan) WHERE c.status LIKE 'Member'";
                    cmdMember.ExecuteNonQuery();
                }

                using (MySqlCommand cmdNonMember = new MySqlCommand())
                {
                    cmdNonMember.Connection = conn;
                    cmdNonMember.CommandText = "UPDATE detailtransaksilayanans dl JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer SET dl.total_harga = dl.total_harga WHERE c.status LIKE 'Non Member'";
                    cmdNonMember.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void CariTransaksiJasaLayananText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Fungsi untuk mencari transaksi sesuai nama
                // auto mencari data
                DataTable dt = new DataTable();
                MySqlDataAdapter adp = new MySqlDataAdapter("select tl.kode_penjualan_layanan, h.nama_hewan, tl.diskon, sum(dl.total_harga) as TOTAL, tl.proses, tl.status_transaksi, tl.id_cs, pg1.nama_pegawai as nama1, pg2.nama_pegawai as nama2, tr.id_kasir from transaksipenjualanlayanans tl JOIN detailtransaksilayanans dt ON tl.kode_penjualan_layanan = dl.kode_penjualan_layanan JOIN hewans h on tl.id_hewan = h.id_hewan JOIN pegawais pg1 on tr.id_cs = pg1.NIP LEFT JOIN pegawais pg2 on tl.id_kasir = pg2.NIP where tl.kode_penjualan_layanan LIKE 'LY%' AND h.nama_hewan LIKE '" + CariTransaksiJasaLayananText.Text + "%'", conn);

                adp.Fill(dt);
                DataGrid.DataContext = dt;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetRecords();
        }

        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            Layanan_Proses ui = new Layanan_Proses();
            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                ui.idTransaksi = selected_row["kode_penjualan_layanan"].ToString();
                ui.NamaHewanText.Text = selected_row["nama_hewan"].ToString();
                ui.Show();
            }
        }

        /*
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditHewanTransaksi edt = new EditHewanTransaksi();
            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                edt.idTransaksi = selected_row["ID_TRANSAKSI"].ToString();
                edt.NamaHewanText.Text = selected_row["NAMA_HEWAN"].ToString();

                edt.Show();
            }
        }*/

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            string message = "Apakah anda ingin menghapus data ini ?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            try
            {
                MySqlCommand cmd;
                MySqlCommand cmd2;
                DataRowView row = (DataRowView)((Button)e.Source).DataContext;

                string query = "Delete from transaksipenjualanlayanans where kode_penjualan_layanan = @1";
                cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter("@1", row["kode_penjualan_layanan"]));

                conn.Open();

                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    string queryDetail = "Delete from detailtransaksilayanans where kode_penjualan_layanan = @1";
                    cmd2 = new MySqlCommand(queryDetail, conn);
                    cmd2.Parameters.Add(new MySqlParameter("@1", row["kode_penjualan_layanan"]));
                    cmd2.ExecuteNonQuery();

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil dihapus", "Success");
                    TampilDataGrid();
                    conn.Close();
                }
                else
                {
                    conn.Close();
                    return;
                }

            }
            catch (Exception err)
            {
                if (err is ConstraintException || err is MySqlException)
                {
                    MessageBox.Show("Data ini masih digunakan oleh tabel yang lain, silahkan pilih data yang lainnya!", "Warning");
                    conn.Close();
                    return;
                }
                else
                {
                    MessageBox.Show(err.Message);
                    conn.Close();
                }
            }
        }
    }
}
