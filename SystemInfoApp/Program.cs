using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SystemInfoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure(builder =>
                    {
                        builder.UseRouting();
                        builder.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGet("/", async context =>
                            {
                                var builder = new StringBuilder();
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                    builder.AppendLine("OS Architecture = Windows");
                                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                                    builder.AppendLine("OS Architecture = Linux");
                                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                                    builder.AppendLine("OS Architecture = OSX");
                                else
                                    builder.AppendLine("OS Architecture = Others");

                                builder.AppendLine($"OS Description = {RuntimeInformation.OSDescription}");

                                if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                                    builder.AppendLine("ProcessArchitecture = ARM");
                                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                                    builder.AppendLine("ProcessArchitecture = ARM64");
                                else if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                                    builder.AppendLine("ProcessArchitecture = X64");
                                else if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                                    builder.AppendLine("ProcessArchitecture = X86");
                                else
                                    builder.AppendLine("ProcessArchitecture = Others");

                                builder.AppendLine($"BasePath = {PlatformServices.Default.Application.ApplicationBasePath}");
                                builder.AppendLine($"AppName = {PlatformServices.Default.Application.ApplicationName}");
                                builder.AppendLine($"AppVersion = {PlatformServices.Default.Application.ApplicationVersion}");

                                var assemblyVersion = Assembly.GetEntryAssembly()
                                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                    .InformationalVersion;

                                builder.AppendLine($"AssemplyVersion = {assemblyVersion}");
                                builder.AppendLine($"RuntimeFramework = {PlatformServices.Default.Application.RuntimeFramework}");
                                builder.AppendLine($"FrameworkDescription = {RuntimeInformation.FrameworkDescription}");

                                await context.Response.WriteAsync(builder.ToString(), Encoding.UTF8);
                            });
                        });
                    });
                });
    }
}