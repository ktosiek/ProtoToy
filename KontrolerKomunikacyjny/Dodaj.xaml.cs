using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProtoEngine;

namespace KontrolerKomunikacyjny
{
    /// <summary>
    /// Interaction logic for Dodaj.xaml
    /// </summary>
    public partial class Dodaj : Window
    {
        public Dodaj()
        {
            InitializeComponent();
            BladAdresSlaveLabel.Visibility = Visibility.Hidden;
            BladIloscWejscLabel.Visibility = Visibility.Hidden;
            BladIloscWyjscLabel.Visibility = Visibility.Hidden;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            int adres,iloscWejsc,iloscWyjsc;
            if (!int.TryParse(AdresSlaveTextbox.Text, out adres))
                BladAdresSlaveLabel.Visibility = Visibility.Visible;
            else
                BladAdresSlaveLabel.Visibility = Visibility.Hidden;
            if (!int.TryParse(IloscWejscTextbox.Text, out iloscWejsc))
                BladIloscWejscLabel.Visibility = Visibility.Visible;
            else
                BladIloscWejscLabel.Visibility = Visibility.Hidden;
            if(!int.TryParse(IloscWyjscTextbox.Text,out iloscWyjsc))
                BladIloscWyjscLabel.Visibility=Visibility.Visible;
            else
                BladIloscWyjscLabel.Visibility=Visibility.Hidden;
            if(BladAdresSlaveLabel.Visibility==Visibility.Hidden 
                && BladIloscWejscLabel.Visibility==Visibility.Hidden 
                && BladIloscWyjscLabel.Visibility==Visibility.Hidden)
            {
            Slave slave = new Slave(adres);
            
            }

        }
    }
}
