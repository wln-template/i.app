using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
public class Startup
{
    public Startup(IHostingEnvironment env)
    {

    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEntityFrameworkMySql().AddDbContext<Models.MyContext>();
        services.AddMvc();
    }
    public void Configure(IApplicationBuilder app)
    {
        app.UseStaticFiles();
        if (Wlniao.XServer.XStorage.Local.Using)
        {
            var root = Wlniao.IO.PathTool.Map(Wlniao.XServer.XStorage.Local.Path);
            if (!System.IO.Directory.Exists(root))
            {
                System.IO.Directory.CreateDirectory(root);
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(root)
                ,
                RequestPath = new Microsoft.AspNetCore.Http.PathString()
            });
        }
        app.UseMvc(routes =>
        {
            routes.MapRoute("Path", "{controller}/{action}/{op}", new { action = "index", op = "" });
            routes.MapRoute("Home", "{action}", new { controller = "app" });
            routes.MapRoute("Root", "", new { controller = "app", action = "index" });
        });
    }
}