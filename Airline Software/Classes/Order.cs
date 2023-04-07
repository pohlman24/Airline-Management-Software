namespace Airline_Software
{
    public class Order
    {
        // AKA bookings -- CONNECTS CUSTOMER TO FLIGHT
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int FlightId { get; set; }
        public string OrderStatus { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly CancellationDate { get; set; } 
        public Boolean IsRoundTrip { get; set; }


        public Order(int orderId, int customerId, int flightId, string orderStatus, DateOnly orderDate, DateOnly cancellationDate, bool isRoundTrip)
        {
            OrderId = orderId;
            CustomerId = customerId;
            FlightId = flightId;
            OrderStatus = orderStatus;
            OrderDate = orderDate;
            CancellationDate = cancellationDate;
            IsRoundTrip = isRoundTrip;
        }

        public static Order CreateOrder(int customerId, int flightId, string orderStatus, DateOnly orderDate, DateOnly cancellationDate, bool isRoundTrip)
        {
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            int orderId = GenerateOrderID();
            Order newOrder = new Order(orderId, customerId, flightId, orderStatus, orderDate, cancellationDate, isRoundTrip);
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            orders.Add(newOrder);
            CsvDatabase.WriteCsvFile<Order>(filePath, orders);
            return newOrder;
        }

        private static int GenerateOrderID()
        {
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            int maxID = orders.Count > 0 ? orders.Max(o => o.OrderId) : 0;
            return maxID + 1;
        }
    }
}