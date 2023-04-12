using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Airline_Software
{
    public class CsvDatabase
    {
        public static List<T> ReadCsvFile<T>(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = new List<T>();
                records = csv.GetRecords<T>().ToList();
                return records;
            }
        }

        public static void WriteCsvFile<T>(string filePath, List<T> record)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, config))
            {
                if (typeof(T) == typeof(User))
                {
                    csv.Context.RegisterClassMap<UserMap>();
                }
                else if (typeof(T) == typeof(Customer))
                {
                    csv.Context.RegisterClassMap<CustomerMap>();
                }
                else if (typeof(T) == typeof(Airport))
                {
                    csv.Context.RegisterClassMap<AirportMap>();
                }
                else if (typeof(T) == typeof(Flight))
                {
                    csv.Context.RegisterClassMap<FlightMap>();
                }
                else if (typeof(T) == typeof(BoardingPass))
                {
                    csv.Context.RegisterClassMap<BoardingPassMap>();
                }
                else if (typeof(T) == typeof(Order))
                {
                    csv.Context.RegisterClassMap<OrderMap>();
                }
                else if (typeof(T) == typeof(Plane))
                {
                    csv.Context.RegisterClassMap<PlaneMap>();
                }

                csv.WriteRecords(record);
            }
        }

        public static void UpdateRecord<T>(List<T> records, Func<T, int> idSelector, int id, Action<T, T> update, T updatedRecord)
        {
            int indexToUpdate = records.FindIndex(record => idSelector(record) == id);
            if (indexToUpdate != -1)
            {
                update(records[indexToUpdate], updatedRecord);
            }
        }

        public static void RemoveRecord<T>(List<T> records, Func<T, int> idSelector, int id)
        {
            int indexToRemove = records.FindIndex(record => idSelector(record) == id);
            if (indexToRemove != -1)
            {
                records.RemoveAt(indexToRemove);
            }
        }

        public static T? FindRecord<T>(List<T> records, Func<T, int> idSelector, int id)
        {
            T? foundRecord = default;
            int indexToFind = records.FindIndex(record => idSelector(record) == id);
            if (indexToFind != -1)
            {
                foundRecord = records[indexToFind];
            }
            return foundRecord;
        }


        // EXAMPLE OF USE 
        // these use a class I made for testing called Person that only had ID, Age, and Name has properties. 
        /*
        // READING FROM DATABASE
        string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/PersonDb.csv"; // file path to 'table' (csv file) -- need to make it the relative path but i couldnt figure out how
        List<Person> people = CsvDatabase.ReadCsvFile<Person>(filePath); // read the file and store the data into a list

        // ADDING TO DATABASE
        // Person is the class of the record
        people.Add(new Person { Id = 12, Age = 23, Name = "Reece Pohlman33" });
        CsvDatabase.WriteCsvFile<Person>(filePath, people);
        
        // UPDATING DATABASE RECORD
        // people = list with read in file data | p => p.Id is telling to filter by Id | 1 is the Id of the record being replaced, | (current, updated) are required 
        Person updatedPerson = new Person { Name = "Reece 2.0", Age = 24 };
        CsvDatabase.UpdateRecord(people, p => p.Id, 1, (current, updated) =>
        {
            current.Name = updated.Name;
            current.Age = updated.Age;

        }, updatedPerson); /// dont forget to added the updatedPerson here !

        // also need to write changes back to the csv here
        CsvDatabase.WriteCsvFile<Person>(filePath, people);

        // DELETEING RECORD FROM DATABASE
        RemoveRecord(people, p => p.Id, 3);

        // FINDING FROM DATABASE
        FindRecord(people, p => p.Id, 3);
        */

    }
}
