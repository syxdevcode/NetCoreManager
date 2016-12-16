using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NetCoreManager.Component.Tools.Encrypt;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.Mvc.TagHelperExtensions
{
    [HtmlTargetElement("img", Attributes = "asp-captcha")]
    public class CaptchaTagHelper : TagHelper
    {
        private static readonly CryptoTool crypto = CryptoTool.Create(CryptoTypes.EncAes);

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-captcha")]
        public bool EnableCaptCha { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (EnableCaptCha)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var bitmap = new Bitmap(115, 30, PixelFormat.Format32bppArgb))
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            Rectangle rect = new Rectangle(0, 0, 114, 29);
                            graphics.FillRectangle(Brushes.White, rect);
                            Random r = new Random();
                            int startIndex = r.Next(1, 5);
                            int length = r.Next(6, 6);
                            string drawString = Guid.NewGuid().ToString().Replace("-", "0").Substring(startIndex, length).ToLower();

                            Font drawFont = new Font("Arial", 16, FontStyle.Bold);

                            using (SolidBrush drawBrush = new SolidBrush(Color.DarkGray))
                            {
                                PointF drawPoint = new PointF(15, 3);
                                graphics.DrawRectangle(new Pen(Color.White, 0), rect);
                                graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
                            }
                            var encryptedString = crypto.Encrypt(drawString, "DotNetCore!");
                            output.PostElement.AppendFormat("<input type=\"hidden\" name=\"__captcha_image\" id=\"__captcha_image\" value=\"{0}\" />", encryptedString);
                        }
                        bitmap.Save(memoryStream, ImageFormat.Jpeg);
                        byte[] imageBytes = memoryStream.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        output.Attributes.Add("src", "data:image/png;base64," + base64String);
                    }
                }
            }

        }
    }
}
