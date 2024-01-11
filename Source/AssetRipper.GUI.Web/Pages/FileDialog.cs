using AssetRipper.GUI.Web.Paths;

namespace AssetRipper.GUI.Web.Pages
{
	public class FileDialog : DefaultPage
	{
		public static FileDialog Instance { get; } = new();

		public override string? GetTitle() => Localization.MenuUITest;

		public override void WriteInnerContent(TextWriter writer)
		{
			new H1(writer).Close(GetTitle());
			if (GameFileLoader.IsLoaded)
			{
				using (new P(writer).End())
				{
					WritePicker(writer, "/Export", Localization.MenuExport, "btn btn-primary");
				}

				using (new P(writer).End())
				{
					WriteLink(writer, "/Reset", Localization.MenuFileReset, "btn btn-danger");
				}
			}
			else
			{
				using (new P(writer).End())
				{
					WritePicker(writer, "/LoadFile", Localization.MenuLoad, "btn btn-primary");
				}
			}
		}

		private static void WritePicker(TextWriter writer, string url, string name, string? @class = null)
		{
			using (new Form(writer).WithAction(url).WithMethod("post").End())
			{
				// It has to keep name as 'Path' for the server to read the text inside.
				new Input(writer).WithClass("form-control").WithType("file").WithName("Path").Close();
				new Input(writer).WithType("submit").WithClass(@class).WithValue(name.ToHtml()).Close();
				Console.WriteLine($"WritePicker url: {url} name: {name} class: {@class}");
			}
		}

		private static void WriteLink(TextWriter writer, string url, string name, string? @class = null)
		{
			using (new Form(writer).WithAction(url).WithMethod("post").End())
			{
				new Input(writer).WithType("submit").WithClass(@class).WithValue(name.ToHtml()).Close();
			}
		}
	}
}
