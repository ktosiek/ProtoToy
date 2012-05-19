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

        /// <summary>
        /// Tworzy nową instancję urządzenia.
        /// </summary>
        /// <returns></returns>
        public Device create()
        {
            return new Device(device);
        }

        public static DevicePrototype fromXml(XmlNode node, Protocol protocol)
        {
            return new DevicePrototype(node, protocol);
        }

        public DevicePrototype(XmlNode node, Protocol protocol)
        {
            device = Device.fromXml(node, protocol);
        }
    }
}
