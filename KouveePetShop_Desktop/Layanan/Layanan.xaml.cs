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

        DateTime tanggal = DateTime.Now;
        public Layanan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshopd;UID=root;PASSWORD=;";
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

            if(conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT id_layanan AS 'ID Layanan', nama_layanan AS 'Nama Layanan', updateLog_by AS 'Diubah Oleh' FROM layanans WHERE aktif LIKE '1'";
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

        private void BindGridLog()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT nama_layanan AS 'Nama Layanan', createLog_at AS 'Di Buat', updateLog_at AS 'Di Ubah', deleteLog_at AS 'Di Hapus' FROM layanans";
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
            idlayananTxt.Text = "";
            namalayananTxt.Text = "";
            updatelogbyCb.Text = "";
            tambahBtn.Content = "Tambah";
            idlayananTxt.IsEnabled = true;
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
            if(conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string nama_layanan = namalayananTxt.Text;
            string updateLog_by = updatelogbyCb.Text;
            string id_layanan = idlayananTxt.Text;

            if (namalayananTxt.Text != "" || string.IsNullOrEmpty(updatelogbyCb.Text))
            {
                if (idlayananTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO layanans(nama_layanan,updateLog_by) VALUES (@nama_layanan,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
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
                        cmd.CommandText = "UPDATE layanans set nama_layanan = @nama_layanan, updateLog_By = @updateLog_by WHERE id_layanan = @id_layanan";
                        cmd.Parameters.AddWithValue("@id_layanan", id_layanan);
                        cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
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
                updatelogbyCb.SelectedValue = row["Diubah Oleh"].ToString();
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
            int aktif = '0';
            string message = "Apakah anda ingin menghapus data ini ?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            if (layananDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)layananDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if(conn.State != ConnectionState.Open)
                    conn.Open();
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE layanans set aktif = @aktif where id_layanan =" + row["ID Layanan"].ToString();
                        cmd.Parameters.AddWithValue("@aktif", aktif);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Layanan berhasil di hapus");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menghapus data layanan");
                    }
                    ClearAll();
                }
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
                cmd.CommandText = "SELECT id_layanan AS 'ID Layanan', nama_layanan AS 'Nama Layanan', updateLog_by AS 'Diubah Oleh' FROM layanans WHERE nama_layanan = @nama_layanan";

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

        private void CariTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindGrid();
        }
    }
}