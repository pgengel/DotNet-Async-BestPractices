using System.Threading;
using System.Threading.Tasks;

namespace AsyncBestPracticesZenWeb
{
	class IO
	{
		public static string DownloadFile()
		{
			Thread.Sleep(1000);
			return "File contents";
		}

		public static async Task<string> DownloadFileAsync()
		{
			await Task.Delay(1000).ConfigureAwait(false);//to help avoid deadlocks.
			//await Task.Delay(1000);
			return "async File contents";
		}
	}
}
