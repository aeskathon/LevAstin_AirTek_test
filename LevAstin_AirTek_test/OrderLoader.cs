using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LevAstin_AirTek_test
{
    public interface IOrderLoader
    {
        List<Order> LoadOrders();
    }

    public class OrderLoader : IOrderLoader
    {
        private readonly string source;
        public OrderLoader(Configuration configuration)
        {
            source = configuration.OrdersSource;
        }

        // Strictly speaking if you plan to have orders listed in priority order, you should use an array, not an object
        // Otherwise we would need to write our own parser instead of deserializing the whole file, because fields of an object is an unordered set
        // I implemented it in this way, assuming that the number at the end of order name (order-XXX) can also be used as a priority marker
        public List<Order> LoadOrders()
        {
            var orders = new List<Order>();
            var text = File.ReadAllText(source);
            var deserializedOrders = JsonConvert.DeserializeObject<Dictionary<string, OrderImportModel>>(text);

            return deserializedOrders.OrderBy(x => x.Key).Select(pair => new Order
            {
                Source = KnownAirports.Find("YUL"), // Looks like it is always a starting point in this test task. In real life I would expect to have it stated explicitly for each order
                Destination = KnownAirports.Find(pair.Value.Destination) ??
                              new Airport { Code = pair.Value.Destination },
                Name = pair.Key
            }).ToList();
        }

        private class OrderImportModel
        {
            public string Destination { get; set; }
        }


    }
}
