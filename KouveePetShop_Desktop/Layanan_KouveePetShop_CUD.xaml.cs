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
            String connectionString = "SERVER=localhost:8081,DATABASE=backend_p3l,UID=root,PASSWORD=;";

            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand cmd = new MySqlCommand();

            connection.Open();

            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
