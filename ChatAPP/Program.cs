using ChatAPP;
using Microsoft.AspNetCore;

public partial class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseUrls("http://localhost:3000")
        .UseStartup<Startup>();
}