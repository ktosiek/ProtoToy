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
           RozmieszczeniePolRamek();
            


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           //ProtoEngine.Protocol p1=new ProtoEngine.Protocol( "E:/Projekty/ProtoToy/Modbus-serial.xml" );
           

        }

        private void WyslijPrzycisk_Click(object sender, RoutedEventArgs e)
        {
            Ramka ramka_master=new Ramka();
            ramka_master.adres= AdresSlaveTextbox.Text;
            ramka_master.funkcja=FunkcjaTextbox.Text;
            ramka_master.dane=DaneTextbox.Text;
            ramka_master.SumaCRC(ramka_master.dane);
            String tmp = "Master: ";
            tmp += ramka_master.Wyswietl();

            Slave s1= new Slave(1);
            Slave s2=new Slave(2);
            Slave s3=new Slave(3);
            List<Slave> listaSlave= new List<Slave>();
            
            listaSlave.Add(s1);
            listaSlave.Add(s2);
            listaSlave.Add(s3);

             Ramka ramka_slave=listaSlave[0].Receive(ramka_master);
             Ramka ramka_slave1=listaSlave[1].Receive(ramka_master);
            
             Slave slave1 = new Slave(1);
             tmp += "Slave: ";
             tmp+=ramka_slave1.Wyswietl();
             
            
            ChatEtykieta.Content = tmp;

           
        }
         private void RozmieszczeniePolRamek()
        {
          PolaRamki polaRamki = new PolaRamki();

            int marginesGorny = 47;
            int marginesLewy = 280;
            int marginesLewyTextbox = 360;
            double przes = AdresSlaveLabel.Height + 20;
            Thickness thickness;
            int i = 0;
            for (double tmp = marginesGorny; i < 3; i++, tmp += przes)
            {
                thickness = new Thickness(marginesLewy, tmp, 0, 0);
                Thickness thicknessTextbox = new Thickness(marginesLewyTextbox, tmp, 0, 0);
                if (i == 0)
                {
                    AdresSlaveLabel.Margin = thickness;
                    AdresSlaveLabel.Content = polaRamki.listaNazw[i];
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
