using Airline_Software;
using System;

class Program
{
    static void Main(string[] args)
    {
        int choice;

        while (true)
        {
            DefaultPage();
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateAccount();
                    break;

                case 2:
                    LogIn();
                    break;

                /* case 3:
                     BookFlight();
                     break;

                 case 4:
                     CancelFlight();
                     break;

                 case 5:
                     ViewAccountHistory();
                     break;

                 case 6:
                     ChangePassword();
                     break;*/

                case 7:
                    Console.WriteLine("\nThank you for using our service!");
                    return;

                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
            }
        }
    }

    static void CreateAccount()
    {
        Console.Write("\nEnter your name: ");
        string nameInput = Console.ReadLine();
        Console.Write("Enter your email: ");
        string emailInput = Console.ReadLine();
        Console.Write("Create a password: ");
        string passwordInput = Console.ReadLine();

        Console.WriteLine("\nAccount created successfully!");
    }

    static void LogIn()
    {
        Console.WriteLine("\n\n**** LOG IN ****");
        Console.Write("\nEnter your id: ");
        string userId = Console.ReadLine();
        int id = int.Parse(userId);
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();
        User user = User.FindUserById(id);
        if (password == user.Password && user.UserType == "Customer")
        {
            Console.WriteLine("\nWelcome, " + user.FirstName + user.LastName + "!");
            HomePage();
        }
        else if (password == user.Password && user.UserType == "Load Engineer")
        {
            Console.WriteLine("\nWelcome, " + user.FirstName + " " + user.LastName + "!");
            LoadEngineerFunctionality();
        }
        else if (password == user.Password && user.UserType == "Flight Manager")
        {
            Console.WriteLine("\nWelcome, " + user.FirstName + " " + user.LastName + "!");
            FlightManagerFunctionality();
        }
        else
        {
            Console.WriteLine("\nInvalid email or password!");
        }
    }

    /*  static void BookFlight()
      {

          Console.Write("\nEnter the origin airport: ");
          flight.origin = Console.ReadLine();
          Console.Write("Enter the destination airport: ");
          flight.destination = Console.ReadLine();
          Console.Write("Enter the departure date (dd/mm/yyyy): ");
          flight.date = Console.ReadLine();
          Console.Write("Enter the number of passengers: ");
          flight.num_passengers = int.Parse(Console.ReadLine());

          if (user.email == null)
          {
              Console.WriteLine("\nPlease log in or create an account first!");
              DefaultPage();

          }

           Console.WriteLine("\nFlight booked successfully!");
      }

      static void CancelFlight()
      {
          if (user.email == null)
          {
              Console.WriteLine("\nPlease log in or create an account first!");
              return;
          }
          if (flight.num_passengers > 0)
          {
              Console.WriteLine("\nDo you want to cancel your flight? (Y/N): ");
              string answer = Console.ReadLine().ToLower();
              if (answer == "y")
              {
                  flight.num_passengers = 0;
                  Console.WriteLine("\nFlight cancelled successfully!");

              }
              else
              {
                  Console.WriteLine("\nFlight cancellation aborted.");

              }

          }
          else
          {
              Console.WriteLine("\nNo Flight to cancel");
          }
      }

      static void ViewAccountHistory()
      {
          if (user.email == null)
          {
              Console.WriteLine("\nPlease log in or create an account first!");
              return;
          }
          Console.WriteLine("\nThis feature is not yet implemented.");
      }

      static void ChangePassword()
      {
          if (user.email == null)
          {
              Console.WriteLine("\nPlease log in or create an account first!");
              return;
          }
          Console.Write("\nEnter your current password: ");
          string currentPassword = Console.ReadLine();
          if (currentPassword == user.password)
          {
              Console.Write("Enter your new password: ");
              user.password = Console.ReadLine();
              Console.WriteLine("Password changed successfully!");
          }
          else
          {
              Console.WriteLine("Incorrect password!");
          }
      }*/
    static void HomePage()
    {
        int choice;

        while (true)
        {
            Console.WriteLine("\n**** HOME PAGE ****");
            Console.WriteLine("1. Book a Flight");
            Console.WriteLine("2. Cancel a Flight");
            Console.WriteLine("3. View Account History");
            Console.WriteLine("4. Change Password");
            Console.WriteLine("5. Log Out");
            Console.Write("Enter your choice: ");
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                /*case 1:
                    BookFlight();
                    break;
                case 2:
                    CancelFlight();
                    break;
                case 3:
                    ViewAccountHistory();
                    break;
                case 4:
                    ChangePassword();
                    break;*/
                case 5:
                    Console.WriteLine("\nLogging out...");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
            }
        }
    }

    static void DefaultPage()
    {
        Console.WriteLine("\n\n**** AIRLINE SERVICE SYSTEM ****");
        Console.WriteLine("1. Create Account");
        Console.WriteLine("2. Log In");
        Console.WriteLine("3. Book a Flight");
        Console.WriteLine("4. Cancel a Flight");
        Console.WriteLine("5. View Account History");
        Console.WriteLine("6. Change Password");
        Console.WriteLine("7. Exit");
        Console.Write("Enter your choice: ");
    }

    static void LoadEngineerFunctionality()
    {
        // Code to handle flight manager functionality goes here
        while (true)
        {
            Console.WriteLine("\n\n**** LOAD ENGINEER FUNCTIONALITY ****");
            Console.WriteLine("1. View All Flights");
            Console.WriteLine("2. Add a New Flight"); // this should only include depart time and arrival location
            Console.WriteLine("3. Delete a Flight");
            Console.WriteLine("4. Update a Flight");
            Console.WriteLine("5. Log Out");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine("\n");

            switch (choice)
            {
                case 1:
                    // output list of all scheduled flights 
                    Flight.ShowAllFlights();
                    break;
                case 2:
                    AddFlight();
                    break;
                case 3:
                    CancelFlightAdmin();
                    break;
                case 4:
                    UpdateFlight();
                    break;
                case 5:
                    Console.WriteLine("\nLogging out...");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
            }
        }
    }

    static void AddFlight()
    {
        Flight.AssignPlaneForFlight();


        // Get user input for flight details
        Console.WriteLine("**** Add Flight ****");
        Console.WriteLine("Enter your flight details or type 'back' to go back to previous page\n");
        Console.WriteLine("Flight Departure City: ");
        string departCity = Console.ReadLine();
        if (departCity == "back" || departCity == "BACK" || departCity == "Back" || departCity == "b") { LoadEngineerFunctionality(); }
        Console.WriteLine("Flight Arrival City: ");
        string arrCity = Console.ReadLine();
        Console.WriteLine("Flight Departure Time: yyyy-mm-dd hh:mm (24-hour)");
        string departTime = Console.ReadLine();

        // loop in case of invalid choice 
        while (true)
        {
            // Confirm input is correct
            Console.WriteLine("\nConfirm the following info is correct (Y/N)");
            Console.WriteLine("Depature City: " + departCity);
            Console.WriteLine("Arrival City: " + arrCity);
            Console.WriteLine("Departure Time: " + departTime);
            string isCorrect = Console.ReadLine().ToUpper();

            if (isCorrect == "Y" || isCorrect == "YES")
            {
                // use Try/Catch incase of invalid airport --- would technically be better to have the check right after the user inputs it
                // but I think that would make the code less organized and theres also the 
                try
                {
                    // get object references 
                    Airport departAirport = Airport.FindAirportbyCity(departCity);
                    Airport arrivalAiport = Airport.FindAirportbyCity(arrCity);
                    DateTime time = DateTime.Parse(departTime);
                    // create new flight 
                    Flight newFlight = Flight.CreateFlight(departAirport.AirportId, arrivalAiport.AirportId, time);
                    //output summary of created flight
                    Console.WriteLine("\nNew Flight Created");
                    newFlight.FlightSummary();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nAirport(s) not an available option. List of available aiports below.");
                    Airport.DisplayAllAiports();
                    AddFlight();
                }

            }
            else if (isCorrect == "N" || isCorrect == "NO")
            {
                // send user back to beginning
                AddFlight();
                break;
            }
            else
            {
                // if user enters something other than Y or N -- will send them back to top of while loop
                Console.Write("\nInvalid choice, please enter Y or N");
            }
        }

    }

    static void CancelFlightAdmin()
    {
        // get user input
        Console.WriteLine("**** Cancel Flight ****");
        Console.WriteLine("Enter flight number you want to cancel or type 'back' to go back to the previous page");
        string flightNumber = Console.ReadLine();
        if (flightNumber == "back" || flightNumber == "BACK" || flightNumber == "Back" || flightNumber == "b") { LoadEngineerFunctionality(); }

        // use try/catch block to check for invalid flight number 
        try
        {
            while (true) // isCorrect bookmark
            {
                // create flight reference and display info
                Flight flight = Flight.FindFlightByFlightNumber(flightNumber);
                // confirm correct flight 
                Console.WriteLine("\nIs this the correct flight? (Y/N)");
                flight.FlightSummary();
                string isCorrect = Console.ReadLine().ToUpper();

                if (isCorrect == "N" || isCorrect == "NO")
                {
                    // send user back to top of function
                    CancelFlightAdmin();
                }
                else if (isCorrect == "Y" || isCorrect == "YES")
                {
                    while (true) // isSure bookmark
                    {
                        Console.WriteLine("\nAre you sure you want to cancel the flight? (Y/N)");
                        string isSure = Console.ReadLine().ToUpper();
                        if (isSure == "Y" || isSure == "YES")
                        {
                            // delete flight
                            LoadEngineer.CancelFlight(flight);
                            break;
                        }
                        else if (isSure == "N" || isSure == "NO")
                        {
                            // send user back to top of function 
                            CancelFlightAdmin();
                            break;
                        }
                        else
                        {
                            // user entered something other than Y or N, send them back to isSure bookmark
                            Console.WriteLine("\nInvalid Choice. Enter either 'Y' or 'N'");
                        }
                    }
                    break;
                }
                else
                {
                    // IsCorrect check - if user entered something other than Y or N
                    // send them back to top isCorrect bookmark
                    Console.WriteLine("\nInvalid choice. Enter 'Y' or 'N'");
                }
            }
        }
        catch (Exception ex)
        {
            // flight not in DB, prompt them for flight number again
            Console.WriteLine("\nFlight not found\n");
            CancelFlightAdmin();
        }
    }

    // Flight manager function to update flight 
    static void UpdateFlight()
    {
        // get flight number of flight to udpate
        Console.WriteLine("**** Update Flight ****");
        Console.WriteLine("Enter flight number or type 'back' to go back to the previous page");
        string flightNumber = Console.ReadLine().ToUpper();
        if (flightNumber == "BACK") { LoadEngineerFunctionality(); } // go back feature 
        Console.WriteLine("");

        // need in try/catch block in case user enters invalid flight number
        try
        {
            while (true) // choice bookmark
            {
                // create reference to flight object 
                Flight flight = Flight.FindFlightByFlightNumber(flightNumber);
                // prompt user for field to update
                Console.WriteLine("Enter the number for the field you want to update\nOr type 'back' to enter new flight number\n");
                Console.WriteLine(
                    "Flight Number: {0}\n" +
                    "1. Departure City: {1}\n" +
                    "2. Arrival City: {2}\n" +
                    "3. Departure Time: {3}\n",
                    flight.FlightNumber,
                    Airport.FindAirportbyId(flight.DepartureAirportID).City,
                    Airport.FindAirportbyId(flight.ArrivalAirportID).City,
                    flight.DepartureTime
                );

                // check user input for back option
                string backChoice = Console.ReadLine().ToUpper(); // used for the 'back' check
                if (backChoice == "BACK") { UpdateFlight(); }

                // check user input
                int choice = int.Parse(backChoice); // used for the field choice
                // user wants to update departure city
                if (choice == 1)
                {
                    Console.WriteLine("Enter new departure city");
                    string newCity = Console.ReadLine();
                    Airport airport = Airport.FindAirportByProperty(newCity);
                    Flight.UpdateFlight(flight, departureAirportID: airport.AirportId);
                    Console.WriteLine("Flight Updated");
                    break;
                }
                // user wants to update arrival city
                else if (choice == 2)
                {
                    Console.WriteLine("Enter new arrival city");
                    string newCity = Console.ReadLine();
                    Airport airport = Airport.FindAirportbyCity(newCity);
                    Flight.UpdateFlight(flight, arrivalAirportID: airport.AirportId);
                    Console.WriteLine("Flight Updated");
                    break;
                }
                // user wants to update departure time
                else if (choice == 3)
                {
                    Console.WriteLine("Enter new departure time");
                    DateTime newTime = DateTime.Parse(Console.ReadLine());
                    Flight.UpdateFlight(flight, departureTime: newTime);
                    Console.WriteLine("Flight Updated");
                    break;
                }
                else
                {
                    // user entered a invalid option 
                    // send them back to choice bookmark
                    Console.WriteLine("Invalid Choice");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Flight not found");
            UpdateFlight();
        }

    }
  
    static void FlightManagerFunctionality()
    {
        // Code to handle flight manager functionality goes here
        while (true)
        {
            Console.WriteLine("\n\n**** FLIGHT MANAGER FUNCTIONALITY ****");
            Console.WriteLine("1. Generate Flight Manifest");
            Console.WriteLine("2. Log Out");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    GenerateFlightManifest();
                    break;
                case 2:
                    Console.WriteLine("\nLogging out...");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
            }
        }
    }

    static void GenerateFlightManifest()
    {
        Console.WriteLine("\n**** Print Flight Manifest ****");
        Console.WriteLine("Enter flight number or type 'back' to go back");
        string userInput = Console.ReadLine().ToUpper();
        if (userInput == "BACK") { FlightManagerFunctionality(); }
        try
        {
            Flight flight = Flight.FindFlightByFlightNumber(userInput);
            FlightManager.PrintFlightManifest(flight);

            Console.WriteLine("Save manifest? (Y/N)");
            string isSaving = Console.ReadLine().ToUpper();
            if(isSaving == "Y" || isSaving == "YES")
            {
                FlightManager.SaveFlightManifest(flight);
            }
            else
            {
                FlightManagerFunctionality();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nFlight number not found");
            GenerateFlightManifest();
        }
    }


    static void AccountantFunctionality()
    {
        // Code to handle accountant functionality goes here
        while (true)
        {
            try
            {
                Console.WriteLine("\n\n**** ACCOUNTANT FUNCTIONALITY ****");
                Console.WriteLine("1. Calculate Percent Capacity");
                Console.WriteLine("2. Calculate the income generated by a flight ");
                Console.WriteLine("3. Calculate the total income generated by all flights ");
                Console.WriteLine("4. Calculate the number of flights scheduled within a time range ");
                Console.WriteLine("5. Log Out");
                Console.Write("Enter your choice: ");
                string choiceString = Console.ReadLine();
                int choice = int.Parse(choiceString);

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter flight ID: ");
                        int flightId = int.Parse(Console.ReadLine());
                        Flight flight = Flight.FindFlightById(flightId);

                        double percentCapacity = Accountant.CalcPercentCapacity(flight);
                        Console.WriteLine("Percent Capacity: " + percentCapacity);
                        break;

                    case 2:
                        Console.Write("Enter flight ID: ");
                        int flightId2 = int.Parse(Console.ReadLine());
                        Flight flight2 = Flight.FindFlightById(flightId2);

                        double income = Accountant.CalcIncomeFlight(flight2);
                        Console.WriteLine("Income per Flight: " + income);
                        break;

                    case 3:
                        double totalIncome = Accountant.CalcIncomeWhole();
                        Console.WriteLine("Income all Flights: " + totalIncome);
                        break;

                    case 4:
                        Console.WriteLine("Enter time range (day, month, or year):");
                        string range = Console.ReadLine();
                        int numFlights = Accountant.CalcNumFlights(range);
                        Console.WriteLine("Num of flights: " + numFlights);
                        break;

                    case 5:
                        Console.WriteLine("\nLogging out...");
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice! Please try again.");
                        break;
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid integer choice.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }


    static void MarketingManagerFunctionality()
    {
        while (true)
        {
            try
            {
                // Code to handle marketing manager functionality goes here
                Console.WriteLine("\n\n**** MARKETING MANAGER FUNCTIONALITY ****");
                Console.WriteLine("1. Assign Plane for flight");
                Console.WriteLine("2. Assign Plane for all flights");
                Console.WriteLine("3. Log Out");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        try
                        {
                            // Retrieve flight by ID
                            Console.Write("Enter flight ID: ");
                            int flightId = int.Parse(Console.ReadLine());
                            Flight flight = Flight.FindFlightById(flightId);

                            // Assign plane for flight
                            MarketingManager.AssignPlaneForFlight(flight);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while assigning plane for flight: " + ex.Message);
                        }
                        break;
                    case 2:
                        try
                        {
                            // Assign plane for all flights
                            MarketingManager.AssignPlaneForAllFlights();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while assigning planes for all flights: " + ex.Message);
                        }
                        break;
                    case 3:
                        Console.WriteLine("\nLogging out...");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice! Please try again.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid integer choice.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}