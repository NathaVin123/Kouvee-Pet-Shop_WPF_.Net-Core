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
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Win32;

namespace KouveePetShop_Desktop.Produk
{
    /// <summary>
    /// Interaction logic for Produk.xaml
    /// </summary>
    public partial class Produk : Window
    {
        MySqlConnection conn;
        DataTable dt;

        DateTime tanggal = DateTime.Now;
        public Produk()
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
                FillComboBoxSatuan();
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
                cmd.CommandText = "SELECT id_produk AS 'ID Produk', nama_produk AS 'Nama Produk', harga_produk AS 'Harga Produk', stok_produk AS 'Stok Produk', min_stok_produk AS 'Min Stok Produk', satuan_produk AS 'Satuan Produk', gambar AS 'Gambar', updateLog_by as 'Diubah Oleh' FROM produks WHERE aktif LIKE '1'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                produkDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    produkDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    produkDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data produk...");
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
                cmd.CommandText = "SELECT nama_produk AS 'Nama Produk', createLog_at AS 'Di Buat', updateLog_at AS 'Di Ubah', deleteLog_at AS 'Di Hapus' FROM produks";
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
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data pegawai");
            }
            
        }

        private void ClearAll()
        {
            idprodukTxt.Text = "";
            namaprodukTxt.Text = "";
            hargaprodukTxt.Text = "";
            stokprodukTxt.Text = "";
            minimalstokTxt.Text = "";
            satuanCb.Text = "";
            updatelogbyCb.Text = "";
            tambahBtn.Content = "Tambah";
            idprodukTxt.IsEnabled = true;
        }

        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
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

        public void FillComboBoxSatuan()
        {
            string query = "SELECT satuan_produk FROM petshopd.produks;";

            MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader mySqlDataReader;

            try
            {
                mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    string satuan = mySqlDataReader.GetString("satuan_produk");
                    satuanCb.Items.Add(satuan);
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

            byte[] gambarBT = null;
            try
            {
            FileStream fs = new FileStream(this.GambarPath.Text, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            gambarBT = br.ReadBytes((int)fs.Length);
            }
            catch
            {
                MessageBox.Show("Mohon masukan gambar terlebih dahulu");
            }
            string nama_produk = namaprodukTxt.Text;
            string harga_produk = hargaprodukTxt.Text;
            string stok_produk = stokprodukTxt.Text;
            string min_stok_produk = minimalstokTxt.Text;
            string satuan_produk = satuanCb.Text;
            string updateLog_by = updatelogbyCb.Text;
            string id_produk = idprodukTxt.Text;
            string updateLog_at = tanggal.ToString("yyyy-MM-dd H:mm:ss");

            if (namaprodukTxt.Text != "" && hargaprodukTxt.Text != "" && stokprodukTxt.Text != "" && minimalstokTxt.Text != "" || string.IsNullOrEmpty(satuanCb.Text) || string.IsNullOrEmpty(updatelogbyCb.Text))
            {
                if (idprodukTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO produks(nama_produk,harga_produk,stok_produk,min_stok_produk,satuan_produk,gambar,updateLog_by) VALUES (@nama_produk,@harga_produk,@stok_produk,@min_stok_produk,@satuan_produk,@gambar,@updateLog_by)";
                        
                        cmd.Parameters.AddWithValue("@nama_produk", nama_produk);
                        cmd.Parameters.AddWithValue("@harga_produk", harga_produk);
                        cmd.Parameters.AddWithValue("@stok_produk", stok_produk);
                        cmd.Parameters.AddWithValue("@min_stok_produk", min_stok_produk);
                        cmd.Parameters.AddWithValue("@satuan_produk", satuan_produk);
                        cmd.Parameters.AddWithValue("@gambar", gambarBT);
                        cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.SelectedValue);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Produk berhasil ditambahkan");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menambahkan data produk");
                    }
                    ClearAll();
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "UPDATE produks set id_produk = @id_produk, nama_produk = @nama_produk, harga_produk = @harga_produk, stok_produk = @stok_produk, min_stok_produk = @min_stok_produk, satuan_produk = @satuan_produk, gambar = @gambar, updateLog_By = @updateLog_by, updateLog_at = @updateLog_at WHERE id_produk = @id_produk";
                        cmd.Parameters.AddWithValue("@id_produk", id_produk);
                        cmd.Parameters.AddWithValue("@nama_produk", nama_produk);
                        cmd.Parameters.AddWithValue("@harga_produk", harga_produk);
                        cmd.Parameters.AddWithValue("@stok_produk", stok_produk);
                        cmd.Parameters.AddWithValue("@min_stok_produk", min_stok_produk);
                        cmd.Parameters.AddWithValue("@satuan_produk", satuan_produk);
                        cmd.Parameters.AddWithValue("@gambar", gambarBT);
                        cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.SelectedValue);
                        cmd.Parameters.AddWithValue("@updateLog_at", updateLog_at);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Produk berhasil di ubah");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam mengubah data produk");
                    }
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Data Produk harap dilengkapi");
            }
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (produkDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)produkDT.SelectedItems[0];
                idprodukTxt.Text = row["ID Produk"].ToString();
                namaprodukTxt.Text = row["Nama Produk"].ToString();
                hargaprodukTxt.Text = row["Harga Produk"].ToString();
                stokprodukTxt.Text = row["Stok Produk"].ToString();
                minimalstokTxt.Text = row["Min Stok Produk"].ToString();
                satuanCb.Text = row["Satuan Produk"].ToString();
                updatelogbyCb.Text = row["Diubah Oleh"].ToString();
                idprodukTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data produk yang mau diubah");
            }
        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            int aktif = '0';
            //string deleteLog_at = tanggal.ToString("yyyy-MM-dd H:mm:ss");
            string message = "Apakah anda ingin menghapus data ini ?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (produkDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)produkDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd.CommandText = "UPDATE produks set aktif = @aktif where id_produk =" + row["ID Produk"].ToString();
                        cmd.Parameters.AddWithValue("@aktif", aktif);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Produk berhasil di soft delete");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menghapus data produk");
                    }
                    ClearAll();
                }
                

            }
            else
            {
                MessageBox.Show("Tolong pilih data produk yang ingin dihapus");
            }
        }

        private void Cari_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string nama_produk = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_produk", nama_produk);
                cmd.CommandText = "SELECT id_produk AS 'ID Produk', nama_produk AS 'Nama Produk', harga_produk AS 'Harga Produk', stok_produk AS 'Stok Produk', min_stok_produk AS 'Min Stok Produk', satuan_produk AS 'Satuan Produk', gambar AS 'Gambar', updateLog_by as 'Diubah Oleh' FROM produks WHERE nama_produk = @nama_produk";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                produkDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data produk");
            }   
        }

        private void GambarBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Multiselect = false;
            opf.Title = "Silahkan pilih gambar";
            opf.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (opf.ShowDialog() == true)
            {
                UploadImage.Source = new BitmapImage(new Uri(opf.FileName));
                string gambarpath = opf.FileName.ToString();
                GambarPath.Text = gambarpath;
            }
        }

        private void CariTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindGrid();
        }
    }
}
