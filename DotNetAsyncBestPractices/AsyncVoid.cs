using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncBestPracticesZenWeb
{
	//Fire and forget scenario - this is situation which is not.
	// do not return void, unexpected behaviour. Goinf to miss the try catch.
	// We have two tasks running, we do not know which one will complete first.
	// This is a race condition. Should treturn task
	// The second prob is that we can miss the try catch. The global catch picks it up - crash or the error will swallow up.
	class AsyncVoid
	{
		static string m_GetReponse;
		internal static void Run()
		{
			var f = new Form() { Width = 600, Height = 400 };
			var b = new Button() { Text = "Run", Dock = DockStyle.Fill, Font = new Font("Consolas", 18) };
			f.Controls.Add(b);

			b.Click += async delegate
			{
				try
				{
					//await SendDataAsync("");
					SendData("https://google.com");
					await Task.Delay(2000);
					Console.WriteLine("reveived data");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}

				////Example2
				//try
				//{
				//	await Dispatcher.RunAsync(CoreDispatchPriority.Normal, async () =>
				//	{
				//		await LoadAsync(); //If there is an error, the try catch will not happen,  the dispatcher is a void.
				//		m_Result = "done";
				//		throw new Exception();
				//	});
				//}
				//catch (Exception e)
				//{
				//	Console.WriteLine(e);
				//	throw;
				//}
			};

			f.ShowDialog();
		}

		//private static async Task SendDataAsync(string Url)
		private static async void SendData(string Url)
		{
			var request = WebRequest.Create(Url);
			using (var response = await request.GetResponseAsync())
			using (var stream = new StreamReader(response.GetResponseStream()))
				m_GetReponse = stream.ReadToEnd();
		}

		// CAn;t use await in constructor - use the factory method instead
		// Can;t use await in property getters - make it a method  instead
		// async event handler are reentering - disable UI.
	}

	// Action a1 = async () => { await LoadAsync(); m_Result="done"; };
	//Func<Task> a2 = async () => { await LoadAsync(); m_Result = "done"; };

	//Q If bothe overload are offered, it will pick Task-returning. Good!

	//Guidence
	//1. Use asunc for top level evetn handler.
	//2. Return a Task
	//3. Explicitly say FredAsync().FireAndForget() when using a lambda
}
