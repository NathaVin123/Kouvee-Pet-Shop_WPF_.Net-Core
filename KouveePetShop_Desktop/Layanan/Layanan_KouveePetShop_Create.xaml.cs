using System;
using System.Collections.Generic;
using System.Data;
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
using MySql.Data.MySqlClient;

namespace KouveePetShop_Desktop
{
    public partial class Layanan_KouveePetShop : Window
    {
        private string connection;
        MySqlConnection conn;


        public Layanan_KouveePetShop()
        {
            InitializeComponent();
            try {
                string connectionString = "SERVER=localhost;DATABASE=p3l_db;UID=root;PASSWORD=;";

                MySqlConnection connection = new MySqlConnection(connectionString);

                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand cmd = new MySqlCommand("SELECT `nama_layanan`, `harga_layanan`, `jenis_layanan` FROM `layanans`", connection);

                connection.Open();

                DataTable dt = new DataTable();

                DataSet ds = new DataSet();

                dt.Load(cmd.ExecuteReader());
                connection.Close();

                dtLayanan.DataContext = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void dbConnection()
        {
            try
            {
                connection = "SERVER=localhost;DATABASE=p3l_db;UID=root;PASSWORD=;"; //Database Web Server Atma
                conn = new MySqlConnection(connection);
                conn.Open();
            }
            catch
            {
                throw;
            }
        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            //InitializeComponent();

            //MySqlConnection connection = new MySqlConnection("Datasource=localhost;username=root;password=;");
            //connection.Open();
            //string selectQuery = "SELECT * FROM p3l.jenis_layanan";

            //MySqlCommand command = new MySqlCommand(selectQuery, connection);

            //MySqlDataReader reader = command.ExecuteReader();

            //while(reader.Read())
            //{
               /*ComboBox.Items.Add(reader.GetString("fname"));*/
            //}

        }

        public bool TambahLayanan(string id_pegawai_fk,string id_ukuranHewan_fk, string nama_layanan, string harga_layanan, string jenis_layanan)
        {
            //string connectionString = "SERVER=localhost;DATABASE=p3l_db;UID=root;PASSWORD=;";
            //MySqlConnection dbConnection = new MySqlConnection(connectionString);
            dbConnection();
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = "INSERT INTO layanans(id_pegawai_fk, id_ukuranHewan_fk, nama_layanan, harga_layanan, jenis_layanan) VALUES (@id_pegawai_fk,@id_ukuranHewan_fk,@nama_layanan,@harga_layanan,@jenis_layanan)";

            cmd.Parameters.AddWithValue("@id_pegawai_fk", id_pegawai_fk);
            cmd.Parameters.AddWithValue("@id_ukuranHewan_fk", id_ukuranHewan_fk);
            cmd.Parameters.AddWithValue("@nama_layanan", nama_layanan);
            cmd.Parameters.AddWithValue("@harga_layanan", harga_layanan);
            cmd.Parameters.AddWithValue("@jenis_layanan", jenis_layanan);

            cmd.Connection = conn;
            MySqlDataReader add = cmd.ExecuteReader();

            return add.Read() ? true : false;
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            string id_pegawai_fk = IDPegawaiTxt.Text;
            string id_ukuranHewan_fk = IDUkuranHewanTxt.Text;
            string nama_layanan = NamaLayananTxt.Text;
            string harga_layanan = HargaLayananTxt.Text;
            string jenis_layanan = ((ComboBoxItem)JenisLayananCB.SelectedItem).Content.ToString();

            if (nama_layanan == "" || harga_layanan == "" || jenis_layanan== "" || jenis_layanan == "-- Select --")
                MessageBox.Show("Please fill all the field", "Warning");
            else
            {
                bool a = TambahLayanan(id_pegawai_fk, id_ukuranHewan_fk, nama_layanan, harga_layanan, jenis_layanan);
                if (a)
                {
                    MessageBox.Show("Successful", "Successful input");
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Failed", "Failed input");
                    conn.Close();
                }
            }
        }

        private void UbahData_Click(object sender, RoutedEventArgs e)
        {
            var UpdateLayanan = new Layanan_KouveePetShop_Update();
            UpdateLayanan.Show();
            this.Close();
        }

        private void HapusData_Click(object sender, RoutedEventArgs e)
        {
            var UpdateLayanan = new Layanan_KouveePetShop_Update();
            UpdateLayanan.Show();
            this.Close();
        }
    }
}
