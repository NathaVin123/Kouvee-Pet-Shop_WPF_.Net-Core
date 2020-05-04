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
        public Produk()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";
                BindGrid();
                BindGridPegawai();
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
                cmd.CommandText = "SELECT id_produk AS 'ID Produk', nama_produk AS 'Nama Produk', harga_produk AS 'Harga Produk', stok_produk AS 'Stok Produk', min_stok_produk AS 'Min Stok Produk', satuan_produk AS 'Satuan Produk', gambar AS 'Gambar', updateLog_by as 'NIP' FROM produks";
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
            idprodukTxt.Text = "";
            namaprodukTxt.Text = "";
            hargaprodukTxt.Text = "";
            stokprodukTxt.Text = "";
            minimalstokTxt.Text = "";
            satuanprodukTxt.Text = "";
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

        public int id_produk_ai = 10;
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
            id_produk_ai++;
            string id_produk_ai_2 = id_produk_ai.ToString("PRDK0000");
            string nama_produk = namaprodukTxt.Text;
            string harga_produk = hargaprodukTxt.Text;
            string stok_produk = stokprodukTxt.Text;
            string min_stok_produk = minimalstokTxt.Text;
            string satuan_produk = satuanprodukTxt.Text;
            string updateLog_by = updatelogbyCb.Text;
            string id_produk = idprodukTxt.Text;

            if (/*idprodukTxt.Text != "" && */namaprodukTxt.Text != "" && hargaprodukTxt.Text != "" && stokprodukTxt.Text != "" && minimalstokTxt.Text != "" && satuanprodukTxt.Text != "" && updatelogbyCb.Text != "")
            {
                if (idprodukTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO produks(id_produk,nama_produk,harga_produk,stok_produk,min_stok_produk,satuan_produk,gambar,updateLog_by) VALUES (@id_produk,@nama_produk,@harga_produk,@stok_produk,@min_stok_produk,@satuan_produk,@gambar,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@id_produk", id_produk_ai_2);
                        cmd.Parameters.AddWithValue("@nama_produk", nama_produk);
                        cmd.Parameters.AddWithValue("@harga_produk", harga_produk);
                        cmd.Parameters.AddWithValue("@stok_produk", stok_produk);
                        cmd.Parameters.AddWithValue("@min_stok_produk", min_stok_produk);
                        cmd.Parameters.AddWithValue("@satuan_produk", satuan_produk);
                        cmd.Parameters.AddWithValue("@gambar", gambarBT);
                        cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.SelectedValue);
                        cmd.ExecuteNonQuery();
                        BindGrid();
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
                        cmd.CommandText = "UPDATE produks set id_produk = @id_produk, nama_produk = @nama_produk, harga_produk = @harga_produk, stok_produk = @stok_produk, min_stok_produk = @min_stok_produk, satuan_produk = @satuan_produk, gambar = @gambar, updateLog_By = @updateLog_by WHERE id_produk = @id_produk";
                        cmd.Parameters.AddWithValue("@id_produk", id_produk);
                        cmd.Parameters.AddWithValue("@nama_produk", nama_produk);
                        cmd.Parameters.AddWithValue("@harga_produk", harga_produk);
                        cmd.Parameters.AddWithValue("@stok_produk", stok_produk);
                        cmd.Parameters.AddWithValue("@min_stok_produk", min_stok_produk);
                        cmd.Parameters.AddWithValue("@satuan_produk", satuan_produk);
                        cmd.Parameters.AddWithValue("@gambar", gambarBT);
                        cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.SelectedValue);
                        cmd.ExecuteNonQuery();
                        BindGrid();
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
                satuanprodukTxt.Text = row["Satuan Produk"].ToString();
                updatelogbyCb.Text = row["NIP"].ToString();
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
            if (produkDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)produkDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "DELETE FROM produks where id_produk =" + row["ID Produk"].ToString();
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Produk berhasil di hapus");
                }
                catch
                {
                    MessageBox.Show("Terjadi kesalahan dalam menghapus data produk");
                }
                ClearAll();

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
                cmd.CommandText = "SELECT id_produk AS 'ID Produk', nama_produk AS 'Nama Produk', harga_produk AS 'Harga Produk', stok_produk AS 'Stok Produk', min_stok_produk AS 'Min Stok Produk', satuan_produk AS 'Satuan Produk', gambar AS 'Gambar', updateLog_by as 'NIP' FROM produks WHERE nama_produk = @nama_produk";

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
