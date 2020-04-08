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
        public Jenis_Hewan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";
                BindGrid();
                BindGridPegawai();
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
                cmd.CommandText = "SELECT id_jenisHewan AS 'ID Jenis Hewan', nama_jenisHewan AS 'Nama Jenis Hewan', updateLog_by AS 'NIP' FROM jenishewans";
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

        private void ClearAll()
        {
            idjenishewanTxt.Text = "";
            namajenishewanTxt.Text = "";
            updatelogbyTxt.Text = "";
            tambahBtn.Content = "Tambah";
            idjenishewanTxt.IsEnabled = true;
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string id_jenisHewan = idjenishewanTxt.Text;
            string nama_jenisHewan = namajenishewanTxt.Text;
            string updateLog_by = updatelogbyTxt.Text;

            if (idjenishewanTxt.Text != "" && namajenishewanTxt.Text != "" && updatelogbyTxt.Text != "")
            {
                if (idjenishewanTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO jenishewans(id_jenisHewan,nama_jenisHewan,updateLog_by) VALUES (@id_jenisHewan,@nama_jenisHewan,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@id_jenisHewan", id_jenisHewan);
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
                        cmd.CommandText = "UPDATE jenishewans set id_jenisHewan = @id_jenisHewan,nama_jenisHewan = @nama_jenisHewan,updateLog_By = @updateLog_by WHERE id_jenisHewan = @id_jenisHewan";
                        cmd.Parameters.AddWithValue("@id_jenisHewan", id_jenisHewan);
                        cmd.Parameters.AddWithValue("@nama_jenisHewan", nama_jenisHewan);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
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

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (jenishewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)jenishewanDT.SelectedItems[0];
                idjenishewanTxt.Text = row["ID Jenis Hewan"].ToString();
                namajenishewanTxt.Text = row["Nama Jenis Hewan"].ToString();
                updatelogbyTxt.Text = row["NIP"].ToString();
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
            if (jenishewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)jenishewanDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "DELETE FROM jenishewans where id_jenisHewan =" + row["ID Jenis Hewan"].ToString();
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Jenis Hewan berhasil di hapus");
                }
                catch
                {
                    MessageBox.Show("Terjadi kesalahan dalam menghapus data jenis hewan");
                }
                ClearAll();
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
                cmd.CommandText = "SELECT id_jenisHewan AS 'ID Jenis Hewan', nama_jenisHewan AS 'Nama Jenis Hewan', updateLog_by AS 'NIP' FROM jenishewans WHERE nama_jenisHewan = @nama_jenisHewan";

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
    }
}
