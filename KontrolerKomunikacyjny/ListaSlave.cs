using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KontrolerKomunikacyjny
{
    class ListaSlave
    {
        public IList<ProtoEngine.Slave> lista;
        public ListaSlave() {
            lista = new List<ProtoEngine.Slave>();
        }
        public void Dodaj(ProtoEngine.Slave slave)
        {
            lista.Add(slave);
        }
    }
}
