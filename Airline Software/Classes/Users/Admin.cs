using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Admin : User
    {
        public Admin(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {

        }






        //Each user will have a unique 6-digit (can’t start with zero), randomly-assigned customer number
        //(which will act as their user ID), tied to their name and address??????????

        //setter using arbitrarily assigned position number used to create unique identification for each child class
        protected void setID(int position)
        {
            Id = 100000 + position;
        }
    }
}
