using System;
using System.Collections.Generic;
using System.Linq;

namespace LevAstin_AirTek_test
{
    public class Configuration
    {
        public enum SourceType
        {
            Example, File
        }

        public SourceType FlightScheduleSourceType { get; set; }
        public string FlightScheduleSource { get; set; }
        public bool PrintSchedule { get; set; }

        public string OrdersSource { get; set; }
    }

    public class ConfigurationLoader
    {
        private string usage = @"Usage:
LevAstin_AirTek_test.exe -h
LevAstin_AirTek_test.exe [-ff path-to-flight-schedule-file] [-d] -o path-to-orders-file";
        public Configuration FromConsole(string[] args)
        {
            try
            {
                var config = new Configuration();
                config.FlightScheduleSourceType = Configuration.SourceType.Example;
                config.PrintSchedule = false;

                var argsQueue = new Queue<string>(args);
                while (argsQueue.Any())
                {
                    var arg = argsQueue.Dequeue();
                    switch (arg.ToLower())
                    {
                        case "-ff":
                            var source = argsQueue.Dequeue();
                            config.FlightScheduleSourceType = Configuration.SourceType.File;
                            config.FlightScheduleSource = source.ToLower();
                            break;
                        case "-h":
                            Console.WriteLine(usage);
                            Environment.Exit(0);
                            break;
                        case "-d":
                            config.PrintSchedule = true;
                            break;
                        case "-o":
                            config.OrdersSource = argsQueue.Dequeue();
                            break;
                        default:
                            throw new Exception();
                    }
                }

                if (string.IsNullOrWhiteSpace(config.OrdersSource))
                    throw new Exception();

                return config;
            }
            catch (Exception)
            {
                Console.WriteLine("Incorrect syntax");
                Console.WriteLine(usage);
                Environment.Exit(1);
                return null;
            }
        }
    }
}