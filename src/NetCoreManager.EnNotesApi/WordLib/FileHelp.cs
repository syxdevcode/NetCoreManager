using Foundatio.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.EnNotesApi.WordLib
{
    public class FileHelp
    {
        /// <summary>
        /// 从指定的文件中获取单词数组
        /// </summary>
        /// <param name="folderName">文件夹名称</param>
        /// <param name="fileName">文件名</param>
        /// <returns>单词、注释组成的字符串集合</returns> 
        public static async Task<string[]> GetWordArr(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            IFileStorage storage = new FolderFileStorage(folderName);
            //var file = (await storage.GetFileListAsync(@"folderName\*")).FirstOrDefault();

            string content = await storage.GetFileContentsAsync(fileName);

            if (string.IsNullOrWhiteSpace(content))
                return new string[] { };

            // 使用当前系统环境的换行字符串
            // 并且使用中文冒号替换英文冒号
            var wordArr = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return wordArr;
        }
    }
}
