using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public abstract class BlittableCollection<T> : IList<T>, IReadOnlyList<T>
        where T : struct
    {
        #region Fields
        private T[] _array;

        private int _count;
        #endregion

        #region Constructors
        internal BlittableCollection()
        {
        }
        #endregion

        #region Properties
        public int Count
        {
            get { return _count; }
        }

        protected abstract int SizeOfItem
        {
            get;
        }

        internal int SizeInBytes
        {
            get { return sizeof(ushort) + Count * SizeOfItem; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Indexers
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Contract.EndContractBlock();

                return _array[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Contract.EndContractBlock();

                _array[index] = value;
            }
        }
        #endregion

        #region Methods  
        public void Add(T item)
        {
            Insert(_count, item);
        }

        public void Clear()
        {
            _array = null;
            _count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_array, 0, array, arrayIndex, _count);
        }

        public int IndexOf(T item)
        {
            return _array != null ? Array.IndexOf(_array, item, 0, _count) : -1;
        }

        public void Insert(int index, T item)
        {
            if (_array == null)
            {
                _array = new T[16];
            }
            else if (_array.Length == _count)
            {
                Array.Resize(ref _array, _array.Length << 1);
            }

            Array.ConstrainedCopy(_array, index, _array, index + 1, _count - index);
            _array[index] = item;
            ++_count;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; ++i)
            {
                yield return _array[i];
            }
        }

        protected abstract void ReadItems(Stream stream, T[] items, int start, int count);

        protected abstract void WriteItems(Stream stream, T[] items, int start, int count);

        internal void Read(Stream stream)
        {
            var count = stream.ReadUInt16();
            BinaryDataHelpers.SwapBytes(ref count);
            _count = count;

            if (_count > 0)
            {
                ArrayHelpers.EnsureLength(ref _array, _count);
                ReadItems(stream, _array, 0, _count);
            }
        }

        internal unsafe void Write(Stream stream)
        {
            var count = (ushort)_count;
            BinaryDataHelpers.SwapBytes(ref count);
            stream.Write(count);

            if (_count > 0)
            {
                WriteItems(stream, _array, 0, _count);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}