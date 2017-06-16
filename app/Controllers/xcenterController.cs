using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wlniao;
using Wlniao.i;
using Wlniao.XServer;

namespace App.Controllers
{
    /// <summary>
    /// XCenter端交互部分
    /// </summary>
    public partial class xcenterController : XCoreController
    {
        /// <summary>
        /// iApp注册
        /// </summary>
        /// <returns></returns>
        [Route("install")]
        public IActionResult install()
        {
            if (method == "check")
            {
                var ihost = GetRequestNoSecurity("ihost");
                var s = (ihost.IndexOf('?') > 0 ? ihost.Substring(0, ihost.IndexOf('?')) : ihost).Split(new[] { "://", "/" }, StringSplitOptions.RemoveEmptyEntries);
                ihost = ihost.IndexOf("://") > 0 ? (s[0] + "://" + strUtil.Join("/", s.Skip(1).ToArray())) : strUtil.Join("/", s);
                var icommon = Wlniao.i.iCommon.Create(ihost, false);
                if (icommon == null)
                {
                    return Json(new { message = "请输入有效的Wlniao.i服务器地址" });
                }
                else if (icommon.iRegister > DateTime.Now)
                {
                    return Json(new { message = "此应用已注册，请勿重复安装" });
                }
                else if (string.IsNullOrEmpty(icommon.iSecret))
                {
                    return Json(new { message = icommon.iMessage });
                }
                else
                {
                    return Json(new { success = true, data = ihost });
                }
            }
            return View();
        }
        /// <summary>
        /// iApp注册
        /// </summary>
        /// <returns></returns>
        [Route("enroll")]
        public IActionResult enroll()
        {
            var common = Wlniao.i.iCommon.Create(GetRequestNoSecurity("ihost"), false);
            if (common == null)
            {
                errorMsg = "请输入有效的Wlniao.i服务器地址";
            }
            else if (common.iRegister > DateTime.Now)
            {
                ViewBag.SuccessMessage = "此应用已注册，请勿重复安装，请返回 <a href=\"" + (common.Host.IndexOf("://") > 0 ? common.Host : "http://" + common.Host) + "\">" + common.Host + "</a> 使用";
            }
            else if (string.IsNullOrEmpty(common.iSecret))
            {
                errorMsg = common.iMessage;
            }
            else
            {
                var menus = new List<AppMenu>();
                menus.Add(new AppMenu { sort = 1, code = "home", name = "收费管理", link = "/", icon = "/icons/jiaofei.svg" });
                menus.Add(new AppMenu { sort = 2, code = "student", name = "学员管理", link = "/student", appto = "school" });
                menus.Add(new AppMenu { sort = 8, code = "report", name = "统计报表", link = "/report"});
                menus.Add(new AppMenu { sort = 9, code = "setting", name = "收费设置", link = "/setting"});

                var obj = new { menus = menus };
                var rlt = common.Post<String>("app", "enroll", Newtonsoft.Json.JsonConvert.SerializeObject(obj)
                    , new KeyValuePair<string, string>("app", Wlniao.i.iCommon.iApp)
                    , new KeyValuePair<string, string>("name", "收费系统")
                    , new KeyValuePair<string, string>("host", Request.Host.Value)
                    , new KeyValuePair<string, string>("secret", common.iSecret));
                if (rlt.success)
                {
                    //install success
                    common.iRegister = DateTime.Now.AddMinutes(30);
                    ViewBag.SuccessMessage = "应用部署成功，请返回 <a href=\"" + (common.Host.IndexOf("://") > 0 ? common.Host : "http://" + common.Host) + "\">" + common.Host + "</a> 使用";
                }
                else
                {
                    errorMsg = rlt.message;
                }
            }
            return View("install");
        }
    }
}
