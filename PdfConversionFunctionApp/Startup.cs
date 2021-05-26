using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

[assembly: FunctionsStartup(typeof(PdfConversionFunctionApp.Startup))]
namespace PdfConversionFunctionApp
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string path = fileInfo.Directory.Parent.FullName;
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .SetBasePath(path)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var apiConfig = new ApiConfig();
            config.Bind(nameof(ApiConfig), apiConfig);

            builder.Services.AddSingleton<FileService>();
            builder.Services.AddSingleton(apiConfig);
        }
    }
}
