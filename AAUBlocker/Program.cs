using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
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
		public static T GetService<T>()
		{
			return staticServices.GetService<T>();
		}
	}
	public class JSHelper
	{
		public JSHelper()
		{
			this.JS = Program.GetService<IJSRuntime>();
		}
//		public async Task Initialise() => await JS.InvokeVoidAsync("eval", "window.blazorui = window.blazorui || { block: function() {document.documentElement.style.pointerEvents=\"none\"},unblock: function() {document.documentElement.style.pointerEvents=\"auto\"}}");
		public async Task Initialise() => await JS.InvokeVoidAsync("eval", "window.blazorui = window.blazorui || { block: function() {document.body.classList.add('disabled')},unblock: function() {document.body.classList.remove('disabled')}}");

		public IJSRuntime JS { get; }
	}
	public static class MM
	{
		public static async Task<T> GetJsonAsyncUI<T>(this HttpClient client, string uri)
		{
			_ = uri.NullWhenEmpty() ?? throw new ArgumentNullException(nameof(uri));
			var js = new JSHelper();
			await js.Initialise();
			await js.JS.InvokeVoidAsync("blazorui.block");
			var result = await client.GetJsonAsync<T>(uri);
			await Task.Delay(1000);// just to make it obvious let's delay
			await js.JS.InvokeVoidAsync("blazorui.unblock");
			return result;
		}
		public static string NullWhenEmpty(this string value) => string.IsNullOrWhiteSpace(value) ? null : value;
	}
}
