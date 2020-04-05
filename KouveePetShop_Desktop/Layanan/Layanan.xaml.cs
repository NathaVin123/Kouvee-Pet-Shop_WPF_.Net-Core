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
        public Layanan()
        {
            InitializeComponent();

            conn = new MySqlConnection();
            conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";
            BindGrid();
            BindGridPegawai();
            BindGridUkuranHewan();
        }

        private void BindGrid()
        {
            MySqlCommand cmd = new MySqlCommand();

            if(conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT id_layanan AS 'ID Layanan', nama_layanan AS 'Nama Layanan', harga_layanan AS 'Harga Layanan', id_ukuranHewan AS 'ID Ukuran Hewan', updateLog_by AS 'NIP' FROM layanans";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            layananDT.ItemsSource = dt.AsDataView();
            
            if(dt.Rows.Count > 0)
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

        private void BindGridPegawai()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
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

        private void BindGridUkuranHewan()
        {
            MySqlCommand cmd = new MySqlCommand();

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT id_ukuranHewan AS 'ID Ukuran', nama_ukuranHewan AS 'Nama Ukuran' FROM ukuranhewans";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            uhDT.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                LabelCount.Visibility = System.Windows.Visibility.Hidden;
                uhDT.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                LabelCount.Visibility = System.Windows.Visibility.Visible;
                uhDT.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ClearAll()
        {
            idlayananTxt.Text = "";
            namalayananTxt.Text = "";
            hargalayananTxt.Text = "";
            idukuranhewanTxt.Text = "";
            updatelogbyTxt.Text = "";
            tambahBtn.Content = "Tambah";
            idlayananTxt.IsEnabled = true;
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            if(conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            string id_layanan = idlayananTxt.Text;
            string nama_layanan = namalayananTxt.Text;
            string harga_layanan = hargalayananTxt.Text;
            string id_ukuranHewan = idukuranhewanTxt.Text;
            string updateLog_by = updatelogbyTxt.Text;

            if (idlayananTxt.Text != "")
            {
                if (idlayananTxt.IsEnabled == true)
                {
                    cmd.CommandText = "INSERT INTO layanans(id_layanan,nama_layanan,harga_layanan,id_ukuranHewan,updateLog_by) VALUES (@id_layanan,@nama_layanan,@harga_layanan,@id_ukuranHewan,@updateLog_by)";
                    cmd.Parameters.AddWithValue("@id_layanan", id_layanan);
                    cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
                    cmd.Parameters.AddWithValue("@harga_layanan", harga_layanan);
                    cmd.Parameters.AddWithValue("@id_ukuranHewan", id_ukuranHewan);
                    cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Layanan berhasil ditambahkan");
                    ClearAll();
                }
                else
                {
                    cmd.CommandText = "UPDATE layanans set id_layanan = @id_layanan,nama_layanan = @nama_layanan, harga_layanan = @harga_layanan, id_ukuranHewan = @id_ukuranHewan, updateLog_By = @updateLog_by WHERE id_layanan = @id_layanan";
                    cmd.Parameters.AddWithValue("@id_layanan", id_layanan);
                    cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
                    cmd.Parameters.AddWithValue("@harga_layanan", harga_layanan);
                    cmd.Parameters.AddWithValue("@id_ukuranHewan", id_ukuranHewan);
                    cmd.Parameters.AddWithValue("@updateLog_by", updateLog_by);
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Data Layanan berhasil di ubah");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("ID layanan harap diisi");
            }

        }

        private void Ubah_Click(object sender, RoutedEventArgs e)
        {
            if(layananDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)layananDT.SelectedItems[0];
                idlayananTxt.Text = row["ID Layanan"].ToString();
                namalayananTxt.Text = row["Nama Layanan"].ToString();
                hargalayananTxt.Text = row["Harga Layanan"].ToString();
                idukuranhewanTxt.Text = row["ID Ukuran Hewan"].ToString();
                updatelogbyTxt.Text = row["NIP"].ToString();
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
            if (layananDT.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)layananDT.SelectedItems[0];

                MySqlCommand cmd = new MySqlCommand();
                if(conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM layanans where id_layanan =" + row["ID Layanan"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Data Layanan berhasil di hapus");
                ClearAll();
                
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

            cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
            cmd.CommandText = "SELECT id_layanan AS 'ID Layanan', nama_layanan AS 'Nama Layanan', harga_layanan AS 'Harga Layanan', id_ukuranHewan AS 'ID Ukuran Hewan', updateLog_by AS 'NIP' FROM layanans WHERE nama_layanan = @nama_layanan";
            
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            layananDT.ItemsSource = dt.AsDataView();
        }

        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

    }
}
