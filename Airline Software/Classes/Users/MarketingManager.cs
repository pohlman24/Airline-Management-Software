using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class MarketingManager : User
    {
        public MarketingManager(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {

        }


        //determine plane model based on total flight distance
        public Plane determinePlaneModel(double distance)
        {
            string planeName;

            if (distance < 500)
            {
                planeName = "Boeing 737";
            }
            else if (distance < 1200)
            {
                planeName = "Boeing 767";
            }
            else
            {
                planeName = "Boeing 777";
            }

            //TODO do i use dispose here idk what it is but maybe
            Plane plane = new(planeName);
            return plane;
        }
    }
}
