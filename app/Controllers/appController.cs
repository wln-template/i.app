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
    /// 管理后台（框架部分）
    /// </summary>
    public partial class appController : iController
    {
        /// <summary>
        /// 后台首页(登录页)
        /// </summary>
        /// <returns></returns>
        public IActionResult index()
        {
            return Content("sid=" + sid + "&wkey=" + wkey);
        }
    }
}
