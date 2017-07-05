using System;
using System.CodeDom;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace AsyncBestPracticesZenWeb
{
	class Library
	{
		public static async Task FetchFileAsyncBad(int fileNum)
		{
			//Each call is going in bursts. if there is 4 logical cors then, this is CPU work so create 4 threads, so overtime it sees that I need more threads. The treads are wasting time.
			//Do not mess with the user app thread pool.
			await Task.Run(() =>
			{
				//wrapping sync operations with Task.Run inside a lib, you can prevent the app from managing its threads effectively.
				// Do not use Task.Run inside a lib. It is used to make things sync. except WinMD methods

				// Spinning up threads on scalable project hurts the performance.
				// This will make alot of garbage.
				var contents = IO.DownloadFile();
				Console.WriteLine($"Fetched file: {fileNum}: {contents}");
			});
		}

		public static async Task FetchFileAsync(int fileNum)
		{
			//this one thread will initiate the download all the download then handle them all when they come in.
			var contents = await IO.DownloadFileAsync();
			Console.WriteLine($"Fetched file: {fileNum}: {contents}");
		}

		//public static void ParalleDownload() - USE FOR CPU BOUND WORK
		//{
		//	Parallel.For(first, last + 1, i =>
		//	{
		//	make cors as many as required, as optimal. Threads = cors
		//	});
		//}

		public static void FetchFile(int fileNum)
		{
			//this creates a deadlock - do not expose sync method using async.
			FetchFileAsync(fileNum).Wait();
		}
	}
}
