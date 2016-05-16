﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace sz1card1.Common
{
    /// <summary>
    /// AES对称加密解密类
    /// </summary>
    public class AESHelper
    {
        #region 成员变量
        /// <summary>
        /// 密钥(32位,不足在后面补0)
        /// </summary>
        private string _aesKey;
        /// <summary>
        /// 运算模式
        /// </summary>
        private CipherMode _cipherMode = CipherMode.ECB;
        /// <summary>
        /// 填充模式
        /// </summary>
        private PaddingMode _paddingMode = PaddingMode.PKCS7;
        /// <summary>
        /// 字符串采用的编码
        /// </summary>
        private Encoding _encoding = Encoding.UTF8;
        public AESHelper(string aesKey)
        {
            _aesKey = aesKey;
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 获取32byte密钥数据
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private byte[] GetKeyArray(string password)
        {
            if (password == null)
            {
                password = string.Empty;
            }

            if (password.Length < 32)
            {
                password = password.PadRight(32, '0');
            }
            else if (password.Length > 32)
            {
                password = password.Substring(0, 32);
            }

            return _encoding.GetBytes(password);
        }

        /// <summary>
        /// 将字符数组转换成字符串
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private string ConvertByteToString(byte[] inputData)
        {
            StringBuilder sb = new StringBuilder(inputData.Length * 2);
            foreach (var b in inputData)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将字符串转换成字符数组
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private byte[] ConvertStringToByte(string inputString)
        {
            if (inputString == null || inputString.Length < 2)
            {
                throw new ArgumentException();
            }
            int l = inputString.Length / 2;
            byte[] result = new byte[l];
            for (int i = 0; i < l; ++i)
            {
                result[i] = Convert.ToByte(inputString.Substring(2 * i, 2), 16);
            }

            return result;
        }
        #endregion

        #region 加密
        /// <summary>
        /// 加密字节数据
        /// </summary>
        /// <param name="inputData">要加密的字节数据</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] inputData, string password)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = GetKeyArray(password);
            aes.Mode = _cipherMode;
            aes.Padding = _paddingMode;
            ICryptoTransform transform = aes.CreateEncryptor();
            byte[] data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
            aes.Clear();
            return data;
        }

        /// <summary>
        /// 加密字符串(加密为16进制字符串)
        /// </summary>
        /// <param name="inputString">要加密的字符串</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public string Encrypt(string inputString, string password)
        {
            byte[] toEncryptArray = _encoding.GetBytes(inputString);
            byte[] result = Encrypt(toEncryptArray, password);
            return ConvertByteToString(result);
        }

        /// <summary>
        /// 字符串加密(加密为16进制字符串)
        /// </summary>
        /// <param name="inputString">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public string EncryptString(string inputString)
        {
            return Encrypt(inputString, _aesKey);
        }
        #endregion

        #region 解密
        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="inputData">要解密的字节数据</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] inputData, string password)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = GetKeyArray(password);
            aes.Mode = _cipherMode;
            aes.Padding = _paddingMode;
            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] data = null;
            try
            {
                data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
            }
            catch
            {
                return null;
            }
            aes.Clear();
            return data;
        }

        /// <summary>
        /// 解密16进制的字符串为字符串
        /// </summary>
        /// <param name="inputString">要解密的字符串</param>
        /// <param name="password">密码</param>
        /// <returns>字符串</returns>
        public string Decrypt(string inputString, string password)
        {
            byte[] toDecryptArray = ConvertStringToByte(inputString);
            string decryptString = _encoding.GetString(Decrypt(toDecryptArray, password));
            return decryptString;
        }

        /// <summary>
        /// 解密16进制的字符串为字符串
        /// </summary>
        /// <param name="inputString">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public string DecryptString(string inputString)
        {
            return Decrypt(inputString, _aesKey);
        }
        #endregion
        #region 生成N位不重复的数
        /// <summary>
        /// 生成N位不重复的数
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public static string RandMemberPwdKey(int N)
        {
            char[] basicChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder randCode = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < N; i++)
            {
                randCode.Append(basicChar[rnd.Next(0, basicChar.Length)].ToString());
            }
            return randCode.ToString();
        }
        #endregion

    }
}
