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

namespace KouveePetShop_Desktop.Jenis_Hewan
{
    /// <summary>
    /// Interaction logic for Jenis_Hewan.xaml
    /// </summary>
    public partial class Jenis_Hewan : Window
    {
        MySqlConnection conn;
        DataTable dt;
        DateTime tanggal = DateTime.Now;
        public Jenis_Hewan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshopd;UID=root;PASSWORD=; Allow Zero Datetime=True";
                BindGrid();
                BindGridLog();
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

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT id_jenisHewan AS 'ID Jenis Hewan', nama_jenisHewan AS 'Nama Jenis Hewan', updateLog_by AS 'Diubah Oleh' FROM jenishewans WHERE aktif LIKE '1'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                jenishewanDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    jenishewanDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    jenishewanDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data jenis hewan...");
            }
            
        }

        private void BindGridLog()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT nama_jenisHewan AS 'Nama Jenis Hewan', createLog_at AS 'Di Buat', updateLog_at AS 'Di Ubah', deleteLog_at AS 'Di Hapus' FROM jenishewans";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                logDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    logDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    logDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data log");
            }
        }

        private void ClearAll()
        {
            idjenishewanTxt.Text = "";
            namajenishewanTxt.Text = "";
            updatelogbyCb.Text = "";
            tambahBtn.Content = "Tambah";
            idjenishewanTxt.IsEnabled = true;
        }

        public void FillComboBoxNIP()
        {
            string query = "SELECT NIP FROM petshopd.pegawais;";

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
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            { 
            string nama_jenisHewan = namajenishewanTxt.Text;
            string updateLog_by = updatelogbyCb.Text;
            string id_jenisHewan = idjenishewanTxt.Text;
            string updateLog_at = tanggal.ToString("yyyy-MM-dd H:mm:ss");

            if (namajenishewanTxt.Text != "" || string.IsNullOrEmpty(updatelogbyCb.Text))
            {
                if (idjenishewanTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO jenishewans(nama_jenisHewan,updateLog_by) VALUES (@nama_jenisHewan,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@nama_jenisHewan", nama_jenisHewan);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Data Jenis Hewan berhasil ditambahkan");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menambahkan data jenis hewan");
                    }
                    ClearAll();
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "UPDATE jenishewans set id_jenisHewan = @id_jenisHewan,nama_jenisHewan = @nama_jenisHewan,updateLog_By = @updateLog_by, updateLog_at = @updateLog_at WHERE id_jenisHewan = @id_jenisHewan";
                        cmd.Parameters.AddWithValue("@id_jenisHewan", id_jenisHewan);
                        cmd.Parameters.AddWithValue("@nama_jenisHewan", nama_jenisHewan);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.Parameters.AddWithValue("@updateLog_at", updateLog_at);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Data Jenis Hewan berhasil di ubah");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam mengubah data jenis hewan");
                    }
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Mohon data Jenis Hewan harap diisi");
            }
            }
            catch
            {
                MessageBox.Show("Mohon data Jenis Hewan harap diisi");
            }
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (jenishewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)jenishewanDT.SelectedItems[0];
                idjenishewanTxt.Text = row["ID Jenis Hewan"].ToString();
                namajenishewanTxt.Text = row["Nama Jenis Hewan"].ToString();
                updatelogbyCb.SelectedValue = row["Diubah Oleh"].ToString();
                idjenishewanTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data jenis hewan yang mau diubah");
            }
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            int aktif = '0';
            //string deleteLog_at = tanggal.ToString("yyyy-MM-dd H:mm:ss");
            string message = "Apakah anda ingin menghapus data ini ?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            if (jenishewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)jenishewanDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE jenishewans set aktif = @aktif where id_jenisHewan =" + row["ID Jenis Hewan"].ToString();
                        cmd.Parameters.AddWithValue("@aktif", aktif);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Jenis Hewan berhasil di hapus");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menghapus data jenis hewan");
                    }
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Tolong pilih data jenis hewan yang mau dihapus");
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

            string nama_jenisHewan = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_jenisHewan", nama_jenisHewan);
                cmd.CommandText = "SELECT id_jenisHewan AS 'ID Jenis Hewan', nama_jenisHewan AS 'Nama Jenis Hewan', updateLog_by AS 'Diubah Oleh' FROM jenishewans WHERE nama_jenisHewan = @nama_jenisHewan";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                jenishewanDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data jenis hewan");
            }
        }

        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

        private void CariTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindGrid();
        }
    }
}
