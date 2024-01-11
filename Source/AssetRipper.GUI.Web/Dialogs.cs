using AssetRipper.Web.Extensions;
using Microsoft.AspNetCore.Http;
using NativeFileDialogs.Net;
using System.Net.Mime;

namespace AssetRipper.GUI.Web;

internal static class Dialogs
{
	private static readonly object lockObject = new();

	public static class OpenFiles
	{
		public static async Task HandleGetRequest(HttpContext context)
		{
			context.Response.DisableCaching();
			var paths = await GetUserInputAsync();
			//Maybe do something else when user cancels the dialog?
			var result = Results.Json(paths ?? Array.Empty<string>(), AppJsonSerializerContext.Default.StringArray);
			await context.Response.WriteAsJsonAsync(paths ?? Array.Empty<string>());		}

		public static async Task<string[]> GetUserInputAsync(IDictionary<string, string>? filters = null, string? defaultPath = null)
		{
			NfdStatus status = NfdStatus.Cancelled; // Replace with the actual default or error status
			string[]? paths = null;

			try
			{
				// Assuming Nfd.OpenDialogMultiple is an asynchronous method that can be awaited
				// If it's not, you'll need to find a way to call it correctly in the context of ASP.NET Core
				status = Nfd.OpenDialogMultiple(out paths, filters, defaultPath);
			}
			catch (Exception ex)
			{
				// Handle any exceptions that might occur
				// Log the exception, set the status to an error, etc.
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
