using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace AmdLinkSteamWrapper
{
    class Program
    {
        static IConfiguration config;

        static void Main(string[] args)
        {

            config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            ConfigFile _file = config.Get<ConfigFile>();

            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = _file.SteamPath,
                WorkingDirectory = Path.GetDirectoryName(_file.SteamPath),
                Arguments = _file.SteamArg
            };

            List<Process> steamProcess = Process.GetProcesses().Where(_ => { try { return Path.GetFileName(_.MainModule.FileName).ToLower() == Path.GetFileName(_file.SteamPath).ToLower(); } catch { return false; } }).ToList();

            // Kill old steam process
            foreach (var ligne in steamProcess)
            {
                try
                {
                    ligne.Kill();
                    Console.WriteLine($"Info : Killed PID {ligne.Id}");
                    Thread.Sleep(250);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failure killing old process : {e.Message}");
                }
            }

            //

            Process.Start(info).WaitForExit();



        }
    }

    public class ConfigFile
    {

        public string SteamPath { get; set; }

        public string SteamArg { get; set; }
    }
}
