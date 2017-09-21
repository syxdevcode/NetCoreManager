using Android.App;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Android.Content;
using Java.Interop;

namespace AppNotes
{
    [Activity(Label = "AppNotes", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // 获取WebView对象
            var webView = FindViewById<WebView>(Resource.Id.webView1);

            // 申明WebView的配置
            WebSettings settings = webView.Settings;

            // 设置允许执行JS
            settings.JavaScriptEnabled = true;

            //设置可以通过JS打开窗口
            settings.JavaScriptCanOpenWindowsAutomatically = true;

            //自己创建的WebView客户端类
            var webc = new MyCommWebClient();

            //设置自己的WebView客户端
            webView.SetWebViewClient(webc);

            webView.LoadUrl("http://192.168.3.174:8090/");

            // 实现本地代码调用页面js代码
            var button = FindViewById<Button>(Resource.Id.button1);
            button.Click += delegate
            {
                webView.LoadUrl("javascript:" + "showmessage('安卓按钮点击')");
                //button.Text = string.Format("这是第{0} 单击!", count++);
            };

            // js 返回值  
            var btn = FindViewById<Button>(Resource.Id.button2);//添加点击事件
            btn.Click += delegate
            {
                ValueCall vc = new ValueCall();
                //添加弹出返回值事件
                vc.TestEvent += ShowMessage;
                //调用JS
                webView.EvaluateJavascript("showmessage1('安卓按钮点击,带返回值')", vc);
            };

            // 调用C#代码
            //添加我们刚创建的类,并命名为wv 
            webView.AddJavascriptInterface(new MyJsInterface(this), "wv");

            var btn3 = FindViewById<Button>(Resource.Id.button3);//添加点击事件
            btn3.Click += delegate
            {
                Intent intent = new Intent(this, typeof(SaoYiSaoActivity));
                intent.AddFlags(ActivityFlags.SingleTop);
                StartActivityForResult(intent, 1);
            };

            // 调用电话
            Android.Net.Uri uri = Android.Net.Uri.Parse("tel:15801654715");
            Intent it = new Intent(Intent.ActionDial, uri);
            StartActivity(it);

            // 调用短信
            Android.Net.Uri uri1 = Android.Net.Uri.Parse("smsto:10010");
            Intent it1 = new Intent(Intent.ActionSendto, uri1);
            it.PutExtra("sms_body", "你妹妹,我没欠费 你给我停机!");
            StartActivity(it1);

            // 调用地图
            Android.Net.Uri uri2 = Android.Net.Uri.Parse("geo:38.899533,-77.036476");
            Intent it2 = new Intent(Intent.ActionView, uri2);
            StartActivity(it);

            //调用Email
            Android.Net.Uri uri3 = Android.Net.Uri.Parse("mailto:777@qq.com?cc=aa@qq.com&subject=标题&body=内容");
            var intent1 = new Intent(Intent.ActionView, uri3);
            StartActivity(intent1);

            //调用通讯录
            //第一种(建议使用第一种)
            Android.Net.Uri uri4 = Android.Net.Uri.Parse("content://contacts/people");
            Intent it4 = new Intent(Intent.ActionPick, uri4);
            StartActivityForResult(it4, 11);
            //第二种 
            Intent i = new Intent();
            i.SetAction(Intent.ActionGetContent);
            i.SetType("vnd.android.cursor.item/phone");
            StartActivityForResult(i, 11);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //如果当初的发的requestCode =1
            if (requestCode == 1 && resultCode == Result.Ok)
            {
                // 获取WebView对象
                var webView = FindViewById<WebView>(Resource.Id.webView1);
                webView.LoadUrl(data.GetStringExtra("code"));
                Toast.MakeText(this, "扫描结果:" + data.GetStringExtra("code"), ToastLength.Short).Show();
            }
        }

        public void ShowMessage(string message)
        {
            //很简单就是弹出返回值
            Toast.MakeText(this.ApplicationContext, message, ToastLength.Short).Show();
        }
    }

    /// <summary>
    /// 监测页面中的A标签
    /// </summary>
    class MyCommWebClient : WebViewClient
    {
        /// <summary>
        /// 重写方法,监测页面中的A标签，实现控件内跳转，而不调用本地默认浏览器
        /// </summary>
        /// <param name="view"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public override bool ShouldOverrideUrlLoading(WebView webView, string url)
        {
            // 使用本控件加载
            webView.LoadUrl(url);
            //webView.LoadUrl("javascript:" + "showmessage('安卓按钮点击')");
            return true;
        }
    }

    /// <summary>
    /// js 返回值
    /// </summary>
    public class ValueCall : Java.Lang.Object, IValueCallback
    {
        /// <summary>
        /// 定义委托
        /// </summary>
        /// <param name="message"></param>
        public delegate void TestEventHandler(string message);

        /// <summary>
        /// 用 event关键字声明事件
        /// </summary>
        public event TestEventHandler TestEvent;

        public void Dispose() { }

        /// <summary>
        /// 重写方法，获取返回值
        /// </summary>
        /// <param name="value"></param>
        public void OnReceiveValue(Java.Lang.Object value)
        {
            string a = value.ToString();
            TestEvent(a);
        }
    }

    /// <summary>
    /// js 调用C#代码
    /// </summary>
    public class MyJsInterface : Java.Lang.Object
    {
        Context context;

        //因为要弹出内容..所以构造函数需要一个当前的上下文对象
        public MyJsInterface(Context context)
        {
            this.context = context;
        }

        //注意,这里需要加两个特性
        [Export]
        [JavascriptInterface]
        public void SayHello(string message)
        {
            Toast.MakeText(context, message, ToastLength.Short).Show();
        }
    }
}

