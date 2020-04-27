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

namespace KouveePetShop_Desktop.Ukuran_Hewan
{
    /// <summary>
    /// Interaction logic for Ukuran_Hewan.xaml
    /// </summary>
    public partial class Ukuran_Hewan : Window
    {
        MySqlConnection conn;
        DataTable dt;
        public Ukuran_Hewan()
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
                cmd.CommandText = "SELECT id_ukuranHewan AS 'ID Ukuran Hewan', nama_ukuranHewan AS 'Nama Ukuran Hewan', updateLog_by AS 'NIP' FROM ukuranhewans";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                ukuranHewanDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    ukuranHewanDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    ukuranHewanDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data ukuran hewan...");
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
            idukuranTxt.Text = "";
            namaukuranTxt.Text = "";
            updatelogbyTxt.Text = "";
            tambahBtn.Content = "Tambah";
            idukuranTxt.IsEnabled = true;
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string id_ukuranHewan = idukuranTxt.Text;
            string nama_ukuranHewan = namaukuranTxt.Text;
            string updateLog_by = updatelogbyTxt.Text;

            if (idukuranTxt.Text != "" && namaukuranTxt.Text != "" && updatelogbyTxt.Text != "")
            {
                if (idukuranTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO ukuranhewans(id_ukuranHewan,nama_ukuranHewan,updateLog_by) VALUES (@id_ukuranHewan,@nama_ukuranHewan,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@id_ukuranHewan", id_ukuranHewan);
                        cmd.Parameters.AddWithValue("@nama_ukuranHewan", nama_ukuranHewan);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Data Ukuran Hewan berhasil ditambahkan");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menambahkan data ukuran hewan");
                    }

                    ClearAll();
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "UPDATE ukuranhewans set id_ukuranHewan = @id_ukuranHewan, nama_ukuranHewan = @nama_ukuranHewan, updateLog_By = @updateLog_by WHERE id_ukuranHewan = @id_ukuranHewan";
                        cmd.Parameters.AddWithValue("@id_ukuranHewan", id_ukuranHewan);
                        cmd.Parameters.AddWithValue("@nama_ukuranHewan", nama_ukuranHewan);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Data Ukuran Hewan berhasil di ubah");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam mengubah data ukuran hewan");
                    }
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Data ukuran hewan harap diisi dengan lengkap");
            }
        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (ukuranHewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)ukuranHewanDT.SelectedItems[0];
                idukuranTxt.Text = row["ID Ukuran Hewan"].ToString();
                namaukuranTxt.Text = row["Nama Ukuran Hewan"].ToString();
                updatelogbyTxt.Text = row["NIP"].ToString();
                idukuranTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data ukuran hewan yang mau diubah");
            }
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            if (ukuranHewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)ukuranHewanDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "DELETE FROM ukuranhewans where id_ukuranHewan =" + row["ID Ukuran Hewan"].ToString();
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data ukuran hewan berhasil di hapus");
                }
                catch
                {
                    MessageBox.Show("Terjadi kesalahan dalam menghapus ukuran hewan");
                }
                ClearAll();

            }
            else
            {
                MessageBox.Show("Tolong pilih data ukuran hewan yang mau dihapus");
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
