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
        public Supplier()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;Allow Zero Datetime=True";
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
                cmd.CommandText = "SELECT id_supplier AS 'ID Supplier', nama_supplier AS 'Nama Supplier', alamat_supplier AS 'Alamat Supplier', telepon_supplier AS 'No Telepon', stok_supplier AS 'Stok Supplier', updateLog_by AS 'NIP' FROM suppliers";
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

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string id_supplier = idsupplierTxt.Text;
            string nama_supplier = namasupplierTxt.Text;
            string alamat_supplier = alamatsupplierTxt.Text;
            string telepon_supplier = noteleponTxt.Text;
            string stok_supplier = stokTxt.Text;
            string updateLog_by = updatelogbyTxt.Text;

            if (idsupplierTxt.Text != "" && namasupplierTxt.Text != "" && updatelogbyTxt.Text != "")
            {
                if (idsupplierTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO suppliers(id_supplier,nama_supplier,alamat_supplier,telepon_supplier,stok_supplier,updateLog_by) VALUES (@id_supplier,@nama_supplier,@alamat_supplier,@telepon_supplier,@stok_supplier,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@id_supplier", id_supplier);
                        cmd.Parameters.AddWithValue("@nama_supplier", nama_supplier);
                        cmd.Parameters.AddWithValue("@alamat_supplier", alamat_supplier);
                        cmd.Parameters.AddWithValue("@telepon_supplier", telepon_supplier);
                        cmd.Parameters.AddWithValue("@stok_supplier", stok_supplier);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
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
                        cmd.CommandText = "UPDATE suppliers set id_supplier = @id_supplier, nama_supplier = @nama_supplier, alamat_supplier = @alamat_supplier, telepon_supplier = @telepon_supplier, stok_supplier = @stok_supplier, updateLog_By = @updateLog_by WHERE id_supplier = @id_supplier";
                        cmd.Parameters.AddWithValue("@id_supplier", id_supplier);
                        cmd.Parameters.AddWithValue("@nama_supplier", nama_supplier);
                        cmd.Parameters.AddWithValue("@alamat_supplier", alamat_supplier);
                        cmd.Parameters.AddWithValue("@telepon_supplier", telepon_supplier);
                        cmd.Parameters.AddWithValue("@stok_supplier", stok_supplier);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
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

        private void ClearAll()
        {
            idsupplierTxt.Text = "";
            namasupplierTxt.Text = "";
            alamatsupplierTxt.Text = "";
            noteleponTxt.Text = "";
            stokTxt.Text = "";
            updatelogbyTxt.Text = "";
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
                stokTxt.Text = row["Stok Supplier"].ToString();
                updatelogbyTxt.Text = row["NIP"].ToString();
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
            if (supplierDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)supplierDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "DELETE FROM suppliers where id_supplier =" + row["ID Supplier"].ToString();
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Supplier berhasil di hapus");
                }
                catch
                {
                    MessageBox.Show("Terjadi kesalahan dalam menghapus data supplier");
                }
                ClearAll();

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
    }
}
