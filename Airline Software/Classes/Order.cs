namespace Airline_Software
{
    public class Order
    {
        // AKA bookings -- CONNECTS CUSTOMER TO FLIGHT
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int FlightId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CancellationDate { get; set; } // if they cancel
        public Boolean IsRoundTrip { get; set; } 
    }
}