using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Repositories;
using VisionTech_Anbar_Project.Services;
using VisionTech_Anbar_Project.Utilts;

namespace VisionTech_Anbar_Project
{
    internal static class Program
    {
        public static ServiceProvider ServiceProvider { get; private set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var baseDirectory = AppContext.BaseDirectory;
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..")))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var services = new ServiceCollection();

            // Configure services
            ConfigureServices(services, configuration);

            // Build the ServiceProvider
            ServiceProvider = services.BuildServiceProvider();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(Path.Combine(FileManager.GetLogPath(), "log-.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application Starting");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ServiceProvider.GetRequiredService<Ophrys>());

        }
        private static void ConfigureServices(ServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext using connection string from configuration

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging());
            services.AddScoped<AppDbContext>();
            
            services.AddTransient<PackageRepository>();
            services.AddTransient<ProductRepository>();
            services.AddTransient<CategoryRepository>();
            services.AddTransient<VendorRepository>();
            services.AddTransient<WarehouseRepository>();
            services.AddTransient<BarcodeRepository>();
            services.AddTransient<ImageRepository>();




            // Register other services like PackageService, etc.
            services.AddTransient<PackageService>();
            services.AddTransient<ProductService>();
            services.AddTransient<BarcodeService>();
            services.AddTransient<VendorService>();
            services.AddTransient<WarehouseService>();
            services.AddTransient<CategoryService>();
            services.AddTransient<ImageService>();

            
            services.AddSingleton<IConfiguration>(configuration);


            // Register the main form
            services.AddTransient<Ophrys>();
            services.AddTransient<AddColumnForm>();
            services.AddTransient<AddProductForm>();
            services.AddTransient<EditProductForm>();


        }
    }
}
