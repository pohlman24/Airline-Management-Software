﻿using System;
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

        public static double CalcPercentCapacity(Flight flight)
        {
            int cap = flight.Capacity;
            if(cap == 0)
            {
                return 0;
            }
            int sold = flight.SeatsSold;

            double percentCap = (double) sold / cap;
            return percentCap;
        }

        public static double CalcIncomeFlight(Flight flight)
        {
            int seats = flight.SeatsSold;
            double cost = flight.Price;
            double income = (double) seats * cost;
            return Math.Round(income,2);
        }

        public static double CalcIncomeWhole()
        {
            double totalIncome = 0;
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            foreach (Flight flight in flights)
            {
                int seats = flight.SeatsSold;
                double cost = flight.Price;
                double income = (double) seats * cost;
                totalIncome += income;
            }
            return Math.Round(totalIncome, 2);
        }

        public static int CalcNumFlights(string range)
        {
            // duration options?
            // day, month, year, --- a specific range? 
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            int count = 0;

            // fix output formatting!! DG
            
            foreach (Flight flight in flights)
            {
                if(range == "DAY")
                {
                    if (flight.DepartureTime.Day == DateTime.Today.Day)
                    {
                        
                        count++;
                    }
                    
                }
                else if(range == "WEEK")
                {
                    if (GetWeekOfYear(flight.DepartureTime) == GetWeekOfYear(DateTime.Today)
                        && flight.DepartureTime.Year == DateTime.Today.Year)
                    {
                        
                        count++;
                    }
                }
                else if ( range == "MONTH")
                {
                    if (flight.DepartureTime.Month == DateTime.Today.Month)
                    {
                        count++;
                    }

                }

                else if(range == "YEAR")
                {
                    if(flight.DepartureTime.Year == DateTime.Today.Year)
                    {
                        count++;
                    }
                }
          
                else
                {
                    Console.WriteLine("\nInvalid Range");
                    return -1;
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
