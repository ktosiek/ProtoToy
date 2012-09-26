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
using ProtoEngine;

namespace KontrolerKomunikacyjny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Slave> listaSlave;
        public MainWindow()
        {
            InitializeComponent();
            RozmieszczeniePolRamek();

            BladAdresSlaveLabel.Visibility = Visibility.Hidden;
            BladIloscWejscLabel.Visibility = Visibility.Hidden;
            BladIloscWyjscLabel.Visibility = Visibility.Hidden;
            BladMasterAdresLabel.Visibility = Visibility.Hidden;
            BladMasterFunkcjaLabel.Visibility = Visibility.Hidden;
            BladMasterAdresWeWyLabel.Visibility = Visibility.Hidden;
            BladMasterWartoscLabel.Visibility = Visibility.Hidden;

            listaSlave = new List<ProtoEngine.Slave>();
            DodajAdresTextbox.Text = "1";
            IloscWejscTextbox.Text = "1";
            IloscWyjscTextbox.Text = "1";
            byte adres;
            byte.TryParse(DodajAdresTextbox.Text, out adres);
 
            Slave slave = new Slave(adres,1,1);
            listaSlave.Add(slave);
            UrzadzeniaWSystemieLabel.Content += listaSlave[0].Wyswietl();

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //ProtoEngine.Protocol p1=new ProtoEngine.Protocol( "E:/Projekty/ProtoToy/Modbus-serial.xml" );


        }
        private void WyslijPrzycisk_Click(object sender, RoutedEventArgs e)
        {
            Ramka ramka_master = new Ramka(2);


            byte adres, funkcja, adresWeWy,wartosc;
            int blad = 0;
            if (!byte.TryParse(AdresSlaveTextbox.Text, out adres)) //sprawdza, czy adres jest typu byte
            {
                BladMasterAdresLabel.Visibility = Visibility.Visible;
                blad = 1;
            }
            else
                BladMasterAdresLabel.Visibility = Visibility.Hidden;
            if (!byte.TryParse(FunkcjaTextbox.Text, out funkcja)) //sprawdza, czy kod funkcji jest typu byte
            {
                BladMasterFunkcjaLabel.Visibility = Visibility.Visible;
                blad = 1;
            }
            else
                BladMasterFunkcjaLabel.Visibility = Visibility.Hidden;
            if (funkcja == 1 || funkcja == 2)
            {
                if (!byte.TryParse(AdresWeWyTextbox.Text, out adresWeWy)) //TODO sprawdza czy adres we/wy jest typu 2*byte
                {
                    BladMasterAdresWeWyLabel.Visibility = Visibility.Visible;
                    blad = 1;
                }
                ramka_master.dane[1] = adresWeWy; //TODO zamienić dane[1]=adresWeWy na dane[0]=adresWeWy
            }
            if (funkcja == 5 || funkcja == 6) //jeśli kod funkcji oznacza zapis
            {
                if (!byte.TryParse(AdresWeWyTextbox.Text, out adresWeWy)) //TODO sprawdza czy adres we/wy jest typu 2*byte
                {
                    BladMasterAdresWeWyLabel.Visibility = Visibility.Visible;
                    blad = 1;
                }
                else
                    BladMasterAdresWeWyLabel.Visibility = Visibility.Hidden;
                if (!byte.TryParse(WartoscTextbox.Text, out wartosc)) //sprawdza, czy wartość we/wy jest typu byte
                {
                    BladMasterWartoscLabel.Visibility = Visibility.Visible;
                    blad = 1;
                }
                else
                    BladMasterWartoscLabel.Visibility = Visibility.Hidden;
                ramka_master.dane[1] = adresWeWy;
                ramka_master.dane[0] = wartosc;
            }
            ramka_master.adres = adres;
            ramka_master.funkcja = funkcja;
           

            ramka_master.SumaCRC(ramka_master.dane[0]);

            if (blad == 0)
            {
                String tmp = "Master [adres_slave kod_funkcji]: ";
                tmp += ramka_master.Wyswietl(true);
                Ramka ramka_slave=new Ramka(2); //TODO dlaczego nie =new Ramka(2)?
                for (int i = 0; i < listaSlave.Count; i++) //do każdego slave z listy
                {
                    ramka_slave = listaSlave[i].Receive(ramka_master); //wysyła ramkę od mastera
                    //slave zwraca ramkę odpowiedzi
                    if (ramka_slave.adres != 0)                         
                    {
                        tmp += "\nSlave [adres_slave kod_funkcji, ] " + listaSlave[i].adress.ToString() + ": ";
                        tmp += ramka_slave.Wyswietl(false);
                        ChatEtykieta.Content += "\n" + tmp;
                    }
                }
            }
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
            for (double tmp = marginesGorny; i < 5; i++, tmp += przes)
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
                 
                }
                else if(i==3)
                {
                    AdresWyWeLabel.Margin=thickness;
                    AdresWyWeLabel.Content = polaRamki.listaNazw[i];
                    AdresWeWyTextbox.Margin = thicknessTextbox;
                }
                else if (i == 4)
                {
                    WartoscLabel.Margin = thickness;
                    WartoscLabel.Content = polaRamki.listaNazw[i];
                    WartoscTextbox.Margin = thicknessTextbox;
                }
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {


            byte adres;
            int iloscWejsc, iloscWyjsc;
            if (!byte.TryParse(DodajAdresTextbox.Text, out adres))
                BladAdresSlaveLabel.Visibility = Visibility.Visible;
            else
                BladAdresSlaveLabel.Visibility = Visibility.Hidden;
            if (!int.TryParse(IloscWejscTextbox.Text, out iloscWejsc))
                BladIloscWejscLabel.Visibility = Visibility.Visible;
            else
                BladIloscWejscLabel.Visibility = Visibility.Hidden;
            if (!int.TryParse(IloscWyjscTextbox.Text, out iloscWyjsc))
                BladIloscWyjscLabel.Visibility = Visibility.Visible;
            else
                BladIloscWyjscLabel.Visibility = Visibility.Hidden;
            if (BladAdresSlaveLabel.Visibility == Visibility.Hidden
                && BladIloscWejscLabel.Visibility == Visibility.Hidden
                && BladIloscWyjscLabel.Visibility == Visibility.Hidden)
            {
                Slave slave = new Slave(adres,iloscWejsc,iloscWyjsc);
                bool jest=false;
                for (int i = 0; i < listaSlave.Count(); i++)
                    if (listaSlave[i].adress == slave.adress)
                        jest = true;
                if (!jest)
                    listaSlave.Add(slave);
               else
              MessageBox.Show("Slave o wybranym adresie już istnieje");
              
                UrzadzeniaWSystemieLabel.Content = "";
                for (int i = 0; i < listaSlave.Count; i++)
             
                    UrzadzeniaWSystemieLabel.Content +=  listaSlave[i].Wyswietl();
            }
        }
    }
}