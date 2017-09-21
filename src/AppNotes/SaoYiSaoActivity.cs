using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.IO;
using Android.Graphics;

namespace AppNotes
{
    [Activity(Label = "SaoYiSaoActivity")]
    public class SaoYiSaoActivity : Activity, Android.Hardware.Camera.IPreviewCallback, ISurfaceHolderCallback
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SaoYiSao);

            // 获取surfaceView1
            var surface = FindViewById<SurfaceView>(Resource.Id.surfaceView1);

            // 获取surface的线程
            var holder = surface.Holder;

            //设置线程回调为本类
            holder.AddCallback(this);

            //表明该surface不包含原生数据
            holder.SetType(SurfaceType.PushBuffers);

            //设置这个Surface的大小
            holder.SetFixedSize(200, 300);
        }
         
        public void OnPreviewFrame(byte[] data, Android.Hardware.Camera camera)
        {
            try
            {
                //获取相机宽度
                int previewWidth = camera.GetParameters().PreviewSize.Width;
                //获取相机高度
                int previewHeight = camera.GetParameters().PreviewSize.Height;
                //解析二维码
                var date = CodeDecoder(data, previewWidth, previewHeight);
                //判断是否解析到二维码.
                if (date != null)
                {
                    //跳转回主页面
                    Intent intent = new Intent(this, typeof(MainActivity));
                    //放入一个key 为code 的解析后的值
                    intent.PutExtra("code", date);
                    //状态设为OK
                    SetResult(Android.App.Result.Ok, intent);
                    //关闭当前界面
                    Finish();
                }

            }
            catch (IOException)
            {

            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
        }

        private Android.Hardware.Camera camera;

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            camera = Android.Hardware.Camera.Open();
            Android.Hardware.Camera.Parameters p = camera.GetParameters();
            p.PictureFormat = ImageFormatType.Jpeg;
            camera.SetParameters(p);
            camera.SetPreviewCallback(this);
            camera.SetPreviewDisplay(holder);
            camera.StartPreview();
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <returns></returns>
        public string CodeDecoder(byte[] data, int width, int height)
        {
            byte[] bytes = data;//获取图片字节

            //设置位图源
            PlanarYUVLuminanceSource source = new PlanarYUVLuminanceSource(data, width, height, 0, 0, width, height, false);
            //处理像素值内容信息
            BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
            //初始化解析器
            ZXing.Reader reader = new QRCodeReader();
            //解析位图
            ZXing.Result result = reader.decode(bitmap);
            if (result == null)
                return null;
            return result.Text;//返回解析结果  
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            //删除回调
            holder.RemoveCallback(this);
            //删除照相机回调
            camera.SetPreviewCallback(null);
            //停止照相机预览
            camera.StopPreview();
            //释放照相机
            camera.Release();
            camera = null;
        }
    }
}