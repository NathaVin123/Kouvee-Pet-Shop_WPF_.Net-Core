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
    /// <summary>
    /// Interaction logic for Layanan_KouveePetShop.xaml
    /// </summary>
    public partial class Layanan_KouveePetShop : Window
    {
        public Layanan_KouveePetShop()
        {
            InitializeComponent();
            try {
                string connectionString = "SERVER=localhost;DATABASE=9160;UID=root;PASSWORD=;";

                MySqlConnection connection = new MySqlConnection(connectionString);

                MySqlCommand cmd = new MySqlCommand("SELECT `nama_layanan`, `harga_layanan`, `jenis_layanan` FROM `layanans`", connection);

                connection.Open();

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                connection.Close();

                dtLayanan.DataContext = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            InitializeComponent();

            MySqlConnection connection = new MySqlConnection("Datasource=localhost;username=root;password=;");
            connection.Open();
            string selectQuery = "SELECT * FROM 9160.jenis_layanan";

            MySqlCommand command = new MySqlCommand(selectQuery, connection);

            MySqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
               /*ComboBox.Items.Add(reader.GetString("fname"));*/
            }
        }
    }
}
