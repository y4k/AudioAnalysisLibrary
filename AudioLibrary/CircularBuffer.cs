using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AudioAnalysisLibrary
{
    public class CircularBuffer<T> : IEnumerable<T>
    {
        private T[] _buffer;
        private int _head;
        private int _tail;
        private int _size;
        private int _capacity;

        public CircularBuffer(int capacity): this(capacity, false)
        {
        }

        public CircularBuffer(int capacity, bool allowOverflow)
        {
            if(capacity < 0)
            {
                throw new ArgumentException(nameof(capacity));
            }

            _buffer = new T[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
            _capacity = 0;
        }

        public CircularBuffer(IEnumerable<T> data) : this(data.Count(), false)
        {
            _buffer = data.ToArray();
        }

        public void Enqueue(T data)
        {
            _buffer[_tail++] = data;
        }

        public void Enqueue(IEnumerable<T> data)
        {
            foreach (var d in data)
            {
                Enqueue(d);
            }
        }

        public void Clear()
        {
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        public T Dequeue()
        {
            return _buffer[_head++];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            //Start at head, iterate to size, then from 0 to tail.
            int index = _head;
            for (int i = 0; i < _size; i++, index++)
            {
                if(index == _capacity)
                {
                    index = 0;
                }
                yield return _buffer[index];
            }
        }
    }
}