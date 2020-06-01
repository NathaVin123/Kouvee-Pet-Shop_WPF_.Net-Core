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
    /// Interaction logic for Transaksi_TambahLayanan.xaml
    /// </summary>
    public partial class Transaksi_TambahLayanan : Window
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public string idTransaksi;
        public string idDetail;

        public Transaksi_TambahLayanan()
        {
            InitializeComponent();

            connection = "Server=localhost; User Id=root;Password=;Database=petshopd;Allow Zero Datetime=True";
            conn = new MySqlConnection(connection);
            conn.Open();
            FillComboBoxNamaJasa();
            conn.Close();
        }

        public void FillComboBoxNamaJasa()
        {
            // Ambil ID dan Nama produk dari tabel produk ke combobox
            string Query = "select id_layanan, nama_layanan from petshopd.jasa_layanan;";
            MySqlCommand cmdComboBox = new MySqlCommand(Query, conn);
            MySqlDataReader reader;
            try
            {
                reader = cmdComboBox.ExecuteReader();

                while (reader.Read())
                {
                    int idJasa = reader.GetInt32("id_layanan");
                    string namajasa = reader.GetString("nama_layanan");
                    ComboBoxJasaLayanan.Items.Add(idJasa + " - " + namajasa);
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
            if (string.IsNullOrEmpty(ComboBoxJasaLayanan.Text))
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
                        cmd.CommandText = "INSERT INTO detailtransaksilayanans(kode_penjualan_layanan, id_layanan, jml_transaksi_layanan) VALUES(@idtransaksi, @idjasa, @jumlah)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@idtransaksi", idTransaksi);
                        cmd.Parameters.AddWithValue("@idjasa", ComboBoxJasaLayanan.SelectedValue);
                        cmd.Parameters.AddWithValue("@jumlah", JumlahJasaLayananText.Text);
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
