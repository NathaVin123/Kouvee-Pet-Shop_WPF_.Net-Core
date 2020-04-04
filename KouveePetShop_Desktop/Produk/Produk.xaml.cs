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
using Microsoft.Win32;

namespace KouveePetShop_Desktop.Produk
{
    /// <summary>
    /// Interaction logic for Produk.xaml
    /// </summary>
    public partial class Produk : Window
    {
        public Produk()
        {
            InitializeComponent();
        }

        private void MenuUtama_Click(object sender, RoutedEventArgs e)
        {
            var Menu = new Menu.Menu_KouveePetShop();
            Menu.Show();
            this.Close();
        }

        private void GambarBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Multiselect = false;
            opf.Filter = "Choose Image(*.JPG;*.PNG;*.GIF)|*.jpg;*.png;*.gif";
            Nullable<bool> dialogOK = opf.ShowDialog();

            if(dialogOK == true)
            {
                UploadImage = Image.
            }
        }
    }
}
