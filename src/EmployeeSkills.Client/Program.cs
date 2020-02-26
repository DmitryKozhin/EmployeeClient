using System.IO;

using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

using EmployeeSkills.Client.ViewModels;

using Newtonsoft.Json;

using ReactiveUI;

namespace EmployeeSkills.Client
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance; // Interesting line of code that fixes the issue!
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
        }
    }
}
