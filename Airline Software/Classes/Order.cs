﻿namespace Airline_Software
{
    public class Order
    {
        // AKA bookings -- CONNECTS CUSTOMER TO FLIGHT
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int FlightId1 { get; set; } //outbound flight
        public int FlightId2 { get; set; } //optional return flight
        public string OrderStatus { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly CancellationDate { get; set; } 
        public Boolean IsRoundTrip { get; set; }
        public Boolean UsedPoints { get; set; } //if customer paid with points, points spent will be decreased on cancellation


        public Order(int OrderId, int CustomerId, int FlightId1, int FlightId2, string OrderStatus, DateOnly OrderDate, DateOnly CancellationDate, bool IsRoundTrip, bool UsedPoints)
        {
            this.OrderId = OrderId;
            this.CustomerId = CustomerId;
            this.FlightId1 = FlightId1;
            this.FlightId2 = FlightId2;
            this.OrderStatus = OrderStatus;
            this.OrderDate = OrderDate;
            this.CancellationDate = CancellationDate;
            this.IsRoundTrip = IsRoundTrip;
            this.UsedPoints = UsedPoints;
        }

        public static Order CreateOrder(int customerId, int flightId1,string orderStatus, DateOnly orderDate, DateOnly cancellationDate, bool isRoundTrip, bool UsedPoints, int flightId2 = -1)
        {
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            int orderId = GenerateOrderID();
            Order newOrder = new Order(orderId, customerId, flightId1, flightId2, orderStatus, orderDate, cancellationDate, isRoundTrip, UsedPoints);
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            orders.Add(newOrder);
            CsvDatabase.WriteCsvFile<Order>(filePath, orders);
            return newOrder;
        }

        public static void UpdateOrder(Order order, int customerId = -1, int flightId1 = -1, int flightId2 = -1, string orderStatus = "",
                               DateOnly? orderDate = null, DateOnly? cancellationDate = null, bool? isRoundTrip = null, bool? usedPoints = null)
        {
            order.CustomerId = customerId == -1 ? order.CustomerId : customerId;
            order.FlightId1 = flightId1 == -1 ? order.FlightId1 : flightId1;
            order.FlightId2 = flightId2 == -1 ? order.FlightId2 : flightId2;
            order.OrderStatus = string.IsNullOrEmpty(orderStatus) ? order.OrderStatus : orderStatus;
            order.OrderDate = orderDate == null ? order.OrderDate : orderDate.Value;
            order.CancellationDate = cancellationDate == null ? order.CancellationDate : cancellationDate.Value;
            order.IsRoundTrip = isRoundTrip == null ? order.IsRoundTrip : isRoundTrip.Value;
            order.UsedPoints = usedPoints == null ? order.UsedPoints : usedPoints.Value;

            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);

            CsvDatabase.UpdateRecord(orders, o => o.OrderId, order.OrderId, (current, updated) =>
            {
                current.CustomerId = updated.CustomerId;
                current.FlightId1 = updated.FlightId1;
                current.FlightId2 = updated.FlightId2;
                current.OrderStatus = updated.OrderStatus;
                current.OrderDate = updated.OrderDate;
                current.CancellationDate = updated.CancellationDate;
                current.IsRoundTrip = updated.IsRoundTrip;
                current.UsedPoints = updated.UsedPoints;
            }, order);

            CsvDatabase.WriteCsvFile(filePath, orders);
        }

        public static void DeleteOrder(Order order)
        {
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            CsvDatabase.RemoveRecord(orders, o => o.OrderId, order.OrderId);
            CsvDatabase.WriteCsvFile(filePath, orders);
        }

        public static Order FindOrderById(int id)
        {
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            if (CsvDatabase.FindRecord(orders, o => o.OrderId, id) == null)
            {
                throw (new Exception("Id Not Found"));
                return null;
            }
            Order order = CsvDatabase.FindRecord(orders, o => o.OrderId, id);
            return order;
        }

        private static int GenerateOrderID()
        {
            string filePath = @"..\..\..\Tables\OrderDb.csv"; ;
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            int maxID = orders.Count > 0 ? orders.Max(p => p.OrderId) : 0;
            return maxID + 1;
        }

        public static void CancelOrder(Order order, Customer customer)
        {
            Flight flight1 = Flight.FindFlightById(order.FlightId1);
            Customer.UpdatePoints(customer, 10 * flight1.PointsEarned);
            if (order.UsedPoints == true) //remove points spent if customer cancels order paid using points
            {
                Customer.UpdatePointsSpent(customer, -10 * flight1.PointsEarned);
            }

            if (order.FlightId2 != -1)
            {
                Flight flight2 = Flight.FindFlightById(order.FlightId2); //for trip back
                Customer.UpdatePoints(customer, 10 * flight2.PointsEarned);
                if (order.UsedPoints == true) //remove points spent if customer cancels order paid using points
                {
                    Customer.UpdatePointsSpent(customer, -10 * flight1.PointsEarned);
                }
            }

            UpdateOrder(order, orderStatus : "Canceled"); //want to update order to be cancelled
        }
    }
}