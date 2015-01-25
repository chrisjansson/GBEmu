using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public class RingBuffer<T> : IEnumerable<T>
    {
        private readonly T[] _elements;
        private int _head;

        public RingBuffer(int size)
        {
            _elements = new T[size];
        }

        public void Insert(T value)
        {
            _elements[_head % _elements.Length] = value;
            _head += 1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _elements.Length; i++)
            {
                yield return _elements[(_head + i)%_elements.Length];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}