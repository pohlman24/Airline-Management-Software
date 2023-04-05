﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Plane
    {
        public string model { get; set; };
        public int capacity { get; set; };
        public int PlaneId { get; set; }

        public Plane(string name)
        {
            this.name = name;
            setCapacity(name);
        }

        //determine capacity based on plane model
        private void setCapacity(string name)
        {
            switch (name)
            {
                case "Boeing 737":
                    capacity = 100;
                    break;
                case "Boeing 767":
                    capacity = 150;
                    break;
                case "Boeing 777":
                    capacity = 200;
                    break;
                default:
                    break;
            }
        }
    }
}
