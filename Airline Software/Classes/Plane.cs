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

        public Plane(string name)
        {
            this.Model = name;
            setCapacity(name);
        }

        //determine capacity based on plane model
        private void setCapacity(string name)
        {
            switch (name)
            {
                case "Boeing 737":
                    Capacity = 100;
                    break;
                case "Boeing 767":
                    Capacity = 150;
                    break;
                case "Boeing 777":
                    Capacity = 200;
                    break;
                default:
                    break;
            }
        }
    }
}
