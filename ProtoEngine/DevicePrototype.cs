using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    public class DevicePrototype
    {
        private Device device;
        public String Name { get { return device.Name; } }

        public static DevicePrototype fromXml(XmlNode node)
        {
            return new DevicePrototype(node);
        }

        public DevicePrototype(XmlNode node)
        {
            device = Device.fromXml(node);
        }
    }
}
