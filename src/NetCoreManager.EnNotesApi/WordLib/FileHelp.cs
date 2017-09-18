using Foundatio.Storage;
using NPOI.XWPF.UserModel;
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
        public static async Task<string[]> GetWordArrByTxt(string folderName, string fileName)
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
            var wordArr = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return wordArr;
        }

        public static List<string> GetWordsByWordDocument(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            try
            {
                var newFile3 = Path.Combine(Path.GetFullPath(folderName), fileName);
                XWPFDocument doc = null;
                using (var fs = new FileStream(newFile3, FileMode.Open, FileAccess.Read))
                {
                    doc = new XWPFDocument(fs);
                    var body = doc.BodyElements;
                    List<string> result = new List<string>();

                    foreach(var item in body)
                    {
                        result.Add(((NPOI.XWPF.UserModel.XWPFParagraph)item).Text);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

    }
}
