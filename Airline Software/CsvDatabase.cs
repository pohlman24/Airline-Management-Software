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



        // EXAMPLE OF USE 
        /*
        string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/PersonDb.csv"; // file path to 'table' (csv file)
        List<Person> people = CsvDatabase.ReadCsvFile<Person>(filePath); // read the file and store the data into a list

        // Adding a new person
        people.Add(new Person { Id = 12, Age = 23, Name = "Reece Pohlman33" });
         CsvDatabase.WriteCsvFile<Person>(filePath, people);
        
        // Updating a person
        // people = list with read in file data | p => p.Id is telling to filter by Id | 1 is the Id of the record being replaced, | (current, updated) are required 
        Person updatedPerson = new Person { Name = "Reece 2.0", Age = 24 };
        CsvDatabase.UpdateRecord(people, p => p.Id, 1, (current, updated) =>
        {
            current.Name = updatedPerson.Name;
            current.Age = updatedPerson.Age;

        }, updatedPerson); /// dont forget to added the updatedPerson here !
        // also need to write changes back to the csv here
        CsvDatabase.WriteCsvFile<Person>(filePath, people);
        */

    }
}
