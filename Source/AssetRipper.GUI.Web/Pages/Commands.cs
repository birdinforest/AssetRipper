using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;

namespace AssetRipper.GUI.Web.Pages;

public static class Commands
{
	public readonly struct LoadFile : ICommand
	{
		static async Task ICommand.Start(HttpRequest request)
		{
			IFormCollection form = await request.ReadFormAsync();

			string[]? paths = new string[] { };
			if (form.TryGetValue("Path", out StringValues values))
			{
				paths = values;
			}
			else
			{
				paths = await Dialogs.OpenFiles.GetUserInputAsync();
			}
			
			Console.WriteLine($"LoadFile. values: {values} paths.Length: {paths.Length} paths: {paths[0]}");

			if (paths is { Length: > 0 })
			{
				GameFileLoader.LoadAndProcess(paths);
			}
		}
	}

	public readonly struct LoadFolder : ICommand
	{
		static async Task ICommand.Start(HttpRequest request)
		{
			Console.WriteLine($"LoadFolder.");
			IFormCollection form = await request.ReadFormAsync();

			string? path;
			if (form.TryGetValue("Path", out StringValues values))
			{
				path = values;
			}
			else
			{
				Dialogs.OpenFolder.GetUserInput(out path);
			}

			if (!string.IsNullOrEmpty(path))
			{
				GameFileLoader.LoadAndProcess([path]);
			}
		}
	}

	public readonly struct Export : ICommand
	{
		static async Task ICommand.Start(HttpRequest request)
		{
			Console.WriteLine($"Export.");
			IFormCollection form = await request.ReadFormAsync();

			string? path;
			if (form.TryGetValue("Path", out StringValues values))
			{
				path = values;
			}
			else
			{
				Dialogs.OpenFolder.GetUserInput(out path);
			}

			if (!string.IsNullOrEmpty(path))
			{
				GameFileLoader.Export(path);
			}
		}
	}

	public readonly struct Reset : ICommand
	{
		static Task ICommand.Start(HttpRequest request)
		{
			Console.WriteLine($"Reset.");
			GameFileLoader.Reset();
			return Task.CompletedTask;
		}
	}
	
	public readonly struct UITest : ICommand
	{
		static async Task ICommand.Start(HttpRequest request)
		{
			Console.WriteLine($"UITest.");
			IFormCollection form = await request.ReadFormAsync();
			Console.WriteLine("UITest clicked");
		}
	}

	public static Task HandleCommand<T>(HttpContext context) where T : ICommand
	{
		Console.WriteLine($"HandleCommand context:{context}");
		context.Response.Redirect(T.RedirectionTarget);
		return T.Start(context.Request);
	}
}
