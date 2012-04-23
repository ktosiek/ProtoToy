using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;

namespace ProtoEngine
{
    public class Protocol
    {
        private String name;
        public String Name { get { return name; } }

        List<Message> requestMsgs = new List<Message>();
        List<Message> responseMsgs = new List<Message>();
        Message msg_start_mark = new Message();
        List<Device> devices = new List<Device>();
        public List<Device> RegisteredDevices { get { return new List<Device>(devices); } }
        List<DevicePrototype> devicePrototypes = new List<DevicePrototype>();
        public List<DevicePrototype> DevicePrototypes { get { return new List<DevicePrototype>(devicePrototypes); } }
        List<Option> options = new List<Option>();
        public List<Option> Options { get { return new List<Option>(options); } }

        public Protocol(String path)
        {
            this.constructor(new FileStream(path, FileMode.Open));
        }

        public Protocol(Stream file)
        {
            this.constructor(file);
        }

        private void constructor(Stream file)
        {
            XmlDocument proto = new XmlDocument();
            proto.Load(file);
            foreach (XmlAttribute attr in proto.Attributes) {
                switch (attr.Name)
                {
                    case "name":
                        name = attr.Value;
                        break;
                    default:
                        throw new ArgumentException("Wrong protocol argument " + attr.Name);
                }
            }

            foreach (XmlNode node in proto.ChildNodes)
            {
                switch (node.Name)
                {
                    case "options":
                        foreach (XmlNode optNode in node.ChildNodes)
                        {
                            Option opt = Option.fromXml(node);
                            options.Add(opt);
                        }
                        break;
                    case "msg_start_mark":
                        msg_start_mark = Message.fromXml(node);
                        break;
                    case "msgs":
                        foreach (XmlNode msgNode in node.ChildNodes)
                        {
                            Message msg = Message.fromXml(msgNode);
                            switch (msg.Type)
                            {
                                case MessageType.Request:
                                    requestMsgs.Add(msg);
                                    break;
                                case MessageType.Response:
                                    responseMsgs.Add(msg);
                                    break;
                                case MessageType.Bidirectional:
                                    requestMsgs.Add(msg);
                                    responseMsgs.Add(msg);
                                    break;
                            }
                        }
                        break;
                    case "devices":
                        foreach (XmlNode devNode in node.ChildNodes)
                        {
                            Device dev = Device.fromXml(devNode);
                            devices.Add(dev);
                        }
                        break;
                    default:
                        throw new ArgumentException("Unexpected node: " + node.Name);
                }
            }
        }

        /**
         * Dodaje urządzenie do listy urządzeń na magistrali jeśli go na niej nie ma.
         */
        public void registerDevice(Device device)
        {
            if (!devices.Contains(device))
                devices.Add(device);
        }

        /**
         * Usuwa urządzenie z listy urządzeń na magistrali, jeśli na niej jest.
         */
        public void unregisterDevice(Device device)
        {
            if (devices.Contains(device))
                devices.Remove(device);
        }

    }
}
