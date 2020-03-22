using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AAUBlocker
{
	public class Program
	{
		static ServiceProvider staticServices;
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddBaseAddressHttpClient();

			staticServices = builder.Services.BuildServiceProvider();
			await builder.Build().RunAsync();
		}
		/// <summary>
		/// Provide application-wide access to services
		/// Don't do this in Blazor server!
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetService<T>()
		{
			return staticServices.GetService<T>();
		}
	}
}
