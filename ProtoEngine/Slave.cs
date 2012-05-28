using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class Slave
    {
        public byte adress { get; set; }
        List<bool> wejscia;
        List<bool> wyjscia;
        List<byte> rejestryWejsciowe;
        List<byte> rejestryWyjsciowe;

        public int iloscWejsc { get; set; }
        public int iloscWyjsc { get; set; }
        public List<DekodowanieFunkcji> listaKodowFunkcji;

        public Slave(byte adress, int iloscWejsc, int iloscWyjsc)
        {
            this.adress = adress;
            this.iloscWejsc = iloscWejsc;
            this.iloscWyjsc = iloscWyjsc;

            InicjalizacjaObszaruDanych();
            InicjalizacjaListyKodowFunkcji();

        }

        public Ramka Receive(Ramka ramka)
        {
            if (this.adress == ramka.adres) //sprawdzanie poprawności adresu
            {
                if (ramka.dane[0].ToString() == ramka.suma.ToString())  //sprawdzanie poprawności sumy kontrolnej
                {
                    return DekodowanieFunkcji(ramka);
                }
                else
                {
                    ramka.dane[0] = 0;                //w przypadku blędu sumy kontrolnej
                    return ramka;
                }

            }

            else
            {
                ramka.adres = 0;                //w przypadku blędu adresu 
                return ramka;
            }
        }
        public Ramka DekodowanieFunkcji(Ramka ramka)
        {
            for (int i = 0; i < listaKodowFunkcji.Count; i++)
                if (ramka.funkcja == listaKodowFunkcji[i].kod)
                {

                    if (ramka.funkcja == 2)
                    {
                        ramka.dane[0] = 0; // wejscia[0];
                        return ramka;
                    }
                    if (ramka.funkcja == 1)
                    {
                        ramka.dane[0] = 1; //wyjscia[0];
                        return ramka;
                    }
                    if (ramka.funkcja == 3)
                    {
                        ramka.dane[0] = rejestryWyjsciowe[0];
                        return ramka;
                    }
                    if (ramka.funkcja == 4)
                    {
                        ramka.dane[0] = rejestryWejsciowe[0];
                        return ramka;
                    }
                }

            ramka.funkcja += 8;  //błąd nie znaleziono takiej funkcji dodaje 98
            ramka.dane[0] = 1;
            return ramka;
        }

        void InicjalizacjaObszaruDanych()
        {
            wejscia = new List<bool>();
            wyjscia = new List<bool>();
            rejestryWyjsciowe = new List<byte>();
            rejestryWejsciowe = new List<byte>();
            if (iloscWejsc <= 65536)
                for (int i = 0; i < iloscWejsc; i++)
                {
                    wejscia.Add(false);
                }
            if (iloscWyjsc <= 65536)
                for (int i = 0; i < iloscWyjsc; i++)
                {
                    wyjscia.Add(true);
                }
            if (iloscWejsc <= 125)
                for (int i = 0; i < iloscWejsc; i++)
                {
                    rejestryWejsciowe.Add(0);
                }
            else
                for (int i = 0; i < 126; i++)
                {
                    rejestryWejsciowe.Add(0);
                }
            if (iloscWyjsc <= 125)
                for (int i = 0; i < iloscWyjsc; i++)
                {
                    rejestryWyjsciowe.Add(255);
                }
            else
                for (int i = 0; i < 126; i++)
                {
                    rejestryWyjsciowe.Add(255);
                }

        }
        private void InicjalizacjaListyKodowFunkcji()
        {
            listaKodowFunkcji = new List<DekodowanieFunkcji>();
            DekodowanieFunkcji d1 = new DekodowanieFunkcji();
            d1.znaczenie = "odczyt wyjść bitowych";
            d1.dane = true;
            d1.kod = 1;
            d1.wejscia = false;
            d1.cyfrowe = true;
            DekodowanieFunkcji d2 = new DekodowanieFunkcji();
            d2.znaczenie = "odczyt wejść bitowych";
            d2.dane = true;
            d2.kod = 2;
            d2.wejscia = true;
            d2.cyfrowe = true;
            DekodowanieFunkcji d3 = new DekodowanieFunkcji();
            d3.znaczenie = "odczyt n rejestrów wyjściowych";
            d3.dane = true;
            d3.kod = 3;
            d3.wejscia = false;
            d3.cyfrowe = false;
            DekodowanieFunkcji d4 = new DekodowanieFunkcji();
            d4.znaczenie = "odczyt n rejestrów wejsciowych";
            d4.dane = true;
            d4.kod = 4;
            d4.wejscia = true;
            d4.cyfrowe = false;
            DekodowanieFunkcji d5 = new DekodowanieFunkcji();
            d5.znaczenie = "zapis 1 bitu";
            d5.dane = false;
            d5.kod = 5;
            d5.cyfrowe = true;
            DekodowanieFunkcji d6 = new DekodowanieFunkcji();
            d6.znaczenie = "zapis 1 rejestru";
            d6.dane = false;
            d6.kod = 6;
            d6.cyfrowe = false;
            DekodowanieFunkcji d7 = new DekodowanieFunkcji();
            d7.znaczenie = "odczyt statusu";
            d7.dane = true;
            d7.kod = 7;
            d7.cyfrowe = false;
            DekodowanieFunkcji d8 = new DekodowanieFunkcji();
            d8.znaczenie = "odczyt test diagnostyczny";
            d8.dane = true;
            d8.kod = 8;
            d8.cyfrowe = false;
            DekodowanieFunkcji d9 = new DekodowanieFunkcji();
            d9.znaczenie = "zapis n bitów";
            d9.dane = false;
            d9.kod = 9;
            d9.cyfrowe = false;
            DekodowanieFunkcji d10 = new DekodowanieFunkcji();
            d10.znaczenie = "zapis n rejestrów";
            d10.dane = false;
            d10.kod = 10;
            d10.cyfrowe = false;
            DekodowanieFunkcji d11 = new DekodowanieFunkcji();
            d11.znaczenie = "odczyt identyfikacja urządzenia slave";
            d11.dane = true;
            d11.kod = 11;
            d11.cyfrowe = false;
            listaKodowFunkcji.Add(d1);
            listaKodowFunkcji.Add(d2);
            listaKodowFunkcji.Add(d3);
            listaKodowFunkcji.Add(d4);
            listaKodowFunkcji.Add(d5);
            listaKodowFunkcji.Add(d6);
            listaKodowFunkcji.Add(d7);
            listaKodowFunkcji.Add(d8);
            listaKodowFunkcji.Add(d9);
            listaKodowFunkcji.Add(d10);
            listaKodowFunkcji.Add(d11);
        }
        public String Wyswietl()
        {
            return "Adres: " + this.adress.ToString() + "; Ilość wejść: " + this.iloscWejsc.ToString() + "; Ilość wyjść" + this.iloscWyjsc.ToString() + "\n";
        }
    }
}
