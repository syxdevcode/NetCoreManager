using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Foundatio.Storage;
using System.IO;
using NetCoreManager.EnNotesApi.WordLib;
using NetCoreManager.EnNotesApi.TranslateApi;

namespace NetCoreManager.EnNotesApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("readtxt")]
        public async Task<string> ReadTxt()
        {
            var setting = await FileHelp.GetWordArrByTxt("Setting", "WordsFileSetting.txt");

            foreach (var wordFile in setting)
            {
                var wordArr = await FileHelp.GetWordArrByTxt("EnWords", wordFile);
                foreach (var wordLine in wordArr)
                {
                    var arr = wordLine.Split('：', ':');

                    if (arr.Length < 2) throw new NullReferenceException("文本行数据异常");

                    // 英文单词
                    string word = arr[0].Trim();

                    // 注释
                    string annotation = arr[1].Trim();
                }
            }
            return null;
        }

        [HttpGet("readword")]
        public async Task<string> ReadWord()
        {
            var setting = await FileHelp.GetWordArrByTxt("Setting", "WordsFileSetting-word.txt");

            foreach (var wordFile in setting)
            {
                var wordArr = FileHelp.GetWordsByWordDocument("EnWords", wordFile);
                foreach (var wordLine in wordArr)
                {
                    var arr = wordLine.Split('：', ':');

                    if (arr.Length < 2) throw new NullReferenceException("文本行数据异常");

                    // 英文单词
                    string word = arr[0].Trim();
                    var result = await BaiduTranslateApi.Translate(word);
                    // 注释
                    string annotation = arr[1].Trim();
                }
            }
            return null;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
