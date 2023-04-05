using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class MarketingManager : Admin
    {
        public MarketingManager()
        {
            //position for marketing manager is 1
            setID(1);
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
