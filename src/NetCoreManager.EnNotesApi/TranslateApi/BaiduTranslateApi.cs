using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreManager.Component.Tools.Lib;
using System.Security.Cryptography;
using System.Text;

namespace NetCoreManager.EnNotesApi.TranslateApi
{
    /// <summary>
    /// 百度翻译接口
    /// 参考文档：http://api.fanyi.baidu.com/api/trans/product/apidoc
    /// </summary>
    public class BaiduTranslateApi
    {
        private static readonly string AppId = "20170918000083575";

        private static readonly string AppSecret = "TUYMy8b58r5VNYi1o3MH";

        private static readonly string HttpUrl = "http://fanyi-api.baidu.com/api/trans/vip/translate";

        private static readonly string HttpsUrl = "https://fanyi-api.baidu.com/api/trans/vip/translate";

        /// <summary>
        /// 调用接口翻译方法
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static async Task<BaiDuTranslateModel> Translate(string word)
        {
            string requestUrl = string.Empty;

            var salt = TimeHelper.GetUnixTimestamp();

            word = System.Net.WebUtility.HtmlEncode(word);

            var signStr = string.Format("{0}{1}{2}{3}", AppId, word, salt, AppSecret);
            var sign = Md5Helper.GetMD5(signStr);

            string par = string.Format("{0}?q={1}&from={2}&to={3}&appid={4}&salt={5}&sign={6}", HttpUrl, word, "en", "zh", AppId, salt, sign);

            var result = await HttpHelper.HttpGetAsync(par, Encoding.UTF8);
            var model = Json.ToObject<BaiDuTranslateModel>(result);

            return model;
        }

        

    }

    public class BaiDuTranslateModel
    {
        public string from { get; set; }

        public string to { get; set; }

        public List<Trans_resultModel> trans_result { get; set; }
    }

    public class Trans_resultModel
    {
        public string src { get; set; }

        public string dst { get; set; }
    }
}
