using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBestPracticesZenWeb
{

	class SyncCode
	{
		public int SumPageSizes(IList<Uri> uris)
		{
			int total = 0;
			foreach (var uri in uris)
			{
				var data = new WebClient().DownloadData(uri);//dont know how long this will take.
				total += data.Length;
			}

			return total;
		}
	}
	class AsyncCode
	{
		public async Task<int> SumPageSizesAsync(IList<Uri> uris)
		{
			//var token = ClientDosconnectedToken();
			int total = 0;
			foreach (var uri in uris)
			{
				var data = await new HttpClient().GetStringAsync(uri);//thx waiter go do something else ill call you when im ready.
				total += data.Length;
			}

			return total;
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			ThreadPoolScaling.Run();
			//CapturingContext.Run();
			//FlowingContext.Run();
		}
	}
}
