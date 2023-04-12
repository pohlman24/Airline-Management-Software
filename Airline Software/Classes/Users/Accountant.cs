using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Accountant : User
    {
        public Accountant(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {
        }

        public static int CalcPercentCapacity(Flight flight)
        {

            return 1;
        }

        public static int CalcNumFlights(string range)
        {
            // duration options?
            // day, month, year, --- a specific range? 
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            int count= 0;
            foreach (Flight flight in flights)
            {
                if (flight.DepartureTime.Day == DateTime.Today.Day && range == "day")
                {
                    Console.WriteLine(flight.DepartureTime);
                    count++;
                }
                else if (GetWeekOfYear(flight.DepartureTime) == GetWeekOfYear(DateTime.Today) 
                        && flight.DepartureTime.Year == DateTime.Today.Year
                        && range == "week")
                {
                    Console.WriteLine(flight.DepartureTime);
                    count++;
                }
                else if(flight.DepartureTime.Month == DateTime.Today.Month && range == "month")
                {
                    Console.WriteLine(flight.DepartureTime);
                    count++;
                }
                else if (flight.DepartureTime.Year == DateTime.Today.Year&& range == "year")
                {
                    Console.WriteLine(flight.DepartureTime);
                    count++;
                }
                else
                {
                    Console.WriteLine("Invalid Range");
                }
            }
            return count;
        }

        // helper function for CalcNumFlights
        public static int GetWeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
