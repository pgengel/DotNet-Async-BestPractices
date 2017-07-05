using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBestPracticesZenWeb
{
	class FlowingContext
	{
		private const int ITERS = 100000;

		private static async Task DoWorkAsync()
		{
			for (int i = 0; i < ITERS; i++)
			{
				await Task.Yield();
			}
		}

		internal static void Run()
		{
			var sw = new Stopwatch();
			while (true)
			{
				CallContext.LogicalSetData("Foo", "Bar");
				sw.Restart();
				DoWorkAsync().Wait();
				var withTime = sw.Elapsed;
				CallContext.FreeNamedDataSlot("Foo");

				sw.Restart();
				DoWorkAsync().Wait();
				var withOutTime = sw.Elapsed;

				Console.WriteLine("With     : {0}", withTime);
				Console.WriteLine("Without  : {0}", withOutTime);
				Console.ReadLine();
			}
		}
	}
}
