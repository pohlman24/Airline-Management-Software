namespace Airline_Software
{
    public class Order
    {
        // orders are the joing table between customers and tickets 
        // assumes that one customer can buy multiple tickets but all must be for the same flight
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int FlightId { get; set; }
        public List<Ticket> Tickets { get; set; }
        public int TotalCost { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }
}