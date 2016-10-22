using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak1
{
    class Program1
    {
        static void Main(string[] args)
        {
            IntegerList lista = new IntegerList();
            ListExample(lista);
            Console.ReadLine();

        }

        public static void ListExample(IIntegerList listOfIntegers)
        {
            listOfIntegers.Add(1); // [1]
            listOfIntegers.Add(2); // [1 ,2]
            listOfIntegers.Add(3); // [1 ,2 ,3]
            listOfIntegers.Add(4); // [1 ,2 ,3 ,4]
            listOfIntegers.Add(5); // [1 ,2 ,3 ,4 ,5]
            listOfIntegers.RemoveAt(0); // [2 ,3 ,4 ,5]
            listOfIntegers.Remove(5); //[2 ,3 ,4]
            Console.WriteLine(listOfIntegers.Count); // 3
            Console.WriteLine(listOfIntegers.Remove(100)); // false
            Console.WriteLine(listOfIntegers.RemoveAt(5)); // false
            listOfIntegers.Clear(); // []
            Console.WriteLine(listOfIntegers.Count); // 0
        }
    }

    public interface IIntegerList
    {
        /// <summary >
        /// Adds an item to the collection .
        /// </ summary >
        void Add(int item);

        /// <summary >
        /// Removes the first occurrence of an item from the collection .
        /// If the item was not found , method does nothing .
        /// </ summary >
        bool Remove(int item);
        /// <summary >
        /// Removes the item at the given index in the collection .
        /// </ summary >
        bool RemoveAt(int index);
        /// <summary >
        /// Returns the item at the given index in the collection .
        /// </ summary >
        int GetElement(int index);
        /// <summary >
        /// Returns the index of the item in the collection .
        /// If item is not found in the collection , method returns -1.
        /// </ summary >
        int IndexOf(int item);
        int Count { get; }
        /// <summary >
        /// Removes all items from the collection .
        /// </ summary >
        void Clear();
        /// <summary >
        /// Determines whether the collection contains a specific value .
        /// </ summary >
        bool Contains(int item);
    }

    public class IntegerList : IIntegerList
    {
        private int[] _internalStorage;
        int size;
        int csize = 0;

        public IntegerList()
        {
            _internalStorage = new int[4];
            size = 4;
        }

        public IntegerList(int initialSize)
        {
            _internalStorage = new int[initialSize];
            size = initialSize;
        }

        public int Count
        {
            get
            {
                return csize;
            }
        }

        public void Add(int item)
        {
            if (csize >= size)
            {
                int[] temp = _internalStorage;
                _internalStorage = new int[2 * size];
                temp.CopyTo(_internalStorage, size);
                size = 2 * size;
            }
            _internalStorage[csize] = item;
            csize++;


        }

        public void Clear()
        {
            _internalStorage = new int[size];
            csize = 0;
        }

        public bool Contains(int item)
        {
            for (int i = 0; i < size; i++)
            {
                if (_internalStorage[i] == item)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetElement(int index)
        {
            if (index < size && index > -1)
            {
                return _internalStorage[index];
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public int IndexOf(int item)
        {
            for (int i = 0; i < size; i++)
            {
                if (_internalStorage[i] == item) return i;
            }
            return -1;
        }

        public bool Remove(int item)
        {
            for (int i = 0; i < size; i++)
            {
                if (_internalStorage[i] == item)
                {
                    return this.RemoveAt(i);
                }
            }
            return false;
        }

        public bool RemoveAt(int index)
        {
            if (index >= csize)
            {
                return false;
            }

            for (int i = index; i < size - 1; i++)
            {
                _internalStorage[i] = _internalStorage[i + 1];
            }
            csize--;
            return true;
        }
    }
}