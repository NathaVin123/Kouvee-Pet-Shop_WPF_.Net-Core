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

namespace KouveePetShop_Desktop
{
    /// <summary>
    /// Interaction logic for Login_KouveePetShop.xaml
    /// </summary>
    public partial class Login_KouveePetShop : Window
    {
        MySqlConnection conn;
        DataTable dt;
        public Login_KouveePetShop()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = "SERVER=localhost;DATABASE=petshop;UID=root;PASSWORD=;";
            }
            catch
            {
                MessageBox.Show("Tidak ada database...");
            }

        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT COUNT(1) FROM pegawais WHERE NIP = @NIP AND password = @password";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NIP", txtNIP.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    var Menu = new Menu.Menu_KouveePetShop();
                    Menu.Show();
                    this.Close();
                }
                else if(txtNIP.Text=="" && txtPassword.Password=="")
                {
                    MessageBox.Show("NIP dan Password harus diisi");
                }
                else
                {
                    MessageBox.Show("NIP dan Password tidak sesuai");
                }
            }
            catch
            {
                MessageBox.Show("Database bermasalah...");
            }
            finally
            {
                conn.Close();
            }
            
        }

        private void KlikDisini_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}