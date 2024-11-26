using Serilog;
using VisionTech_Anbar_Project.Utilts;

namespace VisionTech_Anbar_Project
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(Path.Combine(FileManager.GetLogPath(), "log-.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application Starting");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new CategroyTest());
        }
    }
}