using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace LevAstin_AirTek_test
{
    public interface IFlightScheduleLoader
    {
        FlightSchedule LoadSchedule();
    }

    public class FlightScheduleExampleLoader : IFlightScheduleLoader
    {
        public FlightSchedule LoadSchedule()
        {
            return new FlightSchedule()
            {
                Flights = new List<ScheduledFlight>
                {
                    new ScheduledFlight{Day = 1, Id = 1, FlightRoute = new FlightRoute(KnownAirports.Find("YUL")!, KnownAirports.Find("YYZ")!)},
                    new ScheduledFlight{Day = 1, Id = 2, FlightRoute = new FlightRoute(KnownAirports.Find("YUL")!, KnownAirports.Find("YYC")!)},
                    new ScheduledFlight{Day = 1, Id = 3, FlightRoute = new FlightRoute(KnownAirports.Find("YUL")!, KnownAirports.Find("YVR")!)},
                    new ScheduledFlight{Day = 2, Id = 4, FlightRoute = new FlightRoute(KnownAirports.Find("YUL")!, KnownAirports.Find("YYZ")!)},
                    new ScheduledFlight{Day = 2, Id = 5, FlightRoute = new FlightRoute(KnownAirports.Find("YUL")!, KnownAirports.Find("YYC")!)},
                    new ScheduledFlight{Day = 2, Id = 6, FlightRoute = new FlightRoute(KnownAirports.Find("YUL")!, KnownAirports.Find("YVR")!)},
                }
            };
        }
    }

    public class FlightScheduleFileLoader : IFlightScheduleLoader
    {
        private readonly string source;

        private string fileFormat =
@"1     //day number
YUL YYZ //source - destination
YUL YYC
        //empty line
2       //don't have to be consecutive
YYC YYZ";

        public FlightScheduleFileLoader(Configuration config)
        {
            this.source = config.FlightScheduleSource;
        }

        private enum State
        {
            Day, Night
        }

        public FlightSchedule LoadSchedule()
        {
            try
            {
                var scheduleText = File.ReadAllLines(source);
                var state = State.Night;
                var day = 0;
                var id = 1;
                var schedule = new FlightSchedule
                {
                    Flights = new List<ScheduledFlight>()
                };

                foreach (var line in scheduleText)
                {
                    switch (state)
                    {
                        case State.Night:
                            day = int.Parse(line.Trim());
                            state = State.Day;
                            break;
                        case State.Day:
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                state = State.Night;
                            }
                            else
                            {
                                var sd = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                schedule.Flights.Add(new ScheduledFlight
                                    {
                                        Day = day,
                                        Id = id,
                                        FlightRoute = new FlightRoute(
                                            KnownAirports.Find(sd[0]) ?? new Airport { Code = sd[0] },
                                            KnownAirports.Find(sd[1]) ?? new Airport { Code = sd[1] })
                                    }
                                );
                                id++;
                            }
                            break;
                    }
                }

                return schedule;
            }
            catch (Exception)
            {
                throw new Exception("Incorrect file format. Expecting:\n" + fileFormat);
            }
        }
    }
}