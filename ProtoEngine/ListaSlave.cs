using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class ListaSlave
    {
        public IList<Slave> lista;
        public ListaSlave() {
            lista = new List<Slave>();
        }
        public void Dodaj(Slave slave)
        {
            lista.Add(slave);
        }
    }
}
