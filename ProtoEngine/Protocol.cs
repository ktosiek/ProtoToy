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
        public delegate void MessageReceived(FullMessage msg, List<Option> env);

        /// <summary>
        /// Callback wywoływany zaraz przed wysłaniem wiadomości.
        /// </summary>
        /// <param name="transaction">Transakcja wg. której zbudowano odpowiedź</param>
        /// <param name="msgIn">wiadomość od której zdekodowania się zaczęło</param>
        /// <param name="msgOut">wiadomość która ma zostać wysłana</param>
        /// <param name="env">stan opcji i zmiennych po zbudowaniu odpowiedzi</param>
        public delegate void MessageSent(Transaction transaction, FullMessage msgIn,
            FullMessage msgOut, List<Option> env);

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
            FileStream f = new FileStream(path, FileMode.Open, FileAccess.Read);
            this.constructor(f);
            f.Close();
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
            XmlNode protoNode = proto.ChildNodes[1];
            foreach (XmlAttribute attr in protoNode.Attributes) {
                switch (attr.Name)
                {
                    case "name":
                        name = attr.Value;
                        break;
                    default:
                        throw new ArgumentException("Wrong protocol argument " + attr.Name);
                }
            }

            foreach (XmlNode node in protoNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "options":
                        foreach (XmlNode optNode in node.ChildNodes)
                        {
                            Option opt = Option.fromXml(optNode);
                            options.Add(opt);
                        }
                        break;
                    case "msg_start_mark":
                        msg_start_mark = Message.fromXml(node, this);
                        break;
                    case "msgs":
                        foreach (XmlNode msgNode in node.ChildNodes)
                        {
                            Message msg = Message.fromXml(msgNode, this);
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
                            DevicePrototype dev = DevicePrototype.fromXml(devNode, this);
                            devicePrototypes.Add(dev);
                        }
                        break;
                    default:
                        if(node.Name[0] != '#')
                            throw new ArgumentException("Unexpected node: " + node.Name);
                        break;
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
        /// Zaczyna odczytywanie danych, kontynuuje do zakończenia strumienia lub wywołania stop() (TODO: faktycznie wprowadź stop())
        /// Ta metoda będzie wywoływać messageReceived i messageSent.
        /// </summary>
        /// <param name="inStream">strumień z którego czytane będą dane</param>
        /// <param name="outStream">strumień do którego wysyłane będą odpowiedzi</param>
        public void run(Stream inStream, Stream outStream)
        {
            TransactionalStreamReader transStream = new TransactionalStreamReader(inStream);
            while (transStream.isReadable())
            {
                // Zmienne środowiskowe dla tej transakcji
                Dictionary<String,Option> env = new Dictionary<string,Option>();
                foreach(Option opt in options)
                    env.Add(opt.Name, opt);

                List<Option> fields;
                transStream.startTransaction();
                env = this.msg_start_mark.match(env, transStream, out fields);
                if (env == null)
                    throw new NotImplementedException("Should wait for next msg_start_mark");
                Dictionary<String, Option> nextEnv = null;
                nextEnv = matchIncoming(env, transStream, new List<Message>(), out fields);

                if (nextEnv == null)
                    throw new NotImplementedException("Should wait for next msg_start_mark (no requestMsg matched)");
                
                env = nextEnv;
                FullMessage msgIn = new FullMessage(fields);

                if(messageReceived != null)
                    messageReceived(msgIn, new List<Option>(env.Values));


                Transaction transaction = null;
                foreach (Device d in this.RegisteredDevices)
                {
                    Dictionary<String, Option> deviceEnv = new Dictionary<string, Option>(env);
                    foreach (Option opt in d.Options)
                    {
                        deviceEnv.Add(opt.Name, opt);
                    }

                    foreach (Transaction t in d.Transactions)
                    {
                        Dictionary<String, Option> transEnv = new Dictionary<string,Option>(deviceEnv);
                        transEnv = t.match(transEnv);
                        if (transEnv != null)
                        {
                            transaction = t;
                            env = transEnv;
                            break;
                        }
                    }
                    if (transaction != null)
                        break;
                }

                if (transaction == null)
                    throw new NotImplementedException("No transaction matched, wait for next message");

                List<byte[]> output;
                nextEnv = matchOutgoing(env, new List<Message>(), out output, out fields);
                if(nextEnv == null)
                    throw new NotImplementedException("No outgoing message matched, wait for next message");

                FullMessage msgOut = new FullMessage(fields);
                if (messageSent != null)
                    messageSent(transaction, msgIn, msgOut, new List<Option>(env.Values));

                foreach (byte[] data in output)
                    outStream.Write(data, 0, data.Length);
            }
        }


        public List<Message> excludedSoFar = new List<Message>(); // UGLY UGLY UGLY, potrzebne w RuleIncMsg
        public Message currentMessage = null;
        public Dictionary<string, Option> matchIncoming(Dictionary<string, Option> env,
            TransactionalStreamReader transStream,
            List<Message> excluded,
            out List<Option> fields)
        {
            List<Message> oldExcluded = excludedSoFar;
            excludedSoFar = excluded;

            Dictionary<String, Option> nextEnv = null;
            fields = new List<Option>();
            foreach (Message msg in requestMsgs)
            {
                if (!excluded.Contains(msg))
                {
                    List<Option> newFields;
                    Message oldCurrentMsg = currentMessage;
                    currentMessage = msg;

                    transStream.startTransaction();
                    nextEnv = msg.match(env, transStream, out newFields);

                    currentMessage = oldCurrentMsg;

                    if (nextEnv == null)
                        transStream.cancelTransaction();
                    else
                    {
                        if (newFields != null)
                            fields.AddRange(newFields);
                        transStream.commitTransaction();
                        break;
                    }
                }
            }

            excludedSoFar = oldExcluded;

            return nextEnv;
        }

        public Dictionary<string, Option> matchOutgoing(Dictionary<string, Option> env,
            List<Message> excluded,
            out List<byte[]> output,
            out List<Option> fields)
        {
            output = new List<byte[]>();
            List<Message> oldExcluded = excludedSoFar;
            excludedSoFar = excluded;

            Dictionary<String, Option> nextEnv = null;
            fields = new List<Option>();
            foreach (Message msg in responseMsgs)
            {
                if (!excluded.Contains(msg))
                {
                    List<Option> newFields;
                    List<byte[]> newOutput;
                    Message oldCurrentMsg = currentMessage;
                    currentMessage = msg;

                    nextEnv = msg.match(env, out newOutput, out newFields);

                    currentMessage = oldCurrentMsg;

                    if (nextEnv != null)
                    {
                        if (newFields != null)
                            fields.AddRange(newFields);
                        if (newOutput != null)
                            output.AddRange(newOutput);
                        break;
                    }
                }
            }

            excludedSoFar = oldExcluded;

            return nextEnv;
        }
    }
}
