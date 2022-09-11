#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace LevAstin_AirTek_test
{
    public class Airport
    {
        public string Code { get; set; }
        public string City { get; set; }
    }

    public class FlightRoute
    {
        public FlightRoute(Airport source, Airport destination)
        {
            Source = source;
            Destination = destination;
        }

        public Airport Source { get; }
        public Airport Destination { get; }
    }

    public class ScheduledFlight
    {
        public FlightRoute FlightRoute { get; set; }
        public int Day { get; set; }
        public int Id { get; set; }
    }

    public class FlightSchedule
    {
        public List<ScheduledFlight> Flights { get; set; }
    }

    public static class KnownAirports // I assume in real life there is some kind of storage with all the airports, so this works as a substitution to it
    {
        private static readonly Dictionary<string, Airport> airports =
            new List<Airport>
            {
                new Airport { City = "Montreal", Code = "YUL" },
                new Airport { City = "Calgary", Code = "YYC" },
                new Airport { City = "Vancouver", Code = "YVR" },
                new Airport { City = "Toronto", Code = "YYZ" },
            }.ToDictionary(x => x.Code);

        public static Airport? Find(string code)
        {
            return airports.ContainsKey(code) ? airports[code] : null;
        }
    }

    public class Order
    {
        public string Name { get; set; }
        public Airport Source { get; set; }
        public Airport Destination { get; set; }
    }
}
