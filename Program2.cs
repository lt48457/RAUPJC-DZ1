using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak2
{
    class Program2
    {
        static void Main(string[] args)
        {
        }
    }

    public interface IGenericList<X>
    {
        /// <summary >
        /// Adds an item to the collection .
        /// </ summary >
        void Add(X item);
        /// <summary >
        /// Removes the first occurrence of an item from the collection .
        /// If the item was not found , method does nothing .
        /// </ summary >
        bool Remove(X item);
        /// <summary >
        /// Removes the item at the given index in the collection .
        /// </ summary >
        bool RemoveAt(int index);
        /// <summary >
        /// Returns the item at the given index in the collection .
        /// </ summary >
        X GetElement(int index);
        /// <summary >
        /// Returns the index of the item in the collection .
        /// If item is not found in the collection , method returns -1.
        /// </ summary >
        int IndexOf(X item);
        /// <summary >
        /// Readonly property . Gets the number of items contained in the collection.
        /// </ summary >
        int Count { get; }
        /// <summary >
        /// Removes all items from the collection .
        /// </ summary >
        void Clear();
        /// <summary >
        /// Determines whether the collection contains a specific value .
        /// </ summary >
        bool Contains(X item);
    }

    public class GenericList<X> : IGenericList<X>
    {
        private X[] _internalStorage;
        int size;
        int csize = 0;

        public GenericList()
        {
            _internalStorage = new X[4];
            size = 4;
        }

        public GenericList(int initialSize)
        {
            _internalStorage = new X[initialSize];
            size = initialSize;
        }

        public int Count
        {
            get
            {
                return csize;
            }
        }

        public void Add(X item)
        {
            if (csize>= size)
            {
                X[] temp = _internalStorage;
                _internalStorage = new X[2 * size];
                temp.CopyTo(_internalStorage, size);
                size = 2 * size;
            }
            _internalStorage[csize] = item;
            csize++;
        }

        public void Clear()
        {
            _internalStorage= new X[size];
            csize = 0;
        }

        public bool Contains(X item)
        {
            for (int i = 0; i < size; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public X GetElement(int index)
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

        public int IndexOf(X item)
        {
            for (int i = 0; i < size; i++)
            {
                if (_internalStorage[i].Equals(item)) return i;
            }
            return -1;
        }

        public bool Remove(X item)
        {
            for (int i = 0; i < size; i++)
            {
                if (_internalStorage[i].Equals(item))
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

            for (int i = index; i < size-1; i++)
            {
                _internalStorage[i] = _internalStorage[i + 1];
            }
            csize--;
            return true;
        }
    }
}
