using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class FlightManager : Admin
    {
        public FlightManager(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {

        }
        /*public FlightManager()
        {
            //position for flight manager is 3
            setID(3);
        }
*/
        //print list of all passengers on a specific flight
        /*public void printFlightManifest(Flight flight)
        {
            //create .csv file to store list of all passengers that can be displayed
            using (StreamWriter writer = new("flight-manifest.csv"))
            {
                foreach (string[] passenger in flight.passengers)
                {
                    //join passenger last and first name with comma for .csv compatability
                    writer.WriteLine(string.Join(",", passenger));
                }
            }
        }*/
        
    }
}
