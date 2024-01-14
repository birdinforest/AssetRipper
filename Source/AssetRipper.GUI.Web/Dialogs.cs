using AssetRipper.Web.Extensions;
using Microsoft.AspNetCore.Http;
using NativeFileDialogs.Net;
using System.Diagnostics;
using System.Net.Mime;

namespace AssetRipper.GUI.Web;

internal static class Dialogs
{
	private static readonly object lockObject = new();
	
	private static void StartFileDialogServerProcess()
	{
		string serverExecutablePath = @"../AssetRipper.GUI.FileDialog/bin/Debug/AssetRipper.GUI.FileDialog";

		try
		{
			Process serverProcess = new Process();
			serverProcess.StartInfo.FileName = serverExecutablePath;
			serverProcess.StartInfo.CreateNoWindow = true;
			serverProcess.StartInfo.UseShellExecute = false;
			serverProcess.Start();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error starting server process: {ex.Message}");
		}
	}

	public static class OpenFiles
	{
		public static async Task HandleGetRequest(HttpContext context)
		{
			context.Response.DisableCaching();
			var paths = await GetUserInputAsync();
			//Maybe do something else when user cancels the dialog?
			var result = Results.Json(paths ?? Array.Empty<string>(), AppJsonSerializerContext.Default.StringArray);
			await context.Response.WriteAsJsonAsync(paths ?? Array.Empty<string>());
		}

		public static async Task<string[]> GetUserInputAsync(IDictionary<string, string>? filters = null, string? defaultPath = null)
		{
			StartFileDialogServerProcess();
			var ipcClient = new IpcClient();
			Console.WriteLine($"GetUserInputAsync establish ipcClient {ipcClient}");
			string[]? paths = await ipcClient.SendOpenFileDialogRequest();
			if (paths is {Length: > 0})
			{
				Console.WriteLine($"GetUserInputAsync get paths. Length: {paths.Length} Paths: {paths[0]}");
			}
			else
			{
				Console.WriteLine($"GetUserInputAsync get empty paths.");
			}
			return paths;
		}
	}

	public static class OpenFile
	{
		public static Task HandleGetRequest(HttpContext context)
		{
			context.Response.DisableCaching();
			NfdStatus status = GetUserInput(out string? path);
			//Maybe do something else when user cancels the dialog?
			return Results.Json(path ?? "", AppJsonSerializerContext.Default.String).ExecuteAsync(context);
		}

		public static NfdStatus GetUserInput(out string? path, IDictionary<string, string>? filters = null, string? defaultPath = null)
		{
			// lock (lockObject)
			// {
				
				return Nfd.OpenDialog(out path, filters, defaultPath);
			// }
		}
	}

	public static class OpenFolder
	{
		public static Task HandleGetRequest(HttpContext context)
		{
			context.Response.DisableCaching();
			NfdStatus status = GetUserInput(out string? path);
			//Maybe do something else when user cancels the dialog?
			return Results.Json(path ?? "", AppJsonSerializerContext.Default.String).ExecuteAsync(context);
		}

		public static NfdStatus GetUserInput(out string? path, string? defaultPath = null)
		{
			lock (lockObject)
			{
				return Nfd.PickFolder(out path, defaultPath);
			}
		}
	}

	public static class SaveFile
	{
		public static Task HandleGetRequest(HttpContext context)
		{
			context.Response.DisableCaching();
			NfdStatus status = GetUserInput(out string? path);
			//Maybe do something else when user cancels the dialog?
			return Results.Json(path ?? "", AppJsonSerializerContext.Default.String).ExecuteAsync(context);
		}

		public static NfdStatus GetUserInput(out string? path, IDictionary<string, string>? filters = null, string defaultName = "Untitled", string? defaultPath = null)
		{
			lock (lockObject)
			{
				return Nfd.SaveDialog(out path, filters, defaultName, defaultPath);
			}
		}
	}
}
