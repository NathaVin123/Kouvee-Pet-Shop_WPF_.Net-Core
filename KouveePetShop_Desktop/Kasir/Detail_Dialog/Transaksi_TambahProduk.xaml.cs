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
using MySql.Data.MySqlClient;
using System.Data;

namespace KouveePetShop_Desktop.Kasir.Detail_Dialog
{

    /// <summary>
    /// Interaction logic for Transaksi_TambahProduk.xaml
    /// </summary>
    public partial class Transaksi_TambahProduk : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public string idTransaksi;
        public string idDetail;
        public Transaksi_TambahProduk()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            connection = "Server=localhost; User Id=root;Password=;Database=petshopd;Allow Zero Datetime=True";
            conn = new MySqlConnection(connection);
            conn.Open();
            FillComboBoxNamaProduk();
            conn.Close();
        }

        public void FillComboBoxNamaProduk()
        {
            // Ambil ID dan Nama produk dari tabel produk ke combobox
            string Query = "select id_produk, nama_produk from petshopd.produks;";
            MySqlCommand cmdComboBox = new MySqlCommand(Query, conn);
            MySqlDataReader reader;
            try
            {
                reader = cmdComboBox.ExecuteReader();

                while (reader.Read())
                {
                    int idProduk = reader.GetInt32("id_produk");
                    string namaProduk = reader.GetString("nama_produk");
                    ComboBoxNamaProduk.Items.Add(idProduk + " - " + namaProduk);
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnTambah_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxNamaProduk.Text))
            {
                MessageBox.Show("Silahkan pilih data terlebih dahulu", "Warning");
                return;
            }
            else
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = "INSERT INTO detailtransaksiproduks dp JOIN produkhargas ph ON dp.id_produkHarga = ph.id_produkHarga JOIN produks p ON ph.id_produk = p.id_produk(kode_penjualan_produk, id_produk, jml_transaksi_produk) VALUES(@idtransaksi, @idproduk, @jumlah)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@idtransaksi", idTransaksi);
                        cmd.Parameters.AddWithValue("@idproduk", ComboBoxNamaProduk.SelectedValue);
                        cmd.Parameters.AddWithValue("@jumlah", JumlahProdukText.Text);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Berhasil ditambahkan", "Success");
                        this.Close();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                        conn.Close();
                    }
                }

            }
        }

        private void BtnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
