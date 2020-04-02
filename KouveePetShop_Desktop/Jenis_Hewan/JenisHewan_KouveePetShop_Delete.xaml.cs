using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace KouveePetShop_Desktop.Jenis_Hewan
{
    /// <summary>
    /// Interaction logic for JenisHewan_KouveePetShop_Delete.xaml
    /// </summary>
    public partial class JenisHewan_KouveePetShop_Delete : Window
    {
        private string connection;
        MySqlConnection conn;

        public JenisHewan_KouveePetShop_Delete()
        {
            InitializeComponent();

            try
            {
                string connectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";

                MySqlConnection connection = new MySqlConnection(connectionString);

                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand cmd = new MySqlCommand("SELECT `id_jenisHewan` AS 'ID Jenis Hewan', `nama_jenisHewan`AS 'Nama Jenis Hewan' FROM `jenishewans`", connection);

                connection.Open();

                DataTable dt = new DataTable();

                DataSet ds = new DataSet();

                dt.Load(cmd.ExecuteReader());
                connection.Close();

                dtJenisHewan.DataContext = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void dbConnection()
        {
            try
            {
                connection = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;"; //Database Web Server Atma
                conn = new MySqlConnection(connection);
                conn.Open();
            }
            catch
            {
                throw;
            }
        }
    }
}
