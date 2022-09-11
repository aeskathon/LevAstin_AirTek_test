using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevAstin_AirTek_test
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationLoader = new ConfigurationLoader();
            var config = configurationLoader.FromConsole(args);

            IFlightScheduleLoader scheduleLoader = null;

            if (config.FlightScheduleSourceType == Configuration.SourceType.File)
                scheduleLoader = new FlightScheduleFileLoader(config);
            else if (config.FlightScheduleSourceType == Configuration.SourceType.Example)
                scheduleLoader = new FlightScheduleExampleLoader();

            var schedule = scheduleLoader.LoadSchedule();

            if (config.PrintSchedule)
                Console.WriteLine(schedule.ToConsoleString());

            IOrderLoader orderLoader = new OrderLoader(config);
            var orders = orderLoader.LoadOrders();

            IOrderScheduler orderScheduler = new OrderScheduler();
            var orderFlights = orderScheduler.ScheduleOrders(schedule, orders);

            foreach (var orderFlight in orderFlights)
            {
                Console.WriteLine(orderFlight.ToConsoleString());
            }

        }
    }

}
