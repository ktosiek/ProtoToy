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
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           //ProtoEngine.Protocol p1=new ProtoEngine.Protocol( "E:/Projekty/ProtoToy/Modbus-serial.xml" );
           

        }

        private void WyslijPrzycisk_Click(object sender, RoutedEventArgs e)
        {
            String tmp = "Master: ";
            tmp += WyslijTextbox.Text+"\n";
             Slave slave1 = new Slave(1);
             tmp += "Slave: ";
            tmp+=slave1.Receive(WyslijTextbox.Text,1);
            ChatEtykieta.Content = tmp;
           
        }
    }
}
