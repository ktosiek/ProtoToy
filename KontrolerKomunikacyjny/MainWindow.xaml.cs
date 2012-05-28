﻿using System;
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
        public List<ProtoEngine.Slave> listaSlave;
        public MainWindow()
        {
            InitializeComponent();
            RozmieszczeniePolRamek();

            BladAdresSlaveLabel.Visibility = Visibility.Hidden;
            BladIloscWejscLabel.Visibility = Visibility.Hidden;
            BladIloscWyjscLabel.Visibility = Visibility.Hidden;
            BladMasterAdresLabel.Visibility = Visibility.Hidden;
            BladMasterFunkcjaLabel.Visibility = Visibility.Hidden;
            BladMasterDaneLabel.Visibility = Visibility.Hidden;
            
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
            ProtoEngine.Ramka ramka_master = new ProtoEngine.Ramka(1);


            byte adres, funkcja, dane;
            if (!byte.TryParse(AdresSlaveTextbox.Text, out adres))
                BladMasterAdresLabel.Visibility = Visibility.Visible;
            else
                BladMasterAdresLabel.Visibility = Visibility.Hidden;
            if (!byte.TryParse(FunkcjaTextbox.Text, out funkcja))
                BladMasterFunkcjaLabel.Visibility = Visibility.Visible;
            else
                BladMasterFunkcjaLabel.Visibility = Visibility.Hidden;
            if (!byte.TryParse(DaneTextbox.Text, out dane))
                BladMasterDaneLabel.Visibility = Visibility.Visible;
            else
                BladMasterDaneLabel.Visibility = Visibility.Hidden;
            
            ramka_master.adres = adres;
            ramka_master.funkcja = funkcja;
            ramka_master.dane[0] = dane;

            ramka_master.SumaCRC(ramka_master.dane[0]);

            String tmp = "Master: ";
            tmp += ramka_master.Wyswietl();
             Ramka ramka_slave;
         for (int i=0;i<listaSlave.Count;i++)
            {
                ramka_slave= listaSlave[i].Receive(ramka_master);
                if (ramka_slave.adres != 0)
                {
                    tmp += "\nSlave "+listaSlave[i].adress.ToString()+": ";
                    tmp += ramka_slave.Wyswietl();
                    ChatEtykieta.Content +="\n"+ tmp;
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