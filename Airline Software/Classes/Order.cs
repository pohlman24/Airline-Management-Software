﻿namespace Airline_Software
{
    public class Order
    {
        // AKA bookings -- CONNECTS CUSTOMER TO FLIGHT
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int FlightId1 { get; set; } //outbound flight
        public int FlightId2 { get; set; } = -1; //optional return flight
        public string OrderStatus { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly CancellationDate { get; set; } 
        public Boolean IsRoundTrip { get; set; }


        public Order(int OrderId, int CustomerId, int FlightId1, int FlightId2, string OrderStatus, DateOnly OrderDate, DateOnly CancellationDate, bool IsRoundTrip)
        {
            this.OrderId = OrderId;
            this.CustomerId = CustomerId;
            this.FlightId1 = FlightId1;
            this.FlightId2 = FlightId2;
            this.OrderStatus = OrderStatus;
            this.OrderDate = OrderDate;
            this.CancellationDate = CancellationDate;
            this.IsRoundTrip = IsRoundTrip;
        }

        public static Order CreateOrder(int customerId, int flightId1, string orderStatus, DateOnly orderDate, DateOnly cancellationDate, bool isRoundTrip, int flightId2 = -1)
        {
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            int orderId = GenerateOrderID();
            Order newOrder = new Order(orderId, customerId, flightId1, flightId2, orderStatus, orderDate, cancellationDate, isRoundTrip);
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            orders.Add(newOrder);
            CsvDatabase.WriteCsvFile<Order>(filePath, orders);

            // need to also create the boarding pass to db
            BoardingPass.CreateBoardingPass(orderId, customerId, flightId1, Customer.FindCustomerById(customerId).FirstName, Customer.FindCustomerById(customerId).LastName,
                                            Flight.FindFlightById(flightId1).DepartureTime, Flight.FindFlightById(flightId1).ArrivalTime,
                                            Flight.FindFlightById(flightId1).DepartureAirportID, Flight.FindFlightById(flightId1).ArrivalAirportID);

            if (flightId2 != -1)
            {
                BoardingPass.CreateBoardingPass(orderId, customerId, flightId2, Customer.FindCustomerById(customerId).FirstName, Customer.FindCustomerById(customerId).LastName,
                                            Flight.FindFlightById(flightId2).DepartureTime, Flight.FindFlightById(flightId2).ArrivalTime,
                                            Flight.FindFlightById(flightId2).DepartureAirportID, Flight.FindFlightById(flightId2).ArrivalAirportID);
            }

            return newOrder;
        }

        public static void UpdateOrder(Order order, int customerId = -1, int flightId1 = -1, int flightId2 = -1, string orderStatus = "",
                               DateOnly? orderDate = null, DateOnly? cancellationDate = null, bool? isRoundTrip = null)
        {
            order.CustomerId = customerId == -1 ? order.CustomerId : customerId;
            order.FlightId1 = flightId1 == -1 ? order.FlightId1 : flightId1;
            order.FlightId2 = flightId2 == -1 ? order.FlightId2 : flightId2;
            order.OrderStatus = string.IsNullOrEmpty(orderStatus) ? order.OrderStatus : orderStatus;
            order.OrderDate = orderDate == null ? order.OrderDate : orderDate.Value;
            order.CancellationDate = cancellationDate == null ? order.CancellationDate : cancellationDate.Value;
            order.IsRoundTrip = isRoundTrip == null ? order.IsRoundTrip : isRoundTrip.Value;

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

        public static void CancelOrder(Order order)
        {
            Customer customer = Customer.FindCustomerById(order.CustomerId);
            Flight flight1 = Flight.FindFlightById(order.FlightId1);
            Customer.UpdatePoints(customer, flight1.PointsEarned);
            //TODO what about order history?

            if (order.FlightId2 != -1)
            {
                Flight flight2 = Flight.FindFlightById(order.FlightId2); //for trip back
                Customer.UpdatePoints(customer, flight2.PointsEarned);
            }

            DeleteOrder(order);
        }
    }
}