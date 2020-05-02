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


namespace KouveePetShop_Desktop.Layanan
{
    /// <summary>
    /// Interaction logic for Layanan.xaml
    /// </summary>
    public partial class Layanan : Window
    {
        MySqlConnection conn;
        DataTable dt;
        public Layanan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";
                BindGrid();
                BindGridPegawai();
                BindGridUkuranHewan();
                FillComboBoxUkuranHewan();
                FillComboBoxNIP();
            }
            catch
            {
                MessageBox.Show("Tidak ada database...");
            }
           
        }

        private void BindGrid()
        {
            MySqlCommand cmd = new MySqlCommand();

            if(conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT id_layanan AS 'ID Layanan', nama_layanan AS 'Nama Layanan', harga_layanan AS 'Harga Layanan', id_ukuranHewan AS 'ID Ukuran Hewan', updateLog_by AS 'NIP' FROM layanans";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                layananDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    layananDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    layananDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data layanan...");
            }
        }

        private void BindGridPegawai()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            try
            {
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
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data pegawai");
            }
            
        }

        private void BindGridUkuranHewan()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT id_ukuranHewan AS 'ID Ukuran', nama_ukuranHewan AS 'Nama Ukuran' FROM ukuranhewans";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                uhDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    uhDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    uhDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data ukuran hewan");
            }
        }

        private void ClearAll()
        {
            idlayananTxt.Text = "";
            namalayananTxt.Text = "";
            hargalayananTxt.Text = "";
            idukuranhewanCb.Text = "";
            updatelogbyTxt.Text = "";
            tambahBtn.Content = "Tambah";
            idlayananTxt.IsEnabled = true;
        }

        public void FillComboBoxUkuranHewan()
        {
            string query = "SELECT id_ukuranHewan, nama_ukuranHewan FROM petshop.ukuranhewans;";
    
            MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader mySqlDataReader;

            try
            {
                mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    string idUkuran = mySqlDataReader.GetString("id_ukuranHewan");
                    string namaUkuran = mySqlDataReader.GetString("nama_ukuranHewan");
                    idukuranhewanCb.Items.Add(idUkuran + " - " + namaUkuran);
                }
                mySqlDataReader.Close();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public void FillComboBoxNIP()
        {
            string query = "SELECT NIP FROM petshop.pegawais;";

            MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader mySqlDataReader;

            try
            {
                mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    string NIP = mySqlDataReader.GetString("NIP");
                    updatelogbyCb.Items.Add(NIP);
                }
                mySqlDataReader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if(conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string id_layanan = idlayananTxt.Text;
            string nama_layanan = namalayananTxt.Text;
            string harga_layanan = hargalayananTxt.Text;
            string id_ukuranHewan = idukuranhewanCb.Text;
            string updateLog_by = updatelogbyCb.Text;

            if (idlayananTxt.Text != "" && namalayananTxt.Text != "" && hargalayananTxt.Text != "" && idukuranhewanCb.Text != "" && updatelogbyCb.Text != "")
            {
                if (idlayananTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO layanans(id_layanan,nama_layanan,harga_layanan,id_ukuranHewan,updateLog_by) VALUES (@id_layanan,@nama_layanan,@harga_layanan,@id_ukuranHewan,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@id_layanan", id_layanan);
                        cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
                        cmd.Parameters.AddWithValue("@harga_layanan", harga_layanan);
                        cmd.Parameters.AddWithValue("@id_ukuranHewan", idukuranhewanCb.SelectedValue);
                        cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.Text);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Data Layanan berhasil ditambahkan");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menambahkan data layanan");
                    }
                    
                    ClearAll();
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "UPDATE layanans set id_layanan = @id_layanan,nama_layanan = @nama_layanan, harga_layanan = @harga_layanan, id_ukuranHewan = @id_ukuranHewan, updateLog_By = @updateLog_by WHERE id_layanan = @id_layanan";
                        cmd.Parameters.AddWithValue("@id_layanan", id_layanan);
                        cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
                        cmd.Parameters.AddWithValue("@harga_layanan", harga_layanan);
                        cmd.Parameters.AddWithValue("@id_ukuranHewan", idukuranhewanCb.SelectedValue);
                        cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.SelectedValue);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Data Layanan berhasil di ubah");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam mengubah data layanan");
                    }
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Data layanan harap diisi dengan lengkap");
            }

        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if(layananDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)layananDT.SelectedItems[0];
                idlayananTxt.Text = row["ID Layanan"].ToString();
                namalayananTxt.Text = row["Nama Layanan"].ToString();
                hargalayananTxt.Text = row["Harga Layanan"].ToString();
                idukuranhewanCb.SelectedValue = row["ID Ukuran Hewan"].ToString();
                updatelogbyCb.SelectedValue = row["NIP"].ToString();
                idlayananTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data layanan yang mau diubah");
            }
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            if (layananDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)layananDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if(conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "DELETE FROM layanans where id_layanan =" + row["ID Layanan"].ToString();
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Layanan berhasil di hapus");
                }
                catch
                {
                    MessageBox.Show("Terjadi kesalahan dalam menghapus data layanan");
                }
                ClearAll();
                
            }
            else
            {
                MessageBox.Show("Tolong pilih data layanan yang mau dihapus");
            }
        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Cari_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string nama_layanan = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
                cmd.CommandText = "SELECT id_layanan AS 'ID Layanan', nama_layanan AS 'Nama Layanan', harga_layanan AS 'Harga Layanan', id_ukuranHewan AS 'ID Ukuran Hewan', updateLog_by AS 'NIP' FROM layanans WHERE nama_layanan = @nama_layanan";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                layananDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data layanan");
            }
        }

        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

    }
}
