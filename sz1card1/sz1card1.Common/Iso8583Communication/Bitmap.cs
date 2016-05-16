using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Iso8583Communication
{
    internal class IsoBitmap
    { 
        //ISO 8583 包的最大域数：64
        internal const int FieldCount = 64;
        private BitArray array;

        public IsoBitmap()
        {
            this.array = new BitArray(IsoBitmap.FieldCount);
        }
        public IsoBitmap(byte[] map)
        {
            Array.Reverse(map);
            this.array = new BitArray(map);
            Array.Reverse(map);
        }
        /// <summary>
        /// 获取一个值，指示位图是否是全128字段的。
        /// </summary>
        public bool IsFull
        {
            get
            {
                return false;
            }
        }

        public byte[] GetBytes()
        {
            byte[] map = new byte[8];
            this.array.CopyTo(map, 0);
            Array.Reverse(map);
            return map;
        }

        public bool Get(int bitNum)
        {
            return this.array.Get(FieldCount - bitNum);
        }
        public void Set(int bitNum, bool value)
        {
            this.array.Set(FieldCount - bitNum, value);
        }
        public void CopyTo(Array array, int index)
        {
            byte[] map = this.GetBytes();
            map.CopyTo(array, index);
        }
        public bool IsEqual(byte[] map)
        {
            byte[] thisMap = this.GetBytes();
            if (thisMap.Length != map.Length)
                return false;
            for (int i = 0; i < map.Length; i++)
                if (map[i] != thisMap[i])
                    return false;
            return true;
        }
    }
}
