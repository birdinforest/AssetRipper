using System.IO.Pipes;
using System.Text;

namespace AssetRipper.GUI.Web
{
	public class IpcClient
	{
		public async Task<string[]> SendOpenFileDialogRequest()
		{
			using (var client = new NamedPipeClientStream("PipeName"))
			{
				await client.ConnectAsync();
				Console.WriteLine($"SendOpenFileDialogRequest. client Connected: {client}");
				
				// Dispose the writer immediately after sending the request. This practice avoid
				// that the writer buffer flushes automatically when disposed, then it will results
				// in pipe access issue. Because the server is terminated
				// immediately after sending the response.
				var writer = new StreamWriter(client, Encoding.UTF8);
				await writer.WriteLineAsync("OpenFileDialog");
				Console.WriteLine($"SendOpenFileDialogRequest. sent command 'OpenFileDialog'");
				await writer.FlushAsync();

				// Read the response (selected file paths) from the separate process
				var reader = new StreamReader(client, Encoding.UTF8);
				string? response = await reader.ReadLineAsync();
				Console.WriteLine($"SendOpenFileDialogRequest. received response: {response}");
				
				await writer.DisposeAsync();
				reader.Dispose();
				
				if (response != null)
				{
				 return new[] {response};
				}
				else
				{
				 return new string[] { };
				}
			}
		}
	}
}
