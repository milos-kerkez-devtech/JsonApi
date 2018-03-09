using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sandbox.Notes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

		public static IWebHost BuildWebHost(string[] args) =>
		    WebHost.CreateDefaultBuilder(args)
			    .UseStartup<Startup>()
		        .Build();

		//public static IWebHost BuildWebHost(string[] args) =>
		//	new WebHostBuilder()
		//		.UseKestrel()
		//		.UseContentRoot(Directory.GetCurrentDirectory())
		//		.ConfigureAppConfiguration((hostingContext, config) => { /* setup config */  })
		//		.ConfigureLogging((hostingContext, logging) => { /* setup logging */  })
		//		.UseIISIntegration()
		//		.UseDefaultServiceProvider((context, options) => { /* setup the DI container to use */  })
		//		.ConfigureServices(services =>
		//		{
		//			services.AddTransient<IConfigureOptions<KestrelServerOptions>, KestrelServerOptionsSetup>();
		//		})
		//		.UseStartup<Startup>()
		//		.Build();

	}
}
