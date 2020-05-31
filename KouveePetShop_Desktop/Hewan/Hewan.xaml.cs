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
        DateTime tanggal = DateTime.Now;
        public Hewan()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshopd;UID=root;PASSWORD=;Allow Zero Datetime=True";
                BindGrid();
                BindGridLog();
                FillComboBoxCustomer();
                FillComboBoxJenisHewan();
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
                cmd.CommandText = "SELECT id_hewan AS 'ID Hewan', nama_hewan AS 'Nama Hewan', tglLahir_hewan AS 'Tanggal Lahir', id_customer AS 'ID Customer', id_jenisHewan AS 'ID Jenis Hewan', updateLog_by AS 'Diubah Oleh' FROM hewans WHERE aktif LIKE '1'";
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

        private void BindGridLog()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT nama_hewan AS 'Nama Hewan', createLog_at AS 'Di Buat', updateLog_at AS 'Di Ubah', deleteLog_at AS 'Di Hapus' FROM hewans";
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

        public void FillComboBoxCustomer()
        {
            string query = "SELECT id_customer, nama_customer FROM petshopd.customers;";

            MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader mySqlDataReader;

            try
            {
                mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    string idCustomer = mySqlDataReader.GetString("id_customer");
                    string namaCustomer = mySqlDataReader.GetString("nama_customer");
                    idcustomerCb.Items.Add(idCustomer + " - " + namaCustomer);
                }
                mySqlDataReader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        public void FillComboBoxJenisHewan()
        {
            string query = "SELECT id_jenisHewan, nama_jenisHewan FROM petshopd.jenishewans;";

            MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader mySqlDataReader;

            try
            {
                mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    string idJenisHewan = mySqlDataReader.GetString("id_jenisHewan");
                    string namaJenisHewan = mySqlDataReader.GetString("nama_jenisHewan");
                    idjenishewanCb.Items.Add(idJenisHewan + " - " + namaJenisHewan);
                }
                mySqlDataReader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
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
                string nama_hewan = namahewanTxt.Text;
                string tglLahir_hewan = tanggallahirDp.SelectedDate.Value.ToString("yyyy-MM-dd");
                string id_customer = idcustomerCb.Text;
                string id_jenisHewan = idjenishewanCb.Text;
                string updateLog_by = updatelogbyCb.Text;
                string id_hewan = idhewanTxt.Text;
                string updateLog_at = tanggal.ToString("yyyy-MM-dd H:mm:ss");

                if (namahewanTxt.Text != "" && tanggallahirDp.SelectedDate != null && updatelogbyCb.Text != "" || string.IsNullOrEmpty(idcustomerCb.Text) || string.IsNullOrEmpty(idjenishewanCb.Text))
                {
                    if (idhewanTxt.IsEnabled == true)
                    {
                        try
                        {
                            cmd.CommandText = "INSERT INTO hewans(nama_hewan,tglLahir_hewan,id_customer,id_jenisHewan,updateLog_by) VALUES (@nama_hewan,@tglLahir_hewan,@id_customer,@id_jenisHewan,@updateLog_by)";
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
                            cmd.CommandText = "UPDATE hewans set id_hewan = @id_hewan, nama_hewan = @nama_hewan, tglLahir_hewan = @tglLahir_hewan, id_customer = @id_customer, id_jenisHewan = @id_jenisHewan, updateLog_By = @updateLog_by, updateLog_at = @updateLog_at WHERE id_hewan = @id_hewan";
                            cmd.Parameters.AddWithValue("@id_hewan", id_hewan);
                            cmd.Parameters.AddWithValue("@nama_hewan", nama_hewan);
                            cmd.Parameters.AddWithValue("@tglLahir_hewan", tglLahir_hewan);
                            cmd.Parameters.AddWithValue("@id_customer", id_customer);
                            cmd.Parameters.AddWithValue("@id_jenisHewan", id_jenisHewan);
                            cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                            cmd.Parameters.AddWithValue("@updateLog_at", updateLog_at);
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
            idcustomerCb.Text = "";
            idjenishewanCb.Text = "";
            updatelogbyCb.Text = "";
            tambahBtn.Content = "Tambah";
            idhewanTxt.IsEnabled = true;
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
                idcustomerCb.SelectedValue = row["ID Customer"].ToString();
                idjenishewanCb.SelectedValue = row["ID Jenis Hewan"].ToString();
                updatelogbyCb.SelectedValue = row["Diubah Oleh"].ToString();
                idhewanTxt.IsEnabled = false;
                tambahBtn.Content = "Update";
            }
            else
            {
                MessageBox.Show("Tolong pilih data Hewan yang ingin diubah");
            }
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            int aktif = '0';
            string message = "Apakah anda ingin menghapus data ini ?";
            string caption = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            if (hewanDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)hewanDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd.CommandText = "UPDATE hewans set aktif = @aktif where id_hewan =" + row["ID Hewan"].ToString();
                        cmd.Parameters.AddWithValue("@aktif", aktif);
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        BindGrid();
                        MessageBox.Show("Data Hewan berhasil di hapus");
                    }
                    catch
                    {
                        MessageBox.Show("Terjadi kesalahan dalam menghapus data hewan");
                    }
                    ClearAll();
                }
                
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

            string nama_Hewan = cariTxt.Text;
            try
            {
                cmd.Parameters.AddWithValue("@nama_hewan", nama_Hewan);
                cmd.CommandText = "SELECT id_hewan AS 'ID Hewan', nama_hewan AS 'Nama Hewan', tglLahir_hewan AS 'Tanggal Lahir', id_customer AS 'ID Customer', id_jenisHewan AS 'ID Jenis Hewan', updateLog_by AS 'Diubah Oleh' FROM hewans WHERE nama_hewan = @nama_hewan";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                hewanDT.ItemsSource = dt.AsDataView();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan dalam mencari data hewan");
            }
        }
    }
}
