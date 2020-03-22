using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace AAUBlocker
{
	public class JSHelper
	{
		public JSHelper()
		{
			// Don't do this in Blazor Server
			this.JS = Program.GetService<IJSRuntime>();
		}
		/// <summary>
		/// Sets up some JSInterop to use to disable all inputs.
		/// Uses a class 'disabled' - see site.css
		/// </summary>
		/// <returns></returns>
		public async Task Initialise() => 
			await JS.InvokeVoidAsync("eval", 
$@"window.blazorui = window.blazorui || {{
	block: function(className) {{
		document.body.classList.add(className || 'disabled')
	}},
	unblock: function(className) {{
		document.body.classList.remove(className || 'disabled')
	}}
}}");

		public IJSRuntime JS { get; }
	}
}
