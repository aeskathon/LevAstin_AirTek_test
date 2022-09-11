using System.Linq;
using System.Text;

namespace LevAstin_AirTek_test
{
    public static class Extensions
    {
        public static string ToConsoleString(this FlightSchedule schedule)
        {
            var sb = new StringBuilder();
            foreach (var flight in schedule.Flights.OrderBy(x => x.Id))
            {
                sb.AppendLine($"Flight: {flight.Id}, departure: {flight.FlightRoute.Source.Code}, arrival: {flight.FlightRoute.Destination.Code}, day: {flight.Day}");
            }
            return sb.ToString();
        }

        public static string ToConsoleString(this OrderFlight orderFlight)
        {
            if (orderFlight.Flight != null)
            {
                var sourceCity = orderFlight.Flight.FlightRoute.Source.City ??
                                 orderFlight.Flight.FlightRoute.Source.Code;
                var destinationCity = orderFlight.Flight.FlightRoute.Destination.City ??
                                      orderFlight.Flight.FlightRoute.Destination.Code;
                return
                    $"order: {orderFlight.Order.Name}, flightNumber: {orderFlight.Flight.Id}, departure: {sourceCity}, arrival: {destinationCity}, day: {orderFlight.Flight.Day}";
            }
            else
            {
                return $"order: {orderFlight.Order.Name}, flightNumber: not scheduled";
            }
        }
    }
}