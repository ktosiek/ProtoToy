using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;

namespace ProtoEngine
{
    public delegate void OptionChangedEventHandler(Object sender, Option oldOpt, Option newOpt);

    public class Protocol
    {
        List<Message> requestMsgs = new List<Message>();
        List<Message> responseMsgs = new List<Message>();
        Message msg_start_mark = new Message();
        List<Device> devices = new List<Device>();
        List<Device> devicePrototypes = new List<Device>();
        Dictionary<String, Option> options = new Dictionary<String, Option>();
        Dictionary<String, ProtoType> types = new Dictionary<String, ProtoType>();

        public event OptionChangedEventHandler OptionChanged;

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
                        throw new ArgumentException("Wrong argument " + attr.Name);
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
                            options.Add(opt.Name, opt);
                        }
                        break;
                    case "types":
                        foreach (XmlNode typeNode in node.ChildNodes)
                        {
                            ProtoType type = ProtoType.fromXml(typeNode);
                            types.Add(type.Name, type);
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

        private String name;
        public String Name { get { return name; } }

        public Dictionary<String, Option> getOptions()
        {
            return new Dictionary<String, Option>(options);
        }

        /**
         * Ustawia opcję w słowniku opcji protokołu.
         * Jeśli synthetic == false wysyła sygnał OptionChanged
         */
        public void setOption(Option opt, Boolean synthetic = true)
        {
            options[opt.Name].setFrom(opt);
        }

        public DeviceFactory getDeviceFactory()
        {
            throw new NotImplementedException();
        }

        /**
         * Dodaje urządzenie do listy urządzeń na magistrali.
         */
        public void registerDevice(Device device)
        {
            throw new NotImplementedException();
        }

        /**
         * Usuwa urządzenie z listy urządzeń na magistrali, jeśli na niej jest.
         */
        public void removeDevice(Device device)
        {
            throw new NotImplementedException();
        }

        /**
         * Zwraca kopię listy zarejestrowanych urządzeń.
         */
        public List<Device> getRegisteredDevices()
        {
            throw new NotImplementedException();
        }
    }
}
