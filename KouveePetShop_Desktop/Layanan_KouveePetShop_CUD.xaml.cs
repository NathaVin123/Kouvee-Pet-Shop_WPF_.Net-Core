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
            string connectionString = "SERVER=localhost;DATABASE=9127;UID=root;PASSWORD=;";

            MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM layanans", connection);

            connection.Open();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();

            dtLayanan.DataContext = dt;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
