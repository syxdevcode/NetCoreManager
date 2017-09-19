using NetCoreManager.Component.Tools.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreManager.EnNotesApi.TranslateApi
{
    /// <summary>
    /// 有道翻译api
    /// 参考地址：http://ai.youdao.com/docs/doc-trans-api.s#p02
    /// </summary>
    public class YouDaoTranslateApi
    {
        private static readonly string AppId = "48582dba94123753";

        private static readonly string AppSecret = "gM2bNwGPY3S2cZn131Z9PhNsWUQvnaii";

        private static readonly string t_HttpUrl = "http://openapi.youdao.com/api";

        private static readonly string t_HttpsUrl = "https://openapi.youdao.com/api";

        /// <summary>
        /// type=1 --英音
        /// type=2 --美音
        /// </summary>
        private static readonly string VoiceUrl = "https://dict.youdao.com/dictvoice";

        /// <summary>
        /// 调用接口翻译方法
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static async Task<YouDaoTranslateModel> Translate(string word)
        {
            var salt = TimeHelper.GetUnixTimestamp();

            word = System.Net.WebUtility.HtmlEncode(word);

            var signStr = string.Format("{0}{1}{2}{3}", AppId, word, salt, AppSecret);
            var sign = Md5Helper.GetMD5(signStr).ToUpper();
            
            string requestUrl = string.Format("{0}?q={1}&from={2}&to={3}&appKey={4}&salt={5}&sign={6}", t_HttpUrl, word, "en", "zh", AppId, salt, sign);

            var result = await HttpHelper.HttpGetAsync(requestUrl, Encoding.UTF8);
            var model = Json.ToObject<YouDaoTranslateModel>(result);

            return model;
        }

        /// <summary>
        /// 调用读音接口 --废弃
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static async Task<string> DictVoice(string word)
        {
            string requestUrl = string.Format("{0}?audio={1}", VoiceUrl, word);
            var result = await HttpHelper.HttpGetAsync(requestUrl, Encoding.UTF8);
            return string.Empty;
        }

    }

    public class YouDaoTranslateModel
    {
        public string query { get; set; }
        public List<string> translation { get; set; }

        public urlModel dict {get;set; }

        public urlModel webdict { get; set; }

        public string l { get; set; }

        public string errorCode { get; set; }

    }

    public class urlModel
    {
        public string url { get; set; }
    }

    public class YouDao
    {
        public string src { get; set; }

        public string dst { get; set; }
    }
}
