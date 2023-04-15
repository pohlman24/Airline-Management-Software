using Airline_Software;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        // Testing Customer CRUD functions
        // CREATE
        // User newUser = Customer.CreateCustomer("Reece", "Pohlman", "Reecepohlman@gmail.com", "4197054849", int.Parse("23"), "1234 Home Address", "Sylvania", "Ohio", "43560", "Password", "Customer", "123456789");
        // READ
        // Customer me = Customer.FindCustomerById(364608);
        // UPDATE
        // Customer.UpdateCustomer(me, firstName: "bobby", lastName: "Johnson");
        // DELETE
        // Customer.DeleteCustomer(me);
        // ChangePassword
        // Customer.ChangeCustomerPassword(me, "potato");

        // Testing Airport CRUD functions
        // CREATE
        // Airport newAirport = Airport.CreateAirport("Toledo", "Ohio", "TOL", int.Parse("12345"), int.Parse("098765"));
        // READ
        // Airport DETairport = Airport.FindAirportbyId(2);
        // UPDATE
        // Airport.UpdateAirport(DETairport, city: "Sylvania");
        // DELETE
        // Airport.DeleteAirport(DETairport);

        // Testing BoardingPass CRUD functions
        // CREATE
        // BoardingPass newPass = BoardingPass.CreateBoardingPass(1, 364608, 1, "bobby", "Johnson", new DateTime(), new DateTime(), 1, 2);
        // READ 
        // BoardingPass newPass = BoardingPass.FindBoardingPassById(1);
        // Console.WriteLine(newPass.FirstName);   
        // UPDATE
        // BoardingPass.UpdateBoardingPass(newPass, firstName: "Reece", lastName: "Pohlman");
        // DELETE
        // BoardingPass.DeleteBoardingPass(newPass);

        // Testing Flight CRUD functions
        // CREATE
        // DateTime dt1 = DateTime.Now;
        // Flight newFlight = Flight.CreateFlight(1, 2, dt1);
        // READ 
        // Flight newFlight = Flight.FindFlightById(5);
        // Console.WriteLine(newFlight.ArrivalAirportID);
        // UPDATE
        // DateTime departTime = new DateTime(2023, 4, 13, 9, 30, 0);
        // Flight.UpdateFlight(newFlight, departureTime : departTime);
        // DELETE
        // Flight.DeleteFlight(newFlight);
        // SET PRICE AND POINTS
        // Console.WriteLine(newFlight.Price);
        // Console.WriteLine(newFlight.PointsEarned);


        /*
        //testing
        //DateTime dt1 = DateTime.Now;
        //DateTime dt2 = new(2023, 4, 18, 10, 0, 0);
        //Flight newFlight = Flight.CreateFlight(1, 2, dt1);
        //Flight newFlight2 = Flight.CreateFlight(2, 1, dt2);
        //Customer customer = Customer.CreateCustomer("bob", "bobby", "Reecepohlman@gmail.com", "4197054849", int.Parse("23"), "1234 Home Address", "Sylvania", "Ohio", "43560", "Password", "Customer", "123456789");
        Customer customer = Customer.FindCustomerById(149340);
        //Customer.UpdatePoints(customer, 100000);
        //Customer.BookTrip(customer, true, newFlight.FlightId, newFlight2.FlightId);
        Order order = Order.FindOrderById(4);
        //Console.WriteLine(customer.ActiveOrders.Count);
        //Order.CancelOrder(order);
        //TODO Mileage doesnt update unless calling find customer
        Console.WriteLine(customer.MileagePoints);
        customer = Customer.FindCustomerById(149340);
        Customer.ViewAccountHistory(customer);
        */
        

        // Testing Order CRUD functions
        // CREATE
        // Order newOrder = Order.CreateOrder(364608, 2, "Active", new DateOnly(), new DateOnly(), true);
        // READ
        // Order newOrder = Order.FindOrderById(2);
        // UPDATE
        // Order.UpdateOrder(newOrder, orderStatus: "Canceled");
        // DELETE
        // Order.DeleteOrder(newOrder);

        // Testing accountant Functions
        //Flight flight = Flight.FindFlightById(2);
        // CalcNumFlights()
        // int flightNums = Accountant.CalcNumFlights("week");
        // Console.WriteLine(flightNums);
        // CalcPercentCapacity()
        // double percent = Accountant.CalcPercentCapacity(flight);
        // Console.WriteLine(percent);
        // CalcIncomeFlight()
        // Console.WriteLine(Accountant.CalcIncomeFlight(flight));
        // CalcIncomeWhole()
        // Console.WriteLine(Accountant.CalcIncomeWhole());

        // FlightManager.PrintFlightManifest(flight);
        // BoardingPass.PrintBoardingPass(me);

        // Test Marketing manager assign Plane function 
        // MarketingManager.AssignPlaneForAllFlights();

        // LoadEngineer.scheduleFlight();
    }
}