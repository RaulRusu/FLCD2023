using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers
{
    public class LinkedList
    {
        public LinkedListNode? First { get; set; } = null;
        public LinkedListNode? Last { get; set; } = null;
        public int Size { get; private set; } = 0;

        public LinkedListNode AddLast(string value)
        {
            var node = new LinkedListNode(value);

            if (First == null)
            {
                this.First = node;
                this.Last = node;
            }
            else
            {
                this.Last.Next = node;
                this.Last = node;
            }

            Size++;
            return node;
        }

        public int? GetIndex(string value)
        {
            var currentNode = this.First;
            var index = 0;

            while (currentNode != null)
            {
                if (currentNode.Value == value)
                    return index;

                index++;
                currentNode = currentNode.Next;
            }

            return null;
        }

        public override string ToString()
        {
            var str = "";
            var currentNode = this.First;

            while (currentNode != null)
            {
                str += currentNode.Value.ToString() + " ";

                currentNode = currentNode.Next;
            }

            return str;
        }
    }

    public class LinkedListNode
    {
        public LinkedListNode() { }
        public LinkedListNode(string value)
        {
            Value = value;
        }

        public LinkedListNode? Next { get; set; } = null;
        public string? Value { get; set; } = null;
    }
}
