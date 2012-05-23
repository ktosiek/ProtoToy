using System;
using System.Collections.Generic;
using System.IO;

namespace ProtoEngine
{
    /// <summary>
    /// Opakowuje Stream do odczytywania transakcyjnego.
    /// Operacje odczytujące z niego muszą być wykonywane w transakcji.
    /// Transakcje mogą się zawierać w sobie, ale nie mogą się przeplatać (commit i cancel działają zawsze na ostatnio otwartej transakcji).
    /// Commit przenosi listę wczytanych znaków aktualnej transakcji do transakcji poprzedniej, lub ją porzuca jeśli jest to jedyna otwarta transakcja.
    /// Cancel przenosi listę wczytanych znaków aktualnej transakcji na początek bufora wejściowego, do ponownego wczytania.
    /// </summary>
    public class TransactionalStreamReader
    {
        private Stream baseStream;
        private Stack<Stack<char>> ongoingTransactions = new Stack<Stack<char>>();
        private Stack<char> readData = new Stack<char>();

        public TransactionalStreamReader(Stream stream)
        {
            baseStream = stream;
        }

        public void startTransaction()
        {
            ongoingTransactions.Push(new Stack<char>());
        }

        public void commitTransaction()
        {
            Stack<Char> thisTrans = ongoingTransactions.Pop();
            try
            {
                foreach (char c in thisTrans)
                    ongoingTransactions.Peek().Push(c);
            }
            catch (InvalidOperationException ex) { /* Peek rzuci je jeśli stos jest pusty */ }
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

        public bool isReadable()
        {
            if (readData.Count > 0)
                return true;
            int c;
            if ((c = baseStream.ReadByte()) != -1)
            {
                readData.Push((char)c);
                return true;
            }
            return false;
        }

        public List<char> getTransactionData()
        {
            return new List<char>(ongoingTransactions.Peek());
        }
    }
}
