using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje3
{
    public class olusanhash<TKey, TValue>
    {
        private const int DefaultCapacity = 16;
        private List<KeyValuePair<TKey, TValue>>[] doluluk;

        public olusanhash()
        {
            doluluk = new List<KeyValuePair<TKey, TValue>>[DefaultCapacity];
        }

        private int GetBucketIndex(TKey key)
        {
            int hashCode = key.GetHashCode();
            int index = Math.Abs(hashCode % doluluk.Length);
            return index;
        }

        public void Add(TKey key, TValue value)
        {
            int index = GetBucketIndex(key);

            if (doluluk[index] == null)
            {
                doluluk[index] = new List<KeyValuePair<TKey, TValue>>();
            }

            var existingPairIndex = doluluk[index].FindIndex(pair => pair.Key.Equals(key));

            if (existingPairIndex == -1)
            {
                doluluk[index].Add(new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                doluluk[index][existingPairIndex] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public void PrintKeyValuePairs()
        {
            foreach (var dolu in doluluk)
            {
                if (dolu != null)
                {
                    foreach (var pair in dolu)
                    {
                        Console.WriteLine($"Key: {pair.Key}");

                        if (pair.Value is Dictionary<string, int> nestedDictionary)
                        {
                            foreach (var nestedPair in nestedDictionary)
                            {
                                Console.WriteLine($"  Nested Key: {nestedPair.Key}, Nested Value: {nestedPair.Value}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"  Value: {pair.Value}");
                        }
                    }
                }
            }
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetBucketIndex(key);

            if (doluluk[index] != null)
            {
                foreach (var pair in doluluk[index])
                {
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;
                        return true;
                    }
                }
            }

            value = default(TValue);
            return false;
        }
    }
}