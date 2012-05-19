using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class Ramka
    {
        public int adres;
        public int funkcja;
        public int dane;
        public int suma;
        public Ramka() { }
        public void SumaCRC(int dane)
        {
            this.suma = dane;

        }
        public String Wyswietl()
        {
            return adres.ToString() + " " + funkcja.ToString() + " " + dane.ToString();
        }

    }
}
