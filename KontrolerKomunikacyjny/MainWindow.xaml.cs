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
        public int a;
        public List<ProtoEngine.Slave> listaSlave;
        public MainWindow()
        {

            InitializeComponent();
            RozmieszczeniePolRamek();

            BladAdresSlaveLabel.Visibility = Visibility.Hidden;
            BladIloscWejscLabel.Visibility = Visibility.Hidden;
            BladIloscWyjscLabel.Visibility = Visibility.Hidden;
            
            listaSlave = new List<ProtoEngine.Slave>();
           



        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //ProtoEngine.Protocol p1=new ProtoEngine.Protocol( "E:/Projekty/ProtoToy/Modbus-serial.xml" );


        }

        private void WyslijPrzycisk_Click(object sender, RoutedEventArgs e)
        {
            ProtoEngine.Ramka ramka_master = new ProtoEngine.Ramka();
            ramka_master.adres = AdresSlaveTextbox.Text;
            ramka_master.funkcja = FunkcjaTextbox.Text;
            ramka_master.dane = DaneTextbox.Text;
            ramka_master.SumaCRC(ramka_master.dane);
            String tmp = "Master: ";
            tmp += ramka_master.Wyswietl();

            

            Ramka ramka_slave = listaSlave[0].Receive(ramka_master);
         

            tmp += "\nSlave: ";
            tmp += ramka_slave.Wyswietl();
            

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

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new Dodaj();
            Application.Current.MainWindow.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {


            int adres, iloscWejsc, iloscWyjsc;
            if (!int.TryParse(DodajAdresTextbox.Text, out adres))
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
                Slave slave = new Slave(adres);
                listaSlave.Add(slave);

                UrzadzeniaWSystemieLabel.Content = "";
                for (int i = 0; i < listaSlave.Count; i++)
                {
                    UrzadzeniaWSystemieLabel.Content += "Slave o adresie: " + listaSlave[i].adress + "\n";
                }
            }
        }
    }
}