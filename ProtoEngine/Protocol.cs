using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;

namespace ProtoEngine
{
    /// <summary>
    /// Klasa odpowiadająca opisowi protokołu.
    /// </summary>
    public class Protocol
    {
        /// <summary>
        /// Callback wywoływany gdy zostanie zdekodowana wiadomość.
        /// </summary>
        /// <param name="msgPrototype">prototyp odebranej wiadomości</param>
        /// <param name="env">stan opcji i zmiennych w momencie jej odebrania,
        /// ważne tylko do powrotu z tej funkcji (jeśli chcesz to zapamiętać - skopiuj)</param>
        public delegate void MessageReceived(Message msgPrototype, List<Option> env);

        /// <summary>
        /// Callback wywoływany zaraz przed wysłaniem wiadomości.
        /// </summary>
        /// <param name="transaction">Transakcja wg. której zbudowano odpowiedź</param>
        /// <param name="msgIn">wiadomość od której zdekodowania się zaczęło</param>
        /// <param name="msgOut">wiadomość która ma zostać wysłana</param>
        /// <param name="env">stan opcji i zmiennych po zbudowaniu odpowiedzi</param>
        public delegate void MessageSent(Transaction transaction, Message msgIn,
            Message msgOut, List<Option> env);

        /// <summary>
        /// Callback wywoływany gdy zostanie zdekodowana wiadomość.
        /// </summary>
        public MessageReceived messageReceived;

        /// <summary>
        /// Callback wywoływany zaraz przed wysłaniem wiadomości.
        /// </summary>
        public MessageSent messageSent;

        private String name;
        /// <summary>
        /// Nazwa protokołu
        /// </summary>
        public String Name { get { return name; } }

        List<Message> requestMsgs = new List<Message>();
        List<Message> responseMsgs = new List<Message>();
        Message msg_start_mark = new Message();
        List<Device> devices = new List<Device>();

        /// <summary>
        /// Lista aktualnie symulowanych urządzeń (urządzeń "na magistrali")
        /// </summary>
        public List<Device> RegisteredDevices { get { return new List<Device>(devices); } }
        List<DevicePrototype> devicePrototypes = new List<DevicePrototype>();
        
        /// <summary>
        /// Lista typów urządzeń które mogą być symulowane dla danego protokołu
        /// </summary>
        public List<DevicePrototype> DevicePrototypes { get { return new List<DevicePrototype>(devicePrototypes); } }
        List<Option> options = new List<Option>();
        
        /// <summary>
        /// Lista opcji dla danego protokołu
        /// </summary>
        public List<Option> Options { get { return new List<Option>(options); } }

        /// <summary>
        /// Konstruktor przyjmujący ścieżkę do pliku opisu protokołu
        /// </summary>
        /// <param name="path">ścieżka do pliku opisu protokołu</param>
        public Protocol(String path)
        {
            this.constructor(new FileStream(path, FileMode.Open));
        }

        /// <summary>
        /// Konstruktor przyjmujący strumień z którego będzie czytany XML
        /// </summary>
        /// <param name="file">strumień zawierający opis protokołu</param>
        public Protocol(Stream file)
        {
            this.constructor(file);
        }

        /// <summary>
        /// Faktyczny konstruktor
        /// </summary>
        /// <param name="file">strumień zawierający opis protokołu</param>
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

        /// <summary>
        /// Dodaje urządzenie do listy urządzeń na magistrali jeśli go na niej nie ma.
        /// </summary>
        /// <param name="device">urządzenie do dodania</param>
        public void registerDevice(Device device)
        {
            if (!devices.Contains(device))
                devices.Add(device);
        }

        /// <summary>
        /// Usuwa urządzenie z listy urządzeń na magistrali, jeśli na niej jest.
        /// </summary>
        /// <param name="device">urządzenie do usunięcia</param>
        public void unregisterDevice(Device device)
        {
            if (devices.Contains(device))
                devices.Remove(device);
        }

        /// <summary>
        /// Zaczyna odczytywanie danych, kontynuuje do zakończenia strumienia lub wywołania stop()
        /// Ta metoda będzie wywoływać messageReceived i messageSent.
        /// </summary>
        /// <param name="inStream">strumień z którego czytane będą dane</param>
        /// <param name="outStream">strumień do którego wysyłane będą odpowiedzi</param>
        public void run(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }
    }
}
