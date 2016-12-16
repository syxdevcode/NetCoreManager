using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.Mvc.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "用户名不能为空。")]
        [Display(Name = "用户名")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密码不能为空。")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "验证码字段是必填项")]
        [StringLength(10, ErrorMessage = "验证码长度必须小于10个字符。")]
        [Display(Name = "验证码", Prompt = "请输入下方的验证码")]
        public string Captcha { get; set; }

        [Display(Name = "记住本次登录")]
        public bool IsRemember { get; set; }
    }
}
