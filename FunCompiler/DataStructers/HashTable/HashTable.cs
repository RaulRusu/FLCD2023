using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers
{
    public class HashTable
    {
        const int DEFAULT_BUCKET_SIZE = 1000;
        private LinkedList[] buckets;
        public int Size { private set; get; }

        public HashTable()
        {
            this.Size = DEFAULT_BUCKET_SIZE;
            buckets = new LinkedList[DEFAULT_BUCKET_SIZE];
        }

        public HashTable(int size)
        {
            this.Size = size;
            buckets = new LinkedList[size];
        }

        /// <summary>
        /// Computes the hash of a string using djb2 Algorithm
        /// </summary>
        /// <param name="value"></param>
        private int hashFunction(string value)
        {
            var hash = 5381;

            byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
            asciiBytes.ToList().ForEach(asciiByte => hash = (hash * 33 + asciiByte) % Size);

            return hash;
        }

        /// <summary>
        /// Add an element in the hash table
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The position of the elemnt</returns>
        public HashTablePosition Add(string value)
        {
            var hash = hashFunction(value);

            var bucket = buckets[hash];
            if (buckets[hash] == null)
            {
                buckets[hash] = new LinkedList();
                bucket = buckets[hash];
            }
            bucket.AddLast(value);

            return new HashTablePosition(hash, bucket.Size - 1);
        }

        /// <summary>
        /// Find an element in the hash table by it's value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The position in the hash table or null if it doesn't exists</returns>
        public HashTablePosition? Find(string value)
        {
            var hash = hashFunction(value);

            var bucket = buckets[hash];
            if (bucket == null)
                return null;

            var index = bucket.GetIndex(value);
            if (!index.HasValue)
                return null;

            return new HashTablePosition(hash, index.Value);
        }

        public override string ToString()
        {
            var index = 0;
            var str = "";

            buckets.ToList().ForEach(bucket =>
            {
                if (bucket != null)
                {
                    str += index.ToString() + ": " + bucket.ToString() + "\n";
                }
                index++;
            });

            return str;
        }
    }

    public class HashTablePosition
    {
        public int Bucket { get; set; }
        public int Index { get; set; }

        public HashTablePosition(int bucket, int index)
        {
            Bucket = bucket;
            Index = index;
        }
        public override string ToString()
        {
            return $"Bucket: {Bucket.ToString()} Index: {Index.ToString()}";
        }
    }
}
