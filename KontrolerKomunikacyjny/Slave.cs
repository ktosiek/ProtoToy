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
        public String Receive(String message, int address)
        {
            if (this.adress == address)
                return message;
            return null;
        }
    }
}
