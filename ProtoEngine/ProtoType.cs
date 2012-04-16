using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    abstract class ProtoType
    {
        static public ProtoType fromXml( XmlNode node ) {
            throw new NotImplementedException();
        }

        private String name;
        public String Name { get { return this.name; } }
    }
}
