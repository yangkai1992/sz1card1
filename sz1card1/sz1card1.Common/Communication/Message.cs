using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace sz1card1.Common.Communication
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Message
    {
        private int length = -1;
        private MessageType type;
        private Guid identity;
        private string action;
        private object[] body;

        public Message(Guid identity, string action)
            : this(identity, action, new object[] { })
        {
        }

        public Message(Guid identity, string action, object body)
            : this(identity, action, new object[] { body })
        {
        }

        public Message(Guid identity, string action, object[] body)
        {
            this.identity = identity;
            this.action = action;
            this.body = body;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid Identity
        {
            get
            {
                return identity;
            }
        }

        /// <summary>
        /// 操作符
        /// </summary>
        public string Action
        {
            get
            {
                return action;
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public object[] Body
        {
            get
            {
                return body;
            }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// 消息长度
        /// </summary>
        public int Length
        {
            get
            {
                if (length == -1)
                {
                    length = ToBytes().Length;
                }
                return length;
            }
        }

        /// <summary>
        /// 将消息转化为二进制
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            Stream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);
            writer.Write((byte)type);//类型
            writer.Write(0);//长度
            writer.Write(Encoding.UTF8.GetBytes(identity.ToString()));
            writer.Write(Encoding.UTF8.GetBytes(action));
            if (body != null)
            {
                foreach (object obj in body)
                {
                    writer.Write(Separator.PS);
                    if (obj != null)
                        writer.Write(Encoding.UTF8.GetBytes(obj.ToString()));
                }
            }
            stream.Seek(0, SeekOrigin.Begin);
            byte[] result = new byte[stream.Length];
            stream.Read(result, 0, result.Length);
            stream.Close();
            int size = result.Length - 5;
            byte[] intArr = BitConverter.GetBytes(size);
            result[1] = intArr[0];
            result[2] = intArr[1];
            result[3] = intArr[2];
            result[4] = intArr[3];
            length = result.Length;
            return result;
        }

        /// <summary>
        /// 从二进制中读取消息
        /// </summary>
        /// <param name="bytes">二进制数据</param>
        /// <returns>消息</returns>
        public static Message FromBytes(byte[] bytes)
        {
            return FromBytes(bytes, bytes.Length);
        }

        /// <summary>
        /// 从二进制中读取消息
        /// </summary>
        /// <param name="bytes">二进制数据</param>
        /// <param name="length">有效数据长度</param>
        /// <returns>消息</returns>
        public static Message FromBytes(byte[] bytes, int length)
        {
            Message message = null;
            using (Stream stream = new MemoryStream(bytes))
            {
                BinaryReader reader = new BinaryReader(stream);
                byte t = reader.ReadByte();
                int size = reader.ReadInt32();
                if (size > 0 && length >= stream.Position + size)
                {
                    byte[] content = reader.ReadBytes(size);
                    string result = Encoding.UTF8.GetString(content, 0, content.Length);
                    string[] arr = result.Split(Separator.PS);
                    string identity = arr[0].Substring(0, 36);
                    string action = arr[0].Substring(36);
                    if (arr.Length > 1)
                    {
                        object[] body = new object[arr.Length - 1];
                        for (int i = 0; i < body.Length; i++)
                        {
                            body[i] = arr[i + 1];
                        }
                        message = new Message(new Guid(identity), action, body);
                    }
                    else
                    {
                        message = new Message(new Guid(identity), action);
                    }
                    message.Type = (MessageType)t;
                    message.length = size + 5;
                }
                reader.Close();
            }
            return message;
        }
    }
}
