using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KontrolerKomunikacyjny
{
    class Slave
    {
        int adress;
        public Slave(int adress) {this.adress=adress;}
        public Ramka Receive(Ramka ramka)
        {
            if (Equals(this.adress, ramka.adres))
                return ramka;
            else return null;
        }
    }
}
