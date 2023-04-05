using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class Accountant : Admin
    {
        public Accountant()
        {
            //position for accountant is 4
            setID(4);
        }

        public void generateSummary()
        {
            //need to know how flight data is being stored first
        }

        //private void calculateFlightIncome(); //same with above how is data being stored
    }
}
