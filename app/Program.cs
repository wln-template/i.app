using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Wlniao;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "ListenPort：" + Wlniao.XCore.ListenPort;
        try
        {
            using (var db = new Models.MyContext())
            {
                var isInit = Models.Setting.Get(db, "IsInit");
                if (isInit != "true")
                {
                    Models.Setting.Set(db, "IsInit", "true");
                    db.SaveChanges();
                }
            }
        }
        catch (Exception ex)
        {
            Wlniao.log.Error("初始化数据库错误：" + ex.Message);
        }
        var host = new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<Startup>()
        .UseUrls(Wlniao.XCore.ListenUrls)
        .Build();
        host.Run();
    }
}
