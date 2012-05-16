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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KontrolerKomunikacyjny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AdresSlaveLabel.Content = "Adres slave";
            AdresSlaveLabel.Margin = new Thickness(280, 20, 0, 0);
           PolaRamki polaRamki=new PolaRamki();
          
            int marginesGorny = 47;
            int marginesLewy = 280;
            int marginesLewyTextbox = 360;
            double przes = AdresSlaveLabel.Height + 20;
            Thickness thickness;
            int i=0;
            for (double tmp = marginesGorny; i < 3; i++, tmp += przes)
            {
                thickness = new Thickness(marginesLewy, tmp, 0, 0);
                Thickness thicknessTextbox = new Thickness(marginesLewyTextbox, tmp, 0, 0);
                if (i == 0){
                    AdresSlaveLabel.Margin = thickness;
                    AdresSlaveLabel.Content=polaRamki.listaNazw[i];
                    AdresSlaveTextbox.Margin = thicknessTextbox;
                  }
                else if (i == 1)
                {
                    FunkcjaLabel.Margin = thickness;
                    FunkcjaLabel.Content = polaRamki.listaNazw[i];
                    FunkcjaTextbox.Margin = thicknessTextbox;
                }
                else if (i == 2)
                {
                    DaneLabel.Margin = thickness;
                    DaneLabel.Content = polaRamki.listaNazw[i];
                    DaneTextbox.Margin = thicknessTextbox;
                }
                
            }
            


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           //ProtoEngine.Protocol p1=new ProtoEngine.Protocol( "E:/Projekty/ProtoToy/Modbus-serial.xml" );
           

        }

        private void WyslijPrzycisk_Click(object sender, RoutedEventArgs e)
        {
            String tmp = "Master: ";
            String ramka= AdresSlaveTextbox.Text+"  "+FunkcjaTextbox.Text+"  "+DaneTextbox.Text+"\n";
            tmp += ramka;
             Slave slave1 = new Slave(1);
             tmp += "Slave: ";
            tmp+=slave1.Receive(ramka,1);
            ChatEtykieta.Content = tmp;
           
        }
    }
}
