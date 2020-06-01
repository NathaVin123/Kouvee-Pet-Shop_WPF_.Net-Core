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

namespace KouveePetShop_Desktop.Kasir
{
    /// <summary>
    /// Interaction logic for Layanan_Proses.xaml
    /// </summary>
    public partial class Layanan_Proses : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public string idTransaksi;
        public string status;

        public Layanan_Proses()
        {
            InitializeComponent();
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
            string Query = "SELECT * FROM customers c JOIN hewans h on c.id_customer = h.id_customer where h.nama_hewan LIKE '" + NamaHewanText.Text + "'";
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
            MySqlCommand cmd = new MySqlCommand("SELECT dl.id_detaillayanan, l.nama_layanan, dl.jml_transaksi_layanan from detailtransaksilayanans dl JOIN layananhargas lh ON dl.id_layananHarga = lh.id_layananHarga JOIN layanans l ON lh.id_layanan = l.id_layanan JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan JOIN hewans h ON tl.id_hewan = h.id_hewan WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "' ", conn);
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

                MySqlCommand cmd = new MySqlCommand("SELECT tl.status_transaksi STATUS from transaksipenjualanlayanans tl JOIN hewans h ON tl.id_hewan = h.id_hewan JOIN customers c ON h.id_customer = c.id_customer WHERE tl.status_transaksi = 'Lunas' AND tl.kode_penjualan_layanan = '" + idTransaksi + "'", conn);
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Detail_Dialog.Transaksi_TambahLayanan tj = new Detail_Dialog.Transaksi_TambahLayanan();
            tj.idTransaksi = idTransaksi;

            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                string idDetail = selected_row["id_detaillayanan"].ToString();
                tj.idDetail = idDetail;
                tj.Show();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Detail_Dialog.Transaksi_EditLayanan edtJs = new Detail_Dialog.Transaksi_EditLayanan();
            edtJs.idTransaksi = idTransaksi;

            DataRowView selected_row = DataGrid.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                string idDetail = selected_row["id_detaillayanan"].ToString();
                edtJs.idDetail = idDetail;

                conn.Open();
                // Ambil jumlah dari tabel detail transaksi jasa layanan ke textbox
                string Query = "select dl.jml_transaksi_layanan as JUMLAH from detailtransaksilayanans dl JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "' AND dl.id_detaillayanan = '" + idDetail + "'";
                MySqlCommand cmdTextBox = new MySqlCommand(Query, conn);
                MySqlDataReader reader;

                // nama jasa layanan
                string Query2 = "select l.nama_layanan as 'Nama Layanan' from layanans l JOIN layananhargas lh ON l.id_layanan = lh.id_layanan JOIN detailtransaksilayanans dl ON lh.id_layananharga = dl.id_layananharga JOIN transaksipenjualanlayanans tl ON dl.kode_penjualan_layanan = tl.kode_penjualan_layanan WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "' AND dl.id_detaillayanan = '" + idDetail + "'";
                MySqlCommand cmdTextBox2 = new MySqlCommand(Query2, conn);
                MySqlDataReader reader2;

                try
                {
                    // kirim value jumlah ke window berikutnya
                    reader = cmdTextBox.ExecuteReader();
                    while (reader.Read())
                    {
                        edtJs.JumlahJasaLayananText.Text = reader.GetString("JUMLAH");
                    }
                    reader.Close();

                    // kirim value nama produk ke window berikutnya
                    reader2 = cmdTextBox2.ExecuteReader();
                    while (reader2.Read())
                    {
                        edtJs.NamaJasaLayananText.Text = reader2.GetString("NAMA");
                    }
                    reader2.Close();

                    conn.Close();
                    edtJs.Show();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    conn.Close();
                }
            }
        }
        /*
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

                string queryDetail = "Delete from detail_transaksi_jasalayanan where ID_DETAILTRANSAKSI_JASALAYANAN = @1";
                cmd = new MySqlCommand(queryDetail, conn);
                cmd.Parameters.Add(new MySqlParameter("@1", row["ID_DETAILTRANSAKSI_JASALAYANAN"]));
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
        */
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
                            cmd.CommandText = "UPDATE transaksipenjualanlayanans tl, customers c SET tl.status_transaksi = 'Lunas' WHERE tl.kode_penjualan_layanan = '" + idTransaksi + "'";
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Berhasil dibayar!", "Success");
                            conn.Close();

                            Struk.Struk_Layanan SP = new Struk.Struk_Layanan();
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

            {
                conn.Open();

                // query nama customer
                string Query = "SELECT * FROM customers c JOIN hewans h on c.id_customer = h.id_customer where h.nama_hewan LIKE '" + NamaHewanText.Text + "'";
                MySqlCommand namaCmd = new MySqlCommand(Query, conn);
                MySqlDataReader reader;

                // query status customer
                string Query2 = "SELECT * FROM customers c JOIN hewans h on c.id_customer = h.id_customer where h.nama_hewan LIKE '" + NamaHewanText.Text + "'";
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnRefresh_Click(sender, e);
        }
    }
}
