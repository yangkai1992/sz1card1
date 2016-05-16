﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Web.SessionState;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    ///   Implementation of a <see cref = "ISessionStateItemCollection" /> that can be efficiently serialized.
    /// </summary>
    [Serializable]
    public class SerializableSessionStateItemCollection : ISessionStateItemCollection, ISerializable
    {
        private readonly SortedList internalCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref = "SerializableSessionStateItemCollection" /> class.
        /// </summary>
        public SerializableSessionStateItemCollection()
        {
            internalCollection = new SortedList(25);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "SerializableSessionStateItemCollection" /> class.
        /// </summary>
        /// <param name = "info">The info.</param>
        /// <param name = "context">The context.</param>
        protected SerializableSessionStateItemCollection(SerializationInfo info, StreamingContext context)
        {
            internalCollection = (SortedList)info.GetValue("internalCollection", typeof(SortedList));
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
        }
        #endregion

        #region ISessionStateItemCollection Members
        /// <summary>
        /// Deletes an item from the collection.
        /// </summary>
        /// <param name = "name">The name of the item to delete from the collection.</param>
        public void Remove(string name)
        {
            Dirty = true;
            internalCollection.Remove(name);
        }

        /// <summary>
        /// Gets or sets a value in the collection by numerical index.
        /// </summary>
        /// <returns>
        /// The value in the collection stored at the specified index.
        /// </returns>
        /// <param name = "index">The numerical index of the value in the collection.</param>
        public object this[int index]
        {
            get { return internalCollection[index]; }
            set
            {
                Dirty = true;
                internalCollection.SetByIndex(index, value);
            }
        }

        /// <summary>
        /// Gets a collection of the variable names for all values stored in the collection.
        /// </summary>
        /// <returns>
        /// The <see cref = "T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> that contains all the collection keys.
        /// </returns>
        NameObjectCollectionBase.KeysCollection ISessionStateItemCollection.Keys
        {
            get { return (NameObjectCollectionBase.KeysCollection)internalCollection.Keys; }
        }

        /// <summary>
        /// Gets a collection of the variable names for all values stored in the collection.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection"/> that contains all the collection keys.</returns>
        protected NameObjectCollectionBase.KeysCollection Keys
        {
            get { return (this as ISessionStateItemCollection).Keys; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the collection has been marked as changed.
        /// </summary>
        /// <returns>
        /// true if the <see cref = "T:System.Web.SessionState.SessionStateItemCollection" /> contents have been changed; otherwise, false.
        /// </returns>
        public bool Dirty { get; set; }

        void ISessionStateItemCollection.Clear()
        {
            Dirty = true;
            internalCollection.Clear();
        }

        /// <summary>
        /// Removes all values and keys from the session-state collection.
        /// </summary>
        protected void Clear()
        {
            (this as ISessionStateItemCollection).Clear();
        }

        /// <summary>
        /// Gets or sets a value in the collection by name.
        /// </summary>
        /// <returns>
        /// The value in the collection with the specified name.
        /// </returns>
        /// <param name = "name">The key name of the value in the collection.</param>
        object ISessionStateItemCollection.this[string name]
        {
            get { return internalCollection[name]; }
            set
            {
                Dirty = true;
                internalCollection[name] = value;
            }
        }

        /// <summary>
        /// Deletes an item at a specified index from the collection.
        /// </summary>
        /// <param name = "index">The index of the item to remove from the collection.</param>
        public void RemoveAt(int index)
        {
            Dirty = true;
            internalCollection.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref = "T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return internalCollection.GetEnumerator();
        }

        /// <summary>
        /// Copies the elements of the <see cref = "T:System.Collections.ICollection" /> to an <see cref = "T:System.Array" />, starting at a particular <see cref = "T:System.Array" /> index.
        /// </summary>
        /// <param name = "array">The one-dimensional <see cref = "T:System.Array" /> that is the destination of the elements copied from <see cref = "T:System.Collections.ICollection" />. The <see cref = "T:System.Array" /> must have zero-based indexing. </param>
        /// <param name = "index">The zero-based index in <paramref name = "array" /> at which copying begins. </param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "array" /> is null. </exception>
        /// <exception cref = "T:System.ArgumentOutOfRangeException"><paramref name = "index" /> is less than zero. </exception>
        /// <exception cref = "T:System.ArgumentException"><paramref name = "array" /> is multidimensional.-or- The number of elements in the source <see cref = "T:System.Collections.ICollection" /> is greater than the available space from <paramref name = "index" /> to the end of the destination <paramref name = "array" />. </exception>
        /// <exception cref = "T:System.ArgumentException">The type of the source <see cref = "T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name = "array" />. </exception>
        /// <filterpriority>2</filterpriority>
        public void CopyTo(Array array, int index)
        {
            internalCollection.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref = "T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref = "T:System.Collections.ICollection" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public int Count
        {
            get { return internalCollection.Count; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref = "T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref = "T:System.Collections.ICollection" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object SyncRoot
        {
            get { return ((ICollection)internalCollection).SyncRoot; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref = "T:System.Collections.ICollection" /> is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// true if access to the <see cref = "T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public bool IsSynchronized
        {
            get { return ((ICollection)internalCollection).IsSynchronized; }
        }
        #endregion
    }

}
