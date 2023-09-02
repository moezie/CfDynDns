using CfDynDns;
using System.Net.Security;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configurationBuilder) =>
     {  if(!context.HostingEnvironment.IsDevelopment())
            configurationBuilder.SetBasePath("/data").AddJsonFile("appsettings.json", false);
     })
    .ConfigureServices((hostConfig, services) =>
    {
        services.AddHostedService<Worker>();
        services.Configure<CloudflareConfig>(hostConfig.Configuration.GetSection(CloudflareConfig.Cloudflare));
    })
    .Build();

await host.RunAsync();
