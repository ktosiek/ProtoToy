using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class Slave
    {
        public int adress { get; set; }
        public Slave(int adress) {this.adress=adress;}
        public Ramka Receive(Ramka ramka)
        {
            if (Equals(this.adress, ramka.adres))
                return ramka;

            else
            {
                ramka.adres = "blad";
                return ramka;
            }
        }
    }
}
