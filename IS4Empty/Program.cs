// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IS4Empty.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Security.Claims;

namespace IS4Empty
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                Log.Information("Starting host...");
                var host = CreateHostBuilder(args).Build();
                
                // Create users at start
                using (var scope = host.Services.CreateScope())
                {
                    var userManager = scope.ServiceProvider
                        .GetRequiredService<UserManager<ApplicationUser>>();

                    var user = new ApplicationUser { UserName = "gaben" };
                    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user, new Claim( "Role", "Admin" )).GetAwaiter().GetResult();

                    var user2 = new ApplicationUser { UserName = "user" };
                    userManager.CreateAsync(user2, "password").GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user2, new Claim( "Role", "User" )).GetAwaiter().GetResult();
                }
                
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}