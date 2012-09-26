using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoEngine;

namespace KontrolerKomunikacyjny
{
    class PolaRamki
    {
        public List<String> listaNazw;
        public List<String> listaTypow;
        public List<int> listaWartosciMax;
        public List<int> listaWartosciMin;
        public PolaRamki()
        {
            InicjalizacjaListyNazw();
            InicjalizacjaListyTypow();
            InicjalizacjaListyWartosciMax();
            InicjalizacjaListyWartosciMin();
           
        }
        private void InicjalizacjaListyNazw()
        {
            listaNazw = new List<string>();
            listaNazw.Add("Adres slave");
            listaNazw.Add("Kod funkcji");
            listaNazw.Add("Dane");
            listaNazw.Add("Adres we/wy");
            listaNazw.Add("Wartość we/wy");
        }
        private void InicjalizacjaListyTypow()
        {
            listaTypow = new List<string>();
            listaTypow.Add("byte");
            listaTypow.Add("byte");
            listaTypow.Add("String");

        }
        private void InicjalizacjaListyWartosciMax() {
            listaWartosciMax = new List<int>();
            listaWartosciMax.Add(247);
            listaWartosciMax.Add(127);
            listaWartosciMax.Add(200);

        }
        private void InicjalizacjaListyWartosciMin() {
            listaWartosciMin = new List<int>();
            listaWartosciMin.Add(1);
            listaWartosciMin.Add(1);
            listaWartosciMin.Add(1);
        }
    }
}
