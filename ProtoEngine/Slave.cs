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
        private int wejscia;
        int wyjscia;
        public Slave(int adress) 
        {this.adress=adress;
        InicjalizacjaListyKodowFunkcji();
        wejscia = 1111111;
        wyjscia = 0000000;
        }
        public List<DekodowanieFunkcji> listaKodowFunkcji;
    
        public Ramka Receive(Ramka ramka)
        {
            if (Equals(this.adress, ramka.adres)) //sprawdzanie poprawności adresu
            {
                if (ramka.dane == ramka.suma)  //sprawdzanie poprawności sumy kontrolnej
                {
                    return DekodowanieFunkcji(ramka);
                }
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
        public Ramka DekodowanieFunkcji(Ramka ramka)
        {
            for (int i=0; i<listaKodowFunkcji.Count; i++)
                if (ramka.funkcja == listaKodowFunkcji[0].kod)
                    if(listaKodowFunkcji[0].dane)
                        ramka.dane=wejscia;

            return ramka;
        }
        private void InicjalizacjaListyKodowFunkcji()
        {
            listaKodowFunkcji = new List<DekodowanieFunkcji>();
            DekodowanieFunkcji d1 = new DekodowanieFunkcji();
            d1.znaczenie = "odczyt wyjść bitowych";
            d1.dane = true;
            d1.kod = 1;
            DekodowanieFunkcji d2 = new DekodowanieFunkcji();
            d2.znaczenie = "odczyt wejść bitowych";
            d2.dane = true;
            d2.kod = 2;
            DekodowanieFunkcji d3 = new DekodowanieFunkcji();
            d3.znaczenie = "odczyt n rejestrów wyjściowych";
            d3.dane = true;
            d3.kod = 3;
            DekodowanieFunkcji d4 = new DekodowanieFunkcji();
            d4.znaczenie = "odczyt n rejestrów wejsciowych";
            d4.dane = true;
            d4.kod = 4;
            DekodowanieFunkcji d5 = new DekodowanieFunkcji();
            d5.znaczenie = "zapis 1 bitu";
            d5.dane = false;
            d5.kod = 5;
            DekodowanieFunkcji d6 = new DekodowanieFunkcji();
            d6.znaczenie = "zapis 1 rejestru";
            d6.dane = false;
            d6.kod = 6;
            DekodowanieFunkcji d7 = new DekodowanieFunkcji();
            d7.znaczenie = "odczyt statusu";
            d7.dane = true;
            d7.kod = 7;
            DekodowanieFunkcji d8 = new DekodowanieFunkcji();
            d8.znaczenie = "odczyt test diagnostyczny";
            d8.dane = true;
            d8.kod = 8;
            DekodowanieFunkcji d9 = new DekodowanieFunkcji();
            d9.znaczenie = "zapis n bitów";
            d9.dane = false;
            d9.kod = 9;
            DekodowanieFunkcji d10 = new DekodowanieFunkcji();
            d10.znaczenie = "zapis n rejestrów";
            d10.dane = false;
            d10.kod = 10;
            DekodowanieFunkcji d11 = new DekodowanieFunkcji();
            d11.znaczenie = "odczyt identyfikacja urządzenia slave";
            d11.dane = true;
            d11.kod = 11;

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
    }
}
