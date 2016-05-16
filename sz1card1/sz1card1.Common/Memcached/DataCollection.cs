using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace sz1card1.Common.Memcached
{
    [Serializable]
    public class DataCollection : ISerializable, ICollection, IEnumerable
    {
        private int timeout = 240;//分钟单位

        private readonly SortedList internalCollection;

        public DataCollection()
        {
            internalCollection = new SortedList();
        }

        public DataCollection(int timeout)
            : this()
        {
            this.timeout = timeout;
        }

        protected DataCollection(SerializationInfo info, StreamingContext context)
        {
            this.internalCollection = (SortedList)info.GetValue("internalCollection", typeof(SortedList));
            this.timeout = info.GetInt32("timeout");
        }

        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        #region ISerializable Members
        /// <summary>
        /// Populates a <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name = "info">The <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
        /// <param name = "context">The destination (see <see cref = "T:System.Runtime.Serialization.StreamingContext" />) for this serialization. </param>
        /// <exception cref = "T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("internalCollection", internalCollection);
            info.AddValue("timeout", timeout);
        }
        #endregion

        public void Add(string name, object value)
        {
            internalCollection.Add(name, value);
        }

        public void Remove(string name)
        {
            internalCollection.Remove(name);
        }

        public object this[int index]
        {
            get
            {
                return internalCollection[index];
            }
            set
            {
                internalCollection.SetByIndex(index, value);
            }
        }

        public object this[string name]
        {
            get
            {
                return internalCollection[name];
            }
            set
            {
                internalCollection[name] = value;
            }
        }

        public void RemoveAt(int index)
        {
            internalCollection.RemoveAt(index);
        }

        public void Clear()
        {
            internalCollection.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return internalCollection.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            internalCollection.CopyTo(array, index);
        }

        public int Count
        {
            get { return internalCollection.Count; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)internalCollection).SyncRoot; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)internalCollection).IsSynchronized; }
        }
    }
}
