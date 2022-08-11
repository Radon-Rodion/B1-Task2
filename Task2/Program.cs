using React.AspNet;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Task2.Data;
using Task2.Services;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File("Logs\\Info.log"))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File("Logs\\Warning.log"))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File("Logs\\Error.log"))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File("Logs\\Fatal.log"))
            .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;

    services.AddMemoryCache();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddSingleton<IFilesProcessor, ExelFileProcessor>();
    services.AddReact();
    services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();

    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connection));
    services.AddControllersWithViews();

    var app = builder.Build();

    /*using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }*/
    app.UseDeveloperExceptionPage();
    app.UseReact(config => { });
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}