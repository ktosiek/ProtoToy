using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KontrolerKomunikacyjny
{
    class PolaRamki
    {
        public List<String> listaNazw;
        public List<String> listaTypow;
        public PolaRamki()
        {
            listaNazw=new List<string>();
            listaNazw.Add("Adres slave");
            listaNazw.Add("Funkcja");
            listaNazw.Add("Dane");
            listaTypow=new List<string>();
            listaTypow.Add("byte");
            listaTypow.Add("byte");
            listaTypow.Add("String");
        }
    }
}
