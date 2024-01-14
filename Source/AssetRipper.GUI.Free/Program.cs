using AssetRipper.GUI.Web;
using System.IO.Pipes;
using System.Text;
using NativeFileDialogs.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AssetRipper.GUI.Free;

public static class Program
{
    public static async Task Main() // Note the async Task
    {
        WebApplicationLauncher.Launch(); // Continue with other operations
    }
}
