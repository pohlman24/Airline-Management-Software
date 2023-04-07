using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Plane
    {
        public string Model { get; set; }
        public int Capacity { get; set; }
        public int PlaneId { get; set; }


        // if we need a database for this than heres the code for it 
        public Plane(string model, int capacity, int planeId)
        {
            Model = model;
            Capacity = capacity;
            PlaneId = planeId;
        }
        public static Plane CreatePlane(string model, int capacity)
        {
            string filePath = @"..\..\..\Tables\PlaneDb.csv"; ;
            int planeId = GeneratePlaneID();
            Plane newPlane = new Plane(model, capacity, planeId);
            List<Plane> planes = CsvDatabase.ReadCsvFile<Plane>(filePath);
            planes.Add(newPlane);
            CsvDatabase.WriteCsvFile<Plane>(filePath, planes);
            return newPlane;
        }

        private static int GeneratePlaneID()
        {
            string filePath = @"..\..\..\Tables\PlaneDb.csv"; ;
            List<Plane> planes = CsvDatabase.ReadCsvFile<Plane>(filePath);
            int maxID = planes.Count > 0 ? planes.Max(p => p.PlaneId) : 0;
            return maxID + 1;
        }

    }
}
