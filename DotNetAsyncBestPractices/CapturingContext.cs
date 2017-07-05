using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncBestPracticesZenWeb
{
	class CapturingContext
	{
		const int ITERS = 20000;

		private static async Task WithContext()
		{
			for (int i = 0; i < ITERS; i++)
			{
				var t = Task.Run((() => { }));
				await t;
			}
		}

		private static async Task WithOutContext()
		{
			for (int i = 0; i < ITERS; i++)
			{
				var t = Task.Run((() => { }));
				await t.ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		internal static void Run()
		{
			var f = new Form(){Width = 600, Height = 400};
			var b = new Button(){Text = "Run", Dock = DockStyle.Fill, Font = new Font("Consolas", 18)};
			f.Controls.Add(b);

			b.Click += async delegate
			{
				b.Text = ".... Running ....";
				await Task.WhenAll(WithContext(), WithOutContext());

				var sw = new Stopwatch();

				sw.Restart();
				await WithContext();
				var withTime = sw.Elapsed;

				sw.Reset();
				await WithOutContext();
				var withoutTime = sw.Elapsed;

				b.Text = string.Format("With     : {0}\nWithout:  {1}\n\nDiff       : {2:F2}x", 
				withTime, withoutTime, withTime.TotalSeconds / withoutTime.TotalSeconds);

			};

			f.ShowDialog();
		}
	}
}
