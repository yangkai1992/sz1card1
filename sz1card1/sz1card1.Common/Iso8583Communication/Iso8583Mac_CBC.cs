using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;

namespace sz1card1.Common.Iso8583Communication
{
    public static class Iso8583Mac_CBC
    {
        /// <summary>
        /// Vx520 DES加解密
        /// </summary>
        /// <param name="type">0:加密 1:解密</param>
        /// <param name="Key">密钥</param>
        /// <param name="Data">要加密或解密的数据</param>
        /// <returns></returns>
        public static byte[] Des(int type, byte[] key, byte[] data)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Padding = PaddingMode.Zeros;
            des.Key = key;
            des.IV = new byte[8];

            MemoryStream ms = new MemoryStream();
            CryptoStream cs;
            if (type == 0)
            {
                cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            }
            else
            {
                cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            }

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            //关闭加密流对象
            byte[] bEncRet;
            // Array.Copy(ms.GetBuffer(), bEncRet, ms.Length);
            bEncRet = ms.ToArray(); // MyCryptoStream关闭之前ms.Length 为8， 关闭之后为16
            return bEncRet;
        }

        public static string Des(int type, string key, string data)
        {

            if (type == 0)
            {
                byte[] ms = Des(type, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(data));
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms)
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            else
            {
                byte[] inputByteArray = new byte[data.Length / 2];
                for (int x = 0; x < data.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(data.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                byte[] ms = Des(type, Encoding.UTF8.GetBytes(key), inputByteArray);
                return Encoding.UTF8.GetString(ms).Trim('\0');
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bKey">Mac密钥</param>
        /// <param name="bIV"></param>
        /// <param name="originMacData">验证的数据</param>
        /// <returns></returns>
        public static byte[] MAC_CBC(byte[] bKey, byte[] bIV, byte[] originMacData)
        {
            int iGroup = 0;
            byte[] bTmpBuf1 = new byte[8];
            byte[] bTmpBuf2 = new byte[8];
            // init

            Array.Copy(bIV, bTmpBuf1, 8);
            if ((originMacData.Length % 8 == 0))
                iGroup = originMacData.Length / 8;
            else
                iGroup = originMacData.Length / 8 + 1;

            byte[] MacData = new byte[iGroup * 8];
            Array.Copy(originMacData, MacData, originMacData.Length);

            int i = 0;
            int j = 0;
            for (i = 0; i < iGroup; i++)
            {
                Array.Copy(MacData, 8 * i, bTmpBuf2, 0, 8);
                for (j = 0; j < 8; j++)
                    bTmpBuf1[j] = (byte)(bTmpBuf1[j] ^ bTmpBuf2[j]);
                bTmpBuf2 = Des(0, bKey, bTmpBuf1);
                Array.Copy(bTmpBuf2, bTmpBuf1, 8);
            }
            return bTmpBuf2;
        }
    }
}
