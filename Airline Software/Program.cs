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
        // Flight newFlight = Flight.CreateFlight(1, 2, dt1, dt1, 100, 200);
        // READ 
         Flight newFlight = Flight.FindFlightById(1);
        // Console.WriteLine(newFlight.ArrivalAirportID);
        // UPDATE
        // Flight.UpdateFlight(newFlight, flightNumber:"TOLDET100");
        // DELETE
        //Flight.DeleteFlight(newFlight);

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
        MarketingManager.AssignPlaneForAllFlights();
    }

}