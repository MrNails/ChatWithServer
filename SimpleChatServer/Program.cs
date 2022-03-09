using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SimpleChatServer.DAL;
using SimpleChatServer.Models;
using SimpleChatServer.Services;
using SimpleChatServer.Services.Exceptions;
using SimpleChatServer.Services.Handlers;
using SimpleChatServer.Services.StaticMappableServices;

namespace SimpleChatServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ActionHandler.AddMappableService(typeof(ChatManager));
            ActionHandler.AddMappableService(typeof(UserManager));
            
            LoadSettings();
            ConfigureLogger();

            MasterDataContext dataContext =
                new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var loginParam = new SqlParameter("@login", "test");
            var pwdParam = new SqlParameter("@password", "12345678");
            // var userIdParam = new SqlParameter("@userId", "12345678") {Direction = ParameterDirection.Output};

            // dataContext.Database.ExecuteSqlRaw("exec MasterData.dbo.proc_CreateUser @login, @password, @userId out",
            //     new object[] { loginParam, pwdParam, userIdParam });

            var user = dataContext.Users.FromSqlRaw("SELECT * FROM [MasterData].[dbo].[func_FindUserData] ('tes55','qwerty1234')").ToList();

            Console.WriteLine($"Id: {user[0].Id}\nLogin: {user[0].Login}\nCreated: {user[0].Created}\nModified: {user[0].Modified}");
            
            var awaitable = Server.Start().ConfigureAwait(false);
            
            string action = string.Empty;
            bool isExit = false;

            while (!isExit)
            {
                Console.WriteLine("Enter type of action: e - exit.");
                action = Console.ReadLine()!;
                
                switch (action.ToLower())
                {
                    case "e":
                        isExit = true;
                        try
                        {
                            Server.RequestStop();

                            while (Server.IsActive)
                            {
                                Console.WriteLine("Attempt to stop server...");
                                System.Threading.Thread.Sleep(500);
                            }
                            
                            //Get exception if it exists
                            awaitable.GetAwaiter().GetResult();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            
                            // Log.Logger.Error(e, "");
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong action.");
                        break;
                }
            }

            Console.WriteLine("Program main end.");
        }

        private static void ConfigureLogger()
        {
            var logConfiguration = new LoggerConfiguration();
            
            Log.Logger = logConfiguration.WriteTo.File(System.IO.Path.Combine(Environment.CurrentDirectory, "log.txt"),
                                                       outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}",
                                                       restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                                                 .CreateLogger();   
        }

        private static bool LoadSettings()
        {
            var fileSettingsPath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");

            if (!File.Exists(fileSettingsPath))
                throw new ServerConfiguringException("Cannot find settings file. Create settings file than restart server.");

            var jObj = JsonObject.Parse(File.ReadAllText(fileSettingsPath));

            if (jObj == null)
                throw new ServerConfiguringException("Configuration file is corrupted.");
            
            LoadConnectionString(jObj["ConnectionStrings"]?.AsObject());
            LoadJWTConfiguration(jObj["JWTConfiguration"]?.AsObject());
            LoadAuthOptions(jObj["AuthenticationOptions"]?.AsObject());


            return true;
        }

        private static void LoadJWTConfiguration(JsonObject? jObj)
        {
            if (jObj == null) 
                throw new ServerConfiguringException("Cannot find jwt configuration in file.");

            var issuer = jObj["Issuer"]?.GetValue<string>();
            var audience = jObj["Audience"]?.GetValue<string>();
            var key = jObj["Key"]?.GetValue<string>();
            
            if (issuer == null || audience == null || key == null)
                throw new ServerConfiguringException("One of jwt configuration properties are missing.");
            
            GlobalSettings.JwtSettings = new JWTSettings(issuer, audience, key);
        }

        private static void LoadConnectionString(JsonObject? jObj)
        {
            if (jObj == null) 
                throw new ServerConfiguringException("Cannot find connection strings in configuration file.");

            foreach (var connectionString in jObj)
                GlobalSettings.ConnectionStrings[connectionString.Key] = connectionString.Value.GetValue<string>();
            
            if (GlobalSettings.ConnectionStrings.Count == 0) 
                throw new ServerConfiguringException("Cannot find connection strings in configuration file.");
        }
        
        private static void LoadAuthOptions(JsonObject? jObj)
        {
            if (jObj == null) 
                throw new ServerConfiguringException("Cannot find authentication in configuration file.");

            var lifetime = jObj["Lifetime"].GetValue<int>();
            
            GlobalSettings.AuthOptions = new AuthenticationOptions(lifetime);
        }
    }
}