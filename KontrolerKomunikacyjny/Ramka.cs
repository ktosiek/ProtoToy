using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KontrolerKomunikacyjny
{
    class Ramka
    {
        public String adres;
        public String funkcja;
        public String dane;
        public int suma;
        public Ramka() { }
        public void SumaCRC(String dane)
        {
            this.suma= 23;

        }
        public String Wyswietl()
        {
            return adres + " " + funkcja + " " + dane;
        }   
        
    }
}
