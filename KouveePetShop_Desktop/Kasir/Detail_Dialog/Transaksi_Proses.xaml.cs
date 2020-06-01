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

namespace KouveePetShop_Desktop.Kasir.Detail_Dialog
{
    /// <summary>
    /// Interaction logic for Transaksi_Proses.xaml
    /// </summary>
    public partial class Transaksi_Proses : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public string idTransaksi;
        public string status;

        public Transaksi_Proses()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                connection = "Server=localhost; User Id=root;Password=;Database=petshopd;Allow Zero Datetime=True";
                conn = new MySqlConnection(connection);
                conn.Open();
                FillComboBoxNamaPegawai();
                conn.Close();

                Loaded += Window_Loaded;
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Warning");
            }
        }

        public void FillComboBoxNamaPegawai()
        {
            // Ambil ID dan Nama produk dari tabel produk ke combobox
            string Query = "SELECT NIP, nama_pegawai from petshopd.pegawais WHERE stat = 'Kasir';";
            MySqlCommand cmdComboBox = new MySqlCommand(Query, conn);
            MySqlDataReader reader;
            try
            {
                reader = cmdComboBox.ExecuteReader();

                while (reader.Read())
                {
                    string idPegawai = reader.GetString("NIP");
                    string namaPegawai = reader.GetString("nama_pegawai");
                    ComboBoxNamaKasir.Items.Add(idPegawai + " - " + namaPegawai);
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public void FillTextBox()
        {
            string Query = "SELECT * FROM customers cr JOIN hewans h on cr.id_customer = h.id_customer where h.nama_hewan LIKE '" + NamaHewanText.Text + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;

            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    string nama = (reader["nama_customer"].ToString());
                    //NamaCustomerText.RefreshCurrent();
                    // NamaCustomerText.Text = reader.GetString("cr.NAMA_CUSTOMER");
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("SELECT dt.id_detailproduk, p.nama_produk, dt.jml_transaksi_produk from detailtransaksiproduks dt JOIN produkhargas ph ON dt.id_produkHarga = ph.id_produkHarga JOIN produks p ON ph.id_produk = p.id_produk JOIN transaksipenjualanproduks tr ON dt.kode_penjualan_produk = tr.kode_penjualan_produk JOIN hewans h ON tr.id_hewan = h.id_hewan WHERE tr.kode_penjualan_produk = '" + idTransaksi + "' ", conn);
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

        private void HitungTotalDiskon()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT sum(tr.diskon) as DISKON from transaksipenjualanproduks tr JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer WHERE tr.kode_penjualan_produk = '" + idTransaksi + "'", conn);
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
            MySqlCommand cmd = new MySqlCommand("SELECT sum(dt.total_harga) as TOTAL from detailtransaksiproduks dt JOIN transaksipenjualanproduks tr ON dt.kode_penjualan_produk = tr.kode_penjualan_produk JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer WHERE tr.kode_penjualan_produk = '" + idTransaksi + "'", conn);
            MySqlDataReader reader;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    TotalHargaText.Text = (reader["TOTAL"].ToString());
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

        private void CekStatus()
        {
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT tr.status_transaksi as STATUS from transaksipenjualanproduks tr JOIN hewans h ON tr.id_hewan = h.id_hewan JOIN customers cr ON h.id_customer = cr.id_customer WHERE tr.status_transaksi = 'Lunas' AND tr.kode_penjualan_produk = '" + idTransaksi + "'", conn);
                MySqlDataReader reader;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string statusData = (reader["STATUS"].ToString());
                    status = statusData;
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnBayar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CekStatus();
                conn.Open();

                if (status == "Lunas")
                {
                    MessageBox.Show("Transaksi sudah dibayar!", "Warning");
                    conn.Close();
                    return;
                }
                else
                {
                    if (NamaCustomerText.Text == "")
                    {
                        MessageBox.Show("Silahkan refresh data terlebih dahulu", "Warning");
                        conn.Close();
                    }
                    else if (ComboBoxNamaKasir.SelectedIndex == -1)
                    {
                        MessageBox.Show("Field tidak boleh kosong", "Warning");
                        conn.Close();
                    }
                    else
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "UPDATE transaksipenjualanproduks tr, customers cr SET tr.status_transaksi = 'Lunas' WHERE tr.kode_penjualan_produk = '" + idTransaksi + "'";
                            cmd.ExecuteNonQuery();

                            UpdateStok();
                            MessageBox.Show("Berhasil dibayar!", "Success");
                            conn.Close();

                            Struk.Struk_Produk SP = new Struk.Struk_Produk();
                            SP.idTransaksi = idTransaksi;
                            SP.Show();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }

        }

        private void BtnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();

            // query nama customer
            string Query = "SELECT * FROM customers cr JOIN hewans h on cr.id_customer = h.id_customer " +
                "where h.nama_hewan LIKE '" + NamaHewanText.Text + "'";
            MySqlCommand namaCmd = new MySqlCommand(Query, conn);
            MySqlDataReader reader;

            // query status customer
            string Query2 = "SELECT * FROM customers cr JOIN hewans h on cr.id_customer = h.id_customer " +
                "where h.nama_hewan LIKE '" + NamaHewanText.Text + "'";
            MySqlCommand statusCmd = new MySqlCommand(Query2, conn);
            MySqlDataReader reader2;
            try
            {
                reader = namaCmd.ExecuteReader();

                while (reader.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    NamaCustomerText.Text = (reader["nama_customer"].ToString());
                    // NamaCustomerText.Text = reader.GetString("cr.NAMA_CUSTOMER");
                }
                reader.Close();

                reader2 = statusCmd.ExecuteReader();

                while (reader2.Read())
                {
                    //txtcomp.Text = (myReader["Comp_Name"].ToString());
                    StatusText.Text = (reader2["status"].ToString());
                    // NamaCustomerText.Text = reader.GetString("cr.NAMA_CUSTOMER");
                }
                reader2.Close();

                SubTotal();
                Diskon();
                Total();
                HitungTotalDiskon();
                HitungTotalHarga();
                TampilDataGrid();
                conn.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transaksi_TambahProduk tp = new Transaksi_TambahProduk();
            tp.idTransaksi = idTransaksi;

            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                string idDetail = selected_row["id_detailproduk"].ToString();
                tp.idDetail = idDetail;
                tp.Show();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Transaksi_EditProduk edtPr = new Transaksi_EditProduk();
            edtPr.idTransaksi = idTransaksi;

            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                string idDetail = selected_row["id_detailproduk"].ToString();
                edtPr.idDetail = idDetail;

                // Ambil jumlah dari tabel detail transaksi produk ke textbox
                conn.Open();
                // jumlah
                string Query = "select dt.jml_transaksi_produk as JUMLAH from detailtransaksiproduks dt JOIN transaksipenjualanproduks tr ON dt.kode_penjualan_produk = tr.kode_penjualan_produk WHERE tr.kode_penjualan_produk = '" + idTransaksi + "' AND dt.id_detailproduk = '" + idDetail + "'";
                MySqlCommand cmdTextBox = new MySqlCommand(Query, conn);
                MySqlDataReader reader;

                // nama produk
                string Query2 = "select p.nama_produk as NAMA from produks p JOIN produkhargas ph ON p.id_produk = ph.id_produk JOIN detailtransaksiproduks dt ON ph.id_produkHarga = dt.id_produkHarga JOIN transaksipenjualanproduks tr ON dt.kode_penjualan_produk = tr.kode_penjualan_produk WHERE tr.kode_penjualan_produk = '" + idTransaksi + "' AND dt.id_detailproduk = '" + idDetail + "'";
                MySqlCommand cmdTextBox2 = new MySqlCommand(Query2, conn);
                MySqlDataReader reader2;

                try
                {
                    // kirim value jumlah ke window berikutnya
                    reader = cmdTextBox.ExecuteReader();
                    while (reader.Read())
                    {
                        edtPr.JumlahProdukText.Text = reader.GetString("JUMLAH");
                    }
                    reader.Close();

                    // kirim value nama produk ke window berikutnya
                    reader2 = cmdTextBox2.ExecuteReader();
                    while (reader2.Read())
                    {
                        edtPr.NamaProdukText.Text = reader2.GetString("NAMA");
                    }
                    reader2.Close();

                    edtPr.namaHewan = NamaHewanText.Text;
                    conn.Close();
                    edtPr.Show();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    conn.Close();
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            string message = "Apakah anda ingin menghapus data ini ?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            try
            {
                MySqlCommand cmd;
                DataRowView row = (DataRowView)((Button)e.Source).DataContext;

                string queryDetail = "Delete from detailtransaksiproduks where id_detailproduk = @1";
                cmd = new MySqlCommand(queryDetail, conn);
                cmd.Parameters.Add(new MySqlParameter("@1", row["id_detailproduk"]));
                conn.Open();

                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
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

        private void SubTotal()
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE detailtransaksiproduks dt SET dt.total_harga = (SELECT sum(dt.jml_transaksi_produk*p.harga) FROM produkHargas p WHERE dt.id_produkHarga = p.id_produkHarga)";
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

        private void UpdateStok()
        {
            try
            {
                using (MySqlCommand cmdMember = new MySqlCommand())
                {
                    cmdMember.Connection = conn;
                    cmdMember.CommandText = "UPDATE produks p JOIN produkhargas ph ON p.id_produk = ph.id_produk JOIN detailtransaksiproduks dt ON ph.id_produkHarga = dt.id_produkHarga JOIN transaksipenjualanproduks tr ON dt.kode_penjualan_produk = tr.kode_penjualan_produk SET p.stok_produk = (SELECT SUM(stok_produk - dt.jml_transaksi_produk) FROM produks p2 WHERE p2.id_produk = p.id_produk) WHERE dt.kode_penjualan_produk = '" + idTransaksi + "'";
                    cmdMember.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnRefresh_Click(sender, e);
        }
    }
}
