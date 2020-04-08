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

namespace KouveePetShop_Desktop.Hewan
{
    /// <summary>
    /// Interaction logic for Hewan.xaml
    /// </summary>
    public partial class Hewan : Window
    {
        MySqlConnection conn;
        DataTable dt;
        public Hewan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;Allow Zero Datetime=True";
                BindGrid();
                BindGridPegawai();
                BindGridCustomer();
                BindGridJenisHewan();
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
                cmd.CommandText = "SELECT id_hewan AS 'ID Hewan', nama_hewan AS 'Nama Hewan', tglLahir_hewan AS 'Tanggal Lahir', id_customer AS 'ID Customer', id_jenisHewan AS 'ID Jenis Hewan', updateLog_by AS 'NIP' FROM hewans";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                hewanDT.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    LabelCount.Visibility = System.Windows.Visibility.Hidden;
                    hewanDT.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelCount.Visibility = System.Windows.Visibility.Visible;
                    hewanDT.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data hewan...");
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
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data supplier");
            }
            
        }

        private void BindGridCustomer()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT id_customer AS 'ID Cust', nama_customer AS 'Nama Cust' FROM customers";
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
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data customer");
            }
            
        }

        private void BindGridJenisHewan()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = "SELECT id_jenisHewan AS 'ID Jenis', nama_jenisHewan AS 'Nama Jenis' FROM jenishewans";
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
                MessageBox.Show("Terjadi kesalahan dalam menampilkan data jenis hewan");
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
                string id_hewan = idhewanTxt.Text;
                string nama_hewan = namahewanTxt.Text;
                string tglLahir_hewan = tanggallahirDp.SelectedDate.Value.ToString("yyyy-MM-dd");
                string id_customer = idcustomerTxt.Text;
                string id_jenisHewan = idjenishewanTxt.Text;
                string updateLog_by = updatelogbyTxt.Text;

                if (idhewanTxt.Text != "" && namahewanTxt.Text != "" && updatelogbyTxt.Text != "")
                {
                    if (idcustomerTxt.IsEnabled == true)
                    {
                        try
                        {
                            cmd.CommandText = "INSERT INTO hewans(id_hewan,nama_hewan,tglLahir_hewan,id_customer,id_jenisHewan,updateLog_by) VALUES (@id_hewan,@nama_hewan,@tglLahir_hewan,@id_customer,@id_jenisHewan,@updateLog_by)";
                            cmd.Parameters.AddWithValue("@id_hewan", id_hewan);
                            cmd.Parameters.AddWithValue("@nama_hewan", nama_hewan);
                            cmd.Parameters.AddWithValue("@tglLahir_hewan", tglLahir_hewan);
                            cmd.Parameters.AddWithValue("@id_customer", id_customer);
                            cmd.Parameters.AddWithValue("@id_jenisHewan", id_jenisHewan);
                            cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                            cmd.ExecuteNonQuery();
                            BindGrid();
                            MessageBox.Show("Data Hewan berhasil ditambahkan");
                        }
                        catch
                        {
                            MessageBox.Show("Terjadi kesalahan dalam menambahkan data hewan");
                        }
                        ClearAll();
                    }
                    else
                    {
                        try
                        {
                            cmd.CommandText = "UPDATE hewans set id_hewan = @id_hewan, nama_hewan = @nama_hewan, tglLahir_hewan = @tglLahir_hewan, id_customer = @id_customer, id_jenisHewan = @id_jenisHewan, updateLog_By = @updateLog_by WHERE id_hewan = @id_hewan";
                            cmd.Parameters.AddWithValue("@id_hewan", id_hewan);
                            cmd.Parameters.AddWithValue("@nama_hewan", nama_hewan);
                            cmd.Parameters.AddWithValue("@tglLahir_hewan", tglLahir_hewan);
                            cmd.Parameters.AddWithValue("@id_customer", id_customer);
                            cmd.Parameters.AddWithValue("@id_jenisHewan", id_jenisHewan);
                            cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                            cmd.ExecuteNonQuery();
                            BindGrid();
                            MessageBox.Show("Data Hewan berhasil di ubah");
                        }
                        catch
                        {
                            MessageBox.Show("Terjadi kesalahan dalam mengubah data hewan");
                        }
                        ClearAll();
                    }
                }
            else
            {
                MessageBox.Show("Mohon data hewan harap dilengkapi");
            }
        }
        catch
        {
                MessageBox.Show("Mohon data Hewan harap diisi");
        }
        }

        private void ClearAll()
        {
            idhewanTxt.Text = "";
            namahewanTxt.Text = "";
            tanggallahirDp.Text = "";
            idcustomerTxt.Text = "";
            idjenishewanTxt.Text = "";
            tambahBtn.Content = "Tambah";
            idcustomerTxt.IsEnabled = true;
        }

        private void Batal_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if (hewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)hewanDT.SelectedItems[0];
                idhewanTxt.Text = row["ID Hewan"].ToString();
                namahewanTxt.Text = row["Nama Hewan"].ToString();
                tanggallahirDp.Text = row["Tanggal Lahir"].ToString();
                idcustomerTxt.Text = row["ID Customer"].ToString();
                idjenishewanTxt.Text = row["ID Jenis Hewan"].ToString();
                updatelogbyTxt.Text = row["NIP"].ToString();
                idcustomerTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data Hewan yang ingin diubah");
            }
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            if (hewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)hewanDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "DELETE FROM hewans where id_hewan =" + row["ID Hewan"].ToString();
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Hewan berhasil di hapus");
                }
                catch
                {
                    MessageBox.Show("Terjadi kesalahan dalam menghapus data hewan");
                }
                ClearAll();
            }
            else
            {
                MessageBox.Show("Tolong pilih data Hewan yang ingin dihapus");
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
