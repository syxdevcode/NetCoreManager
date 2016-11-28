using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace NetCoreManager.Component.Tools.Encrypt
{
    public class EncryptHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password">待加密字符</param>
        /// <param name="salt">盐:byte[]类型</param>
        /// <param name="prf">加密方式</param>
        /// <param name="iterationCount">加密循环次数</param>
        /// <param name="numBytesRequested">派生密钥的期望长度（以字节为单位）。</param>
        /// <returns></returns>
        public static string Encrypt(string password, byte[] salt, KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256, int iterationCount = 3, int numBytesRequested = 128)
        {
            byte[] result = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, numBytesRequested);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password">待加密字符</param>
        /// <param name="strSalt">盐:字符串类型</param>
        /// <param name="prf">加密方式</param>
        /// <param name="iterationCount">加密循环次数</param>
        /// <param name="numBytesRequested">派生密钥的期望长度（以字节为单位）。</param>
        /// <returns></returns>
        public static string Encrypt(string password, string strSalt, KeyDerivationPrf prf= KeyDerivationPrf.HMACSHA256, int iterationCount=3, int numBytesRequested=128)
        {
            byte[] salt = Encoding.UTF8.GetBytes(strSalt);

            return Encrypt(password, salt, prf, iterationCount, numBytesRequested);
        }
        
    }
}
