using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class Slave
    {
        public int adress { get; set; }
        public int iloscWejsc { get; set; }
        public int iloscWyjsc { get; set; }
        public Slave(int adress) {this.adress=adress;}
        public Ramka Receive(Ramka ramka)
        {
            if (Equals(this.adress, ramka.adres)) //sprawdzanie poprawności adresu
            {
                if (ramka.dane == ramka.suma)          //sprawdzanie poprawności sumy kontrolnej
                    return ramka;
                else
                {
                    ramka.dane = -1;                //w przypadku blędu 
                    return ramka;
                }

            }
            
            else
                ramka.adres = -1;                //w przypadku blędu 
                return ramka;
            
        }
    }
}
