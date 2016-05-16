using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace sz1card1.Common.Distributed
{
    [Serializable]
    public class DataContainer : ISerializable
    {
        private int timeout;
        private DataCollection itemCollection;

        protected DataContainer(SerializationInfo info, StreamingContext context)
        {
            itemCollection = (DataCollection)info.GetValue("ItemCollection", typeof(DataCollection));
            this.timeout = info.GetInt32("Timeout");
        }

        public DataContainer(DataCollection itemCollection, int timeout)
        {
            this.itemCollection = itemCollection;
            this.timeout = timeout;
        }

        public DataCollection ItemCollection
        {
            get { return itemCollection; }
            set { itemCollection = value; }
        }

        public int Timeout
        {
            get
            {
                return timeout;
            }
        }

        #region ISerializable Members
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ItemCollection", ItemCollection);
            info.AddValue("Timeout", Timeout);
        }
        #endregion
    }
}
