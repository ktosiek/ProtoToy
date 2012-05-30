using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class Ramka
    {
        public byte adres;
        public byte funkcja;
        public List<byte> dane;
        public int suma;

        public Ramka(int ileBajtowDanych) 
        {
            dane = new List<byte>();
            if (ileBajtowDanych <= 252)
                for (int i = 0; i < ileBajtowDanych; i++)
                    dane.Add(0);

         }
        public void SumaCRC(int dane)
        {
            this.suma = dane;
        }
        public String Wyswietl(bool a)
        {
            if(a)
                return adres.ToString() + " " + funkcja.ToString();
            else

                return adres.ToString() + " " + funkcja.ToString() + " " + dane[0].ToString();
        }

    }
}
