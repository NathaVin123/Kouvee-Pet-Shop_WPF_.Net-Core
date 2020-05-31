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

namespace KouveePetShop_Desktop.Customer
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        MySqlConnection conn;
        DataTable dt;

        DateTime tanggal = DateTime.Now;
        public Customer()
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
                cmd.CommandText = "SELECT id_customer AS 'ID Customer', nama_customer AS 'Nama Customer', alamat_customer AS 'Alamat Customer', tglLahir_customer AS 'Tanggal Lahir', noTelp_customer AS 'No Telepon', updateLog_by AS 'Diubah Oleh' FROM customers WHERE aktif LIKE '1'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                customerDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    customerDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    customerDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data customer...");
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
                cmd.CommandText = "SELECT nama_customer AS 'Nama Customer', createLog_at AS 'Di Buat', updateLog_at AS 'Di Ubah', deleteLog_at AS 'Di Hapus' FROM customers";
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

        //public int id_customer_ai = 10;
        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
            string nama_customer = namacustomerTxt.Text;
            string alamat_customer = alamatcustomerTxt.Text;
            string tglLahir_customer = tanggallahirDp.SelectedDate.Value.ToString("yyyy-MM-dd");
            string noTelp_customer = noteleponTxt.Text;
            string updateLog_by = updatelogbyCb.Text;
            string id_customer = idcustomerTxt.Text;
            string updateLog_at = tanggal.ToString("yyyy-MM-dd H:mm:ss");

                if (namacustomerTxt.Text != "" && alamatcustomerTxt.Text != "" && tanggallahirDp.SelectedDate != null && noteleponTxt.Text != "" || string.IsNullOrEmpty(updatelogbyCb.Text))
            {
                if (idcustomerTxt.IsEnabled == true)
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO customers(nama_customer,alamat_customer,tglLahir_customer,noTelp_customer,updateLog_by) VALUES (@nama_customer,@alamat_customer,@tglLahir_customer,@noTelp_customer,@updateLog_by)";
                        cmd.Parameters.AddWithValue("@nama_customer", nama_customer);
                        cmd.Parameters.AddWithValue("@alamat_customer", alamat_customer);
                        cmd.Parameters.AddWithValue("@tglLahir_customer", tglLahir_customer);
                        cmd.Parameters.AddWithValue("@noTelp_customer", noTelp_customer);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Customer berhasil ditambahkan");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menambahkan data customer");
                    }
                    ClearAll();
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "UPDATE customers set nama_customer = @nama_customer, alamat_customer = @alamat_customer, tglLahir_customer = @tglLahir_customer, noTelp_customer = @noTelp_customer, updateLog_By = @updateLog_by, updateLog_at = @updateLog_at WHERE id_customer = @id_customer";
                        cmd.Parameters.AddWithValue("@id_customer", id_customer);
                        cmd.Parameters.AddWithValue("@nama_customer", nama_customer);
                        cmd.Parameters.AddWithValue("@alamat_customer", alamat_customer);
                        cmd.Parameters.AddWithValue("@tglLahir_customer", tglLahir_customer);
                        cmd.Parameters.AddWithValue("@noTelp_customer", noTelp_customer);
                        cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                        cmd.Parameters.AddWithValue("@updateLog_at", updateLog_at);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Customer berhasil di ubah");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam mengubah data customer");
                    }
                    ClearAll();
                }
            }
            else
            {
                    MessageBox.Show("Mohon data Customer harap dilengkapi");
                }
            }
            catch
            {
                MessageBox.Show("Mohon data Customer harap diisi");
            }
        }

        private void ClearAll()
        {
            idcustomerTxt.Text = "";
            namacustomerTxt.Text = "";
            alamatcustomerTxt.Text = "";
            tanggallahirDp.Text = "";
            noteleponTxt.Text = "";
            updatelogbyCb.Text = "";
            tambahBtn.Content = "Tambah";
            idcustomerTxt.IsEnabled = true;
        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (customerDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)customerDT.SelectedItems[0];
                idcustomerTxt.Text = row["ID Customer"].ToString();
                namacustomerTxt.Text = row["Nama Customer"].ToString();
                alamatcustomerTxt.Text = row["Alamat Customer"].ToString();
                tanggallahirDp.Text = row["Tanggal Lahir"].ToString();
                noteleponTxt.Text = row["No Telepon"].ToString();
                updatelogbyCb.SelectedValue = row["Diubah Oleh"].ToString();
                idcustomerTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data Customer yang mau diubah");
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

            if (customerDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)customerDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE customers set aktif = @aktif where id_customer =" + row["ID Customer"].ToString();
                        cmd.Parameters.AddWithValue("@aktif", aktif);
                        //cmd.Parameters.AddWithValue("@adeleteLog_at", deleteLog_at);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGridLog();
                        MessageBox.Show("Data Customer berhasil di soft delete");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menghapus data customer");
                    }
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Tolong pilih data Customer yang ingin dihapus");
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

            string nama_jenisHewan = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_customer", nama_jenisHewan);
                cmd.CommandText = "SELECT id_customer AS 'ID Customer', nama_customer AS 'Nama Customer', alamat_customer AS 'Alamat Customer', tglLahir_customer AS 'Tanggal Lahir', noTelp_customer AS 'No Telepon', updateLog_by AS 'Diubah Oleh' FROM customers WHERE nama_customer = @nama_customer";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                customerDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data hewan");
            }
        }
    }
}
