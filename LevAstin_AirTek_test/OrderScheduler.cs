using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LevAstin_AirTek_test;

public interface IOrderScheduler
{
    List<OrderFlight> ScheduleOrders(FlightSchedule flightSchedule, List<Order> orders);
}

public class OrderScheduler : IOrderScheduler
{
    private class FlightCapacity
    {
        public ScheduledFlight Flight { get; set; }
        public int Capacity { get; set; }
    }
    public List<OrderFlight> ScheduleOrders(FlightSchedule flightSchedule, List<Order> orders)
    {
        
        var orderFlights = new List<OrderFlight>();

        var flights = flightSchedule.Flights;
        var capacity = flights.Select(x=> new FlightCapacity { Flight = x, Capacity = 20}).ToList();
        foreach (var order in orders)
        {
            var flight = capacity.FirstOrDefault(x =>
                x.Flight.FlightRoute.Source == order.Source && x.Flight.FlightRoute.Destination == order.Destination &&
                x.Capacity > 0);
            if (flight != null)
                flight.Capacity--;
            orderFlights.Add(new OrderFlight{Order = order, Flight = flight?.Flight});
        }

        return orderFlights;
    }
}

public class OrderFlight
{
    public Order Order { get; set; }
    public ScheduledFlight Flight { get; set; }
}