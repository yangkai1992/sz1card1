using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace sz1card1.Common.Security
{
    public class AESUtil
    {
        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="plainText">明文字符串</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回加密后的密文字节数组</returns>  
        public static string AESEncrypt(string plainText, string strKey)
        {
            return AESEncrypt(plainText, strKey, null);
        }

        /// <summary>
        /// AES加密算法  
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="strVector">向量</param>
        /// <returns></returns>
        public static string AESEncrypt(string plainText, string strKey, string strVector)
        {
            //分组加密算法  
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组      
            //设置密钥及密钥向量  
            des.Key = Encoding.ASCII.GetBytes(strKey);
            if (!string.IsNullOrEmpty(strVector))
                des.IV = Encoding.ASCII.GetBytes(strVector);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组  
            cs.Close();
            ms.Close();
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="cipherText">密文Base64编码字符串</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static string AESDecrypt(string cipherText, string strKey)
        {
            return AESDecrypt(cipherText, strKey, null);
        }

        /// <summary>
        /// AES解密  
        /// </summary>
        /// <param name="cipherText">密文Base64编码字符串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="strVector">向量</param>
        /// <returns></returns>
        public static string AESDecrypt(string cipherText, string strKey, string strVector)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.ASCII.GetBytes(strKey);
            if (!string.IsNullOrEmpty(strVector))
                des.IV = Encoding.ASCII.GetBytes(strVector);
            byte[] decryptBytes = new byte[cipherText.Length];
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText));
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            int len = cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            ms.Close();
            return Encoding.UTF8.GetString(decryptBytes, 0, len);
        }
    }
}
