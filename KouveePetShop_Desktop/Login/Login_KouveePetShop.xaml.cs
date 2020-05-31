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
        Menu.Menu_KouveePetShop ADMIN = new Menu.Menu_KouveePetShop();
        Menu.Menu_Kasir KASIR = new Menu.Menu_Kasir();
        
        //MySqlConnection conn;
        public Login_KouveePetShop()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //try
            //{
            //    conn = new MySqlConnection();
            //    conn.ConnectionString = "SERVER=localhost;DATABASE=petshopd;UID=root;PASSWORD=;";
            //}
            //catch
            //{
            //    MessageBox.Show("Tidak ada database...");
            //}

        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    MySqlCommand cmd = new MySqlCommand();
            //    if (conn.State != ConnectionState.Open)
            //        conn.Open();
            //    cmd.Connection = conn;
            //    cmd.CommandText = "SELECT COUNT(1) FROM pegawais WHERE NIP = @NIP AND password = @password";
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Parameters.AddWithValue("@NIP", txtNIP.Text);
            //    cmd.Parameters.AddWithValue("@password", txtPassword.Password);
            //    int count = Convert.ToInt32(cmd.ExecuteScalar());
            //    if (count == 1)
            //    {
            //        var Menu = new Menu.Menu_KouveePetShop();
            //        Menu.Show();
            //        this.Close();

            //    }
            //    else if(txtNIP.Text=="" && txtPassword.Password=="")
            //    {
            //        MessageBox.Show("NIP dan Password harus diisi");
            //    }
            //    else
            //    {
            //        MessageBox.Show("NIP dan Password tidak sesuai");
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("Database bermasalah...");
            //}
            //finally
            //{
            //    conn.Close();
            //}
            try
            {
                if (txtNIP.Text == "" || txtPassword.SecurePassword.Length == 0)
                {
                    MessageBox.Show("Field NIP dan Password tidak boleh kosong", "Warning");
                    return;
                }
                else
                {
                    string NIP = txtNIP.Text;
                    string password = txtPassword.Password;

                    MySqlConnection conn = new MySqlConnection("Server=localhost; User Id=root;Password=;Database=petshopd");
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * from pegawais where NIP = '" + NIP + "' and password = '" + password + "'", conn);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string user = "Anda login sebagai " + ds.Tables[0].Rows[0]["nama_pegawai"].ToString() + " - " + ds.Tables[0].Rows[0]["stat"].ToString();
                        ADMIN.RoleText.Text = user;
                        KASIR.RoleText.Text = user;

                        string idPegawai = ds.Tables[0].Rows[0]["NIP"].ToString();

                        // User akan diarahkan berdasar pada role
                        string role = ds.Tables[0].Rows[0]["stat"].ToString();
                        switch (role)
                        {
                            case "admin":
                                // dashboardAdmin.GetValueRole(user);
                                ADMIN.SendValueRole(user);
                                ADMIN.Show();
                                this.Close();
                                break;
                            case "Kasir":
                                KASIR.Show();
                                this.Close();
                                break;
                        }
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("Masukkan NIP dan Password yang valid!", "Warning");
                        conn.Close();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return;
            }

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}