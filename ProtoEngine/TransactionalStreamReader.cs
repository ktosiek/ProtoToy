using System;
using System.Collections.Generic;
using System.IO;

namespace ProtoEngine
{
    class TransactionalStreamReader
    {
        private Stream baseStream;
        private Stack<Stack<char>> ongoingTransactions;
        private Stack<char> readData;

        public TransactionalStreamReader(Stream stream)
        {
            baseStream = stream;
            ongoingTransactions = new Stack<Stack<char>>();
        }

        public void startTransaction()
        {
            ongoingTransactions.Push(new Stack<char>());
        }

        public void commitTransaction()
        {
            ongoingTransactions.Pop();
        }

        public void cancelTransaction()
        {
            foreach (char c in ongoingTransactions.Pop())
                readData.Push(c);
        }

        public int ReadByte()
        {
            int b;
            if (readData.Count != 0)
                b = readData.Pop();
            else
                b = baseStream.ReadByte();
            ongoingTransactions.Peek().Push((char)b);
            return b;
        }
    }
}
