using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreManager.Component.Tools.Lib
{
    public class Md5Helper
    {
        public static string GetMD5(string str)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
