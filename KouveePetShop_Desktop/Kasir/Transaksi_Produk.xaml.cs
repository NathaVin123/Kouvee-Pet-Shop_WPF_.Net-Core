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
    /// Interaction logic for Transaksi_Produk.xaml
    /// </summary>
    public partial class Transaksi_Produk : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;
        public Transaksi_Produk()
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
            MySqlCommand cmd = new MySqlCommand("SELECT tr.kode_penjualan_produk, h.nama_hewan, tr.diskon, sum(dt.total_harga) as TOTAL, tr.status_transaksi, tr.id_cs, pg1.nama_pegawai as nama1, pg2.nama_pegawai as nama2, tr.id_kasir from transaksipenjualanproduks tr JOIN detailtransaksiproduks dt ON tr.kode_penjualan_produk = dt.kode_penjualan_produk JOIN hewans h on tr.id_hewan = h.id_hewan JOIN pegawais pg1 on tr.id_cs = pg1.NIP LEFT JOIN pegawais pg2 on tr.id_kasir = pg2.NIP where tr.kode_penjualan_produk = dt.kode_penjualan_produk AND tr.kode_penjualan_produk LIKE 'PR%' GROUP BY tr.kode_penjualan_produk ORDER BY tr.kode_penjualan_produk DESC ", conn);
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
                    cmd.CommandText = "UPDATE detailtransaksiproduks dt SET dt.total_harga = (SELECT sum(dt.jml_transaksi_produk*p.harga) FROM produkhargas p WHERE dt.id_produkHarga = p.id_produkHarga)";
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
                    cmdMember.CommandText = "UPDATE transaksipenjualanproduks tr JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer SET tr.diskon = (SELECT sum(dt.total_harga * 10 / 100) FROM detailtransaksiproduks dt WHERE dt.kode_penjualan_produk = tr.kode_penjualan_produk) WHERE cr.status LIKE 'Member' AND tr.kode_penjualan_produk LIKE 'PR%'";
                    cmdMember.ExecuteNonQuery();
                }

                using (MySqlCommand cmdNonMember = new MySqlCommand())
                {
                    cmdNonMember.Connection = conn;
                    cmdNonMember.CommandText = "UPDATE transaksipenjualanproduks tr JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer SET tr.diskon = 0 WHERE cr.status LIKE 'Non Member' AND tr.kode_penjualan_produk LIKE 'PR%'";
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
                    cmdMember.CommandText = "UPDATE detailtransaksiproduks dt JOIN transaksipenjualanproduks tr ON dt.kode_penjualan_produk = tr.kode_penjualan_produk JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer SET dt.total_harga = (SELECT sum(total_harga - (total_harga * 10/100)) FROM detailtransaksiproduks dt2 WHERE dt2.id_detailproduk = dt.id_detailproduk) WHERE cr.status LIKE 'Member'";
                    cmdMember.ExecuteNonQuery();
                }

                using (MySqlCommand cmdNonMember = new MySqlCommand())
                {
                    cmdNonMember.Connection = conn;
                    cmdNonMember.CommandText = "UPDATE detailtransaksiproduks dt JOIN transaksipenjualanproduks tr ON dt.kode_penjualan_produk = tr.kode_penjualan_produk JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer SET dt.total_harga = dt.total_harga WHERE cr.status LIKE 'Non Member'";
                    cmdNonMember.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void CariTransaksiProdukText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Fungsi untuk mencari transaksi sesuai nama
                // auto mencari data
                DataTable dt = new DataTable();
                MySqlDataAdapter adp = new MySqlDataAdapter("select tp.kode_penjualan_produk, h.nama_hewan, tp.diskon, dp.total_harga, tp.status_transaksi, tp.id_cs, pg1.nama_pegawai as nama1, pg2.nama_pegawai as nama2, tp.id_kasir from transaksipenjualanproduks tp JOIN detailtransaksiproduks dp ON tp.kode_penjualan_produk = dp.kode_penjualan_produk JOIN hewans h on tp.id_hewan = h.id_hewan JOIN pegawais pg1 on tp.id_cs = pg1.NIP OUTERJOIN pegawais pg2 on tp.id_kasir = pg2.NIP where tp.kode_penjualan_produk LIKE 'PR%' and h.nama_hewan LIKE '" + CariTransaksiProdukText.Text + "%'", conn);

                adp.Fill(dt);
                DataGrid.DataContext = dt;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            Detail_Dialog.Transaksi_Proses ui = new Detail_Dialog.Transaksi_Proses();
            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                ui.idTransaksi = selected_row["kode_penjualan_produk"].ToString();
                ui.NamaHewanText.Text = selected_row["nama_hewan"].ToString();
                ui.Show();
            }
        }

        /*
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditHewanTransaksi edt = new EditHewanTransaksi();
            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if(selected_row != null)
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

                string query = "Delete from transaksipenjualanproduks where kode_penjualan_produk = @1";
                cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter("@1", row["ID_TRANSAKSI"]));

                conn.Open();

                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    string queryDetail = "Delete from detailtransaksiproduks where kode_penjualan_produk = @1";
                    cmd2 = new MySqlCommand(queryDetail, conn);
                    cmd2.Parameters.Add(new MySqlParameter("@1", row["ID_TRANSAKSI"]));
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

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetRecords();
        }
    }
}
