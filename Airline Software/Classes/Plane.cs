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
        public Plane(string Model, int Capacity, int PlaneId)
        {
            this.Model = Model;
            this.Capacity = Capacity;
            this.PlaneId = PlaneId;
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

        public static void UpdatePlane(Plane plane, string model = "", int capacity = -1)
        {
            plane.Model = string.IsNullOrEmpty(model) ? plane.Model : model;
            plane.Capacity = capacity == -1 ? plane.Capacity : capacity;

            string filePath = @"..\..\..\Tables\PlaneDb.csv";
            List<Plane> planes = CsvDatabase.ReadCsvFile<Plane>(filePath);

            CsvDatabase.UpdateRecord(planes, p => p.PlaneId, plane.PlaneId, (current, updated) =>
            {
                current.Model = updated.Model;
                current.Capacity = updated.Capacity;
            }, plane);

            CsvDatabase.WriteCsvFile(filePath, planes);
        }

        public static void DeletePlane(Plane plane)
        {
            string filePath = @"..\..\..\Tables\PlaneDb.csv";
            List<Plane> planes = CsvDatabase.ReadCsvFile<Plane>(filePath);
            CsvDatabase.RemoveRecord(planes, p => p.PlaneId, plane.PlaneId);
            CsvDatabase.WriteCsvFile(filePath, planes);
        }

        public static Plane FindPlaneById(int id)
        {
            string filePath = @"..\..\..\Tables\PlaneDb.csv";
            List<Plane> planes = CsvDatabase.ReadCsvFile<Plane>(filePath);
            if (CsvDatabase.FindRecord(planes, p => p.PlaneId, id) == null)
            {
                throw (new Exception("Id Not Found"));
                return null;
            }
            Plane plane = CsvDatabase.FindRecord(planes, p => p.PlaneId, id);
            return plane;
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
