using Android.App;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using System;
using Android.Content;
using System.Text;
using Android.Runtime;
using ZXing;
using System.Security.Cryptography;
using Android.Graphics;
using System.Collections.Generic;

namespace FMSKeygen_Android
{
    [Activity(Label = "注册机", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EditText txtMachineCode = null;
        private EditText txtActiviationCode = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // 初始化二维码扫描仪，后面要用到
            MobileBarcodeScanner.Initialize(Application);

            txtMachineCode = FindViewById<EditText>(Resource.Id.txtMachineCode);
            //设置自动转换小写字母为大写
            txtMachineCode.SetFilters(new Android.Text.IInputFilter[] { new Android.Text.InputFilterAllCaps() });
            txtActiviationCode = FindViewById<EditText>(Resource.Id.txtActiviationCode);
            //取消对验证码文本框的所有按键监听
            txtActiviationCode.KeyListener = null;

            // 清除事件
            Button btnclear = FindViewById<Button>(Resource.Id.btnClear);
            btnclear.Click += Btnclear_Click;

            //获取注册码
            Button btngetactiviationcode = FindViewById<Button>(Resource.Id.btnGetActiviationCode);
            btngetactiviationcode.Click += Btngetactiviationcode_Click;

            // 扫描二维码
            Button btnscanqrcode = FindViewById<Button>(Resource.Id.btnScanQRCode);
            btnscanqrcode.Click += Btnscanqrcode_Click;

            //复制
            Button btncopy = FindViewById<Button>(Resource.Id.btnCopy);
            btncopy.Click += Btncopy_Click;

            //读取二维码
            Button btnreadqrcode = FindViewById<Button>(Resource.Id.btnReadQRCode);
            btnreadqrcode.Click += Btnreadqrcode_Click;

            //分享
            Button btnshare = FindViewById<Button>(Resource.Id.btnShare);
            btnshare.Click += Btnshare_Click;
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnshare_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtActiviationCode.Text))
            {
                var dlg = new AlertDialog.Builder(this).SetTitle("警告")
                    .SetMessage("请先获取激活码！");
                dlg.Show();
                return;
            }
            string strerr = ValidateFormat(txtMachineCode.Text);
            if (strerr != string.Empty)
            {
                var dlg = new AlertDialog.Builder(this).SetTitle("警告")
                    .SetMessage("输入的机器码格式不正确！\n" + strerr);
                dlg.Show();
                return;
            }
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");//所有可以分享文本的app
            StringBuilder strbcontent = new StringBuilder();
            strbcontent.AppendLine("机器码：" + txtMachineCode.Text)
                .AppendLine("激活码：" + txtActiviationCode.Text);
            intent.PutExtra(Intent.ExtraText, strbcontent.ToString());
            StartActivity(Intent.CreateChooser(intent, "发送激活码"));
        }

        /// <summary>
        /// 验证机器码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ValidateFormat(string str)
        {
            if (str.Length < 19)
                return "输入的格式不正确";
            if (str.Length != 19)
                str = str.Substring(0, 19);
            string[] strs = str.Split('-');
            if (strs.Length != 4)
                return "不能分隔为4组";
            foreach (string s in strs)
            {
                if (s.Length != 4)
                    return s + "的长度不是4";
                if (!System.Text.RegularExpressions.Regex.IsMatch(s, "^[A-F0-9]{4}$"))
                    return s + "的格式不正确";
            }
            return string.Empty;
        }

        /// <summary>
        /// 读取二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnreadqrcode_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            //从文件浏览器和相册等选择图像文件
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent, 1);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1 && resultCode == Android.App.Result.Ok && data != null)
            {
                // create a barcode reader instance
                IBarcodeReader reader = new BarcodeReader();
                // load a bitmap
                int width = 0, height = 0;
                //像素颜色值列表（注意：一个像素的每个颜色值都是一个列表中单独的元素，
                //后面将会把像素颜色值转换成ARGB32格式的颜色，每个像素颜色值就有4个元素加入到列表中）
                List<byte> pixelbytelist = new List<byte>();
                try
                {
                    //根据选择的文件路径生成Bitmap对象
                    using (Bitmap bmp = Android.Provider.MediaStore.Images.Media.GetBitmap(ContentResolver, data.Data))
                    {
                        width = bmp.Width; //图像宽度
                        height = bmp.Height;  //图像高度
                                              // detect and decode the barcode inside the bitmap
                        bmp.LockPixels();
                        int[] pixels = new int[width * height];
                        //一次性读取所有像素的颜色值（一个整数）到pixels
                        bmp.GetPixels(pixels, 0, width, 0, 0, width, height);
                        bmp.UnlockPixels();
                        for (int i = 0; i < pixels.Length; i++)
                        {
                            int p = pixels[i];  //取出一个像素颜色值
                                                //将像素颜色值中的alpha颜色（透明度）添加到列表
                            pixelbytelist.Add((byte)Color.GetAlphaComponent(p));
                            //将像素颜色值中的红色添加到列表
                            pixelbytelist.Add((byte)Color.GetRedComponent(p));
                            //将像素颜色值中的绿色添加到列表
                            pixelbytelist.Add((byte)Color.GetGreenComponent(p));
                            //将像素颜色值中的蓝色添加到列表
                            pixelbytelist.Add((byte)Color.GetBlueComponent(p));
                        }
                    }
                    //识别
                    var result = reader.Decode(pixelbytelist.ToArray(), width, height, RGBLuminanceSource.BitmapFormat.ARGB32);
                    if (result != null)
                    {
                        txtMachineCode.Text = result.Text.Trim();
                        Btngetactiviationcode_Click(this, null);
                    }
                    else
                        Toast.MakeText(this, "未能识别到二维码！", ToastLength.Short).Show();
                }
                catch (Exception ex)
                {
                    var dlg = new AlertDialog.Builder(this).SetTitle("警告")
                        .SetMessage("获取图像时发生错误！\n" + ex.ToString());
                    dlg.Show();
                }
            }
        }

        /// <summary>
        /// 复制注册码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btncopy_Click(object sender, EventArgs e)
        {
            ClipboardManager clip = (ClipboardManager)GetSystemService(ClipboardService);
            StringBuilder strbcontent = new StringBuilder();
            strbcontent.AppendLine("机器码：" + txtMachineCode.Text)
                .AppendLine("激活码：" + txtActiviationCode.Text);
            ClipData clipdata = ClipData.NewPlainText("激活码", strbcontent.ToString());
            clip.PrimaryClip = clipdata;
            Toast.MakeText(this, "激活码已复制到剪贴板", ToastLength.Short).Show();
        }

        /// <summary>
        /// 扫描二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btnscanqrcode_Click(object sender, EventArgs e)
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            if (result == null)
                return;
            txtMachineCode.Text = result.Text.Trim();
            Btngetactiviationcode_Click(this, null);
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btngetactiviationcode_Click(object sender, EventArgs e)
        {
            string strerr = ValidateFormat(txtMachineCode.Text);
            if (strerr != string.Empty)
            {
                var dlg = new AlertDialog.Builder(this).SetTitle("警告")
                    .SetMessage("输入的机器码格式不正确！\n" + strerr);
                dlg.Show();
                Btnclear_Click(this, null);
                return;
            }
            txtActiviationCode.Text = GetActiveCode(txtMachineCode.Text);
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnclear_Click(object sender, EventArgs e)
        {
            txtMachineCode.Text = txtActiviationCode.Text = string.Empty;
        }

        private string GetActiveCode(string machinecode)
        {
            string guid = "cd89e66c-b897-4ed8-a19f-ef5a30846f0a";
            return MD5(machinecode + MD5(guid, false, false), false, false);
        }

        private string MD5(string str, bool clearsplitter = true, bool islower = true)
        {
            var md5 = MD5CryptoServiceProvider.Create();
            var output = md5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(str));
            StringBuilder strbvalue = new StringBuilder(BitConverter.ToString(output).Replace("-", string.Empty).Substring(8, 16));
            if (!clearsplitter)
                strbvalue.Insert(12, '-').Insert(8, '-').Insert(4, '-');
            return islower ? strbvalue.ToString().ToLower() : strbvalue.ToString().ToUpper();
        }
    }
}

