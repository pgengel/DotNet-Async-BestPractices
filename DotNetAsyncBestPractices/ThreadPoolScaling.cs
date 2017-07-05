using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncBestPracticesZenWeb
{
	class ThreadPoolScaling
	{
		internal static void Run()
		{
			var f = new Form() {Width = 600, Height = 400};
			var b = new Button() {Text = "Run", Dock = DockStyle.Fill, Font = new Font ("Consolas", 18)};
			f.Controls.Add(b);

			b.Click += async delegate
			{
				var tasks = new List<Task>();

				for (int i = 0; i < 100; i++)// hit all the request init in parallel on one thread.
				{

					int fileNum = i;
					tasks.Add(Library.FetchFileAsync(fileNum));
					//Parallel.For
					//tasks.Add(Library.FetchFileAsyncBad(fileNum));
					//tasks.Add(Library.FetchFile(fileNum)); //deadlock created
				}

				await Task.WhenAll(tasks);
				Console.WriteLine("Fetch all the files");
			};

			f.ShowDialog();
		}
	}
}
