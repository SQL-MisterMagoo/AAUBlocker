using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AAUBlocker
{
	public static class Extensions
	{
		static JSHelper jsHelper;

		/// <summary>
		/// Provides a specific extension method for calling an api and disabling inputs for the duration of the call
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="client"></param>
		/// <param name="uri"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static async Task<T> GetJsonAsync<T>(this HttpClient client, string uri, bool DisableInputs, string className="disabled", int MockDelay=0)
		{
			_ = uri.NullWhenEmpty() ?? throw new ArgumentNullException(nameof(uri));

			if (DisableInputs)
			{
				if (jsHelper is null)
				{
					jsHelper = new JSHelper();
					await jsHelper.Initialise();
				}
				await jsHelper.JS.InvokeVoidAsync("blazorui.block", className);
				await Task.Delay(MockDelay);// just to make it obvious let's delay by a second
				var result = await client.GetJsonAsync<T>(uri);
				await jsHelper.JS.InvokeVoidAsync("blazorui.unblock", className);
				return result;
			} 
			else
			{
				await Task.Delay(MockDelay);// just to make it obvious let's delay by a second
				return await client.GetJsonAsync<T>(uri);
			}
		}

		public static string NullWhenEmpty(this string value) => string.IsNullOrWhiteSpace(value) ? null : value;
	}
}
