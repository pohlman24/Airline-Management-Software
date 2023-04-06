using System;
using System.Collections.Generic;
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

        public void generateSummary()
        {
            //need to know how flight data is being stored first
        }

        //private void calculateFlightIncome(); //same with above how is data being stored
    }
}
