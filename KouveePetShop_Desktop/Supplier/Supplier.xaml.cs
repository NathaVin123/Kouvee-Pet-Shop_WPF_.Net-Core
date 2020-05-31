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

namespace KouveePetShop_Desktop.Supplier
{
    /// <summary>
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Supplier : Window
    {
        MySqlConnection conn;
        DataTable dt;

        DateTime tanggal = DateTime.Now;
        public Supplier()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshopd;UID=root;PASSWORD=;Allow Zero Datetime=True";
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
                cmd.CommandText = "SELECT id_supplier AS 'ID Supplier', nama_supplier AS 'Nama Supplier', alamat_supplier AS 'Alamat Supplier', telepon_supplier AS 'No Telepon', updateLog_by AS 'Diubah Oleh' FROM suppliers WHERE aktif LIKE '1'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                supplierDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    supplierDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    supplierDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data supplier...");
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
                cmd.CommandText = "SELECT nama_supplier AS 'Nama Supplier', createLog_at AS 'Di Buat', updateLog_at AS 'Di Ubah', deleteLog_at AS 'Di Hapus' FROM suppliers";
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
                string id_supplier = idsupplierTxt.Text;
                string nama_supplier = namasupplierTxt.Text;
                string alamat_supplier = alamatsupplierTxt.Text;
                string telepon_supplier = noteleponTxt.Text;
                string updateLog_by = updatelogbyCb.Text;
                if (namasupplierTxt.Text != "" && alamatsupplierTxt.Text != "" && noteleponTxt.Text != "" || string.IsNullOrEmpty(updatelogbyCb.Text))
                {
                    if (idsupplierTxt.IsEnabled == true)
                    {
                        try
                        {
                            cmd.CommandText = "INSERT INTO suppliers(nama_supplier,alamat_supplier,telepon_supplier,updateLog_by) VALUES (@nama_supplier,@alamat_supplier,@telepon_supplier,@updateLog_by)";
                            cmd.Parameters.AddWithValue("@nama_supplier", nama_supplier);
                            cmd.Parameters.AddWithValue("@alamat_supplier", alamat_supplier);
                            cmd.Parameters.AddWithValue("@telepon_supplier", telepon_supplier);
                            cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.SelectedValue);
                            cmd.ExecuteNonQuery();
                            BindGrid();
                            BindGridLog();
                            MessageBox.Show("Data Supplier berhasil ditambahkan");
                        }
                        catch
                        {
                            MessageBox.Show("Terjadi kesalahan dalam menambahkan data supplier");
                        }
                        ClearAll();
                    }
                    else
                    {
                        try
                        {
                            cmd.CommandText = "UPDATE suppliers set id_supplier = @id_supplier, nama_supplier = @nama_supplier, alamat_supplier = @alamat_supplier, telepon_supplier = @telepon_supplier, updateLog_By = @updateLog_by WHERE id_supplier = @id_supplier";
                            cmd.Parameters.AddWithValue("@id_supplier", id_supplier);
                            cmd.Parameters.AddWithValue("@nama_supplier", nama_supplier);
                            cmd.Parameters.AddWithValue("@alamat_supplier", alamat_supplier);
                            cmd.Parameters.AddWithValue("@telepon_supplier", telepon_supplier);
                            cmd.Parameters.AddWithValue("@updateLog_by", updatelogbyCb.SelectedValue);
                            cmd.ExecuteNonQuery();
                            BindGrid();
                            BindGridLog();
                            MessageBox.Show("Data Supplier berhasil di ubah");
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
                    MessageBox.Show("Mohon data Supplier harap diisi");
                }
            }
            catch
            {
                MessageBox.Show("Mohon data supplier harap diisi");
            }
        }

        private void ClearAll()
        {
            idsupplierTxt.Text = "";
            namasupplierTxt.Text = "";
            alamatsupplierTxt.Text = "";
            noteleponTxt.Text = "";
            updatelogbyCb.Text = "";
            tambahBtn.Content = "Tambah";
            idsupplierTxt.IsEnabled = true;
        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (supplierDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)supplierDT.SelectedItems[0];
                idsupplierTxt.Text = row["ID Supplier"].ToString();
                namasupplierTxt.Text = row["Nama Supplier"].ToString();
                alamatsupplierTxt.Text = row["Alamat Supplier"].ToString();
                noteleponTxt.Text = row["No Telepon"].ToString();
                updatelogbyCb.SelectedValue = row["Diubah Oleh"].ToString();
                idsupplierTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data Supplier yang mau diubah");
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

            if (supplierDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)supplierDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE suppliers set aktif = @aktif where id_supplier =" + row["ID Supplier"].ToString();
                        cmd.Parameters.AddWithValue("@aktif", aktif);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Supplier berhasil di hapus");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menghapus data supplier");
                    }
                    ClearAll();
                }
                

            }
            else
            {
                MessageBox.Show("Tolong pilih data Supplier yang ingin dihapus");
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

        private void Cari_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string nama_supplier = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_supplier", nama_supplier);
                cmd.CommandText = "SELECT id_supplier AS 'ID Supplier', nama_supplier AS 'Nama Supplier', alamat_supplier AS 'Alamat Supplier', telepon_supplier AS 'No Telepon', updateLog_by AS 'Diubah Oleh' FROM suppliers WHERE nama_supplier = @nama_supplier";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                supplierDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data supplier");
            }
        }
    }
}
