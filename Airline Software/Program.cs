using Airline_Software;
using System;

class Program
{
    static bool loggedIn = false; //global var to check if a customer is logged in
    static User currentUser = null;
    static Customer currentCustomer = null;

    static void Main(string[] args)
    {
        int choice;

        while (true)
        {
            DefaultPage();
            /*
            if (loggedIn == false)
            {
                DefaultPage();
            }
            else
            {
                HomePage(); //TODO only if customer is logged in
            }
            */
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateCustomerAccount();
                    //MarketingManagerFunctionality();
                    break;

                case 2:
                    LogIn();
                    //AccountantFunctionality();
                    break;

                case 3:
                     BookFlight();
                     break;

                 case 4:
                     CancelFlight();
                     break;

                 case 5:
                     ViewAccountHistory();
                     break;

                 case 6:
                     //ChangePassword();
                     break;

                case 7:
                    Console.WriteLine("\nThank you for using our service!");
                    return;

                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
            }
        }
    }

    static void CreateCustomerAccount()
    {
        Console.WriteLine("\n**** CREATE ACCOUNT ****");
        Console.Write("Enter your first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter your last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your phone number: ");
        string phoneNumber = Console.ReadLine();
        Console.Write("Enter your age: ");
        int age = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter your address: ");
        string address = Console.ReadLine();
        Console.Write("Enter your city: ");
        string city = Console.ReadLine();
        Console.Write("Enter your state: ");
        string state = Console.ReadLine();
        Console.Write("Enter your zip code: ");
        string zipCode = Console.ReadLine();
        Console.Write("Enter your credit card number: "); //TODO CVV??????
        string creditCardNumber = Console.ReadLine();
        Console.Write("Create a password: ");
        string password = User.HashPassword(Console.ReadLine());
        currentCustomer = Customer.CreateCustomer(firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, "Customer", creditCardNumber);
        Console.WriteLine("\nAccount created successfully! Your ID number is " + currentCustomer.Id);
        loggedIn = true;
    }

    static void LogIn()
    {
        Console.WriteLine("\n**** LOG IN ****");
        Console.Write("Enter your ID number: ");
        int userId = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter your password: ");
        string password = User.HashPassword(Console.ReadLine());
        try
        {
            currentUser = User.FindUserById(userId);
            if (currentUser.UserType == "Customer")
            {
                currentCustomer = Customer.FindCustomerById(currentUser.Id);
                currentUser = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nInvalid ID!"); //TODO solve
        }
        if (currentUser == null)
        {
            if (password == currentCustomer.Password)
            {
                Console.WriteLine("\nWelcome, " + currentCustomer.FirstName + "!");
                loggedIn = true;
            }
        }
        else if (password == currentUser.Password && currentUser.UserType == "Load Engineer")
        {
            Console.WriteLine("\nWelcome, " + currentUser.FirstName + "!");
            loggedIn = true;
            LoadEngineerFunctionality();
        }
        else if (password == currentUser.Password && currentUser.UserType == "Flight Manager")
        {
            Console.WriteLine("\nWelcome, " + currentUser.FirstName + "!");
            loggedIn = true;
            FlightManagerFunctionality();
        }
        else
        {
            Console.WriteLine("\nInvalid email or password!");
        }
    }

    static void BookFlight()
    {
        //display all airports
        List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(@"..\..\..\Tables\AirportDb.csv");
        Console.Write("\nSelect origin and destination airports from below:\n");
        for (int i = 0; i < airports.Count; i++)
        {
            Console.WriteLine("  " + (i + 1) + ". " + airports[i].Code + " (" + airports[i].City + ", " + airports[i].State + ")");
        }
        Console.Write("Origin number: ");
        //user selects airport by id
        Airport airport = Airport.FindAirportbyId(Convert.ToInt32(Console.ReadLine()));
        Console.Write("Destination number: ");
        Airport airport2 = Airport.FindAirportbyId(Convert.ToInt32(Console.ReadLine()));

        //determine date and roundtrip
        Console.Write("Will the flight be roundtrip? (Y/N): ");
        string roundtrip = Console.ReadLine();
        Console.Write("Please select a departure date (YYYY-MM-DD): ");
        string departDate = Console.ReadLine();
        string returnDate = "";
        if (roundtrip == "Y")
        {
            Console.Write("Please select a return date (YYYY-MM-DD): ");
            returnDate = Console.ReadLine();
        }
        else if (roundtrip != "N")
        {
            Console.WriteLine("Invalid Input!");
        }

        //get all flights departing from selected airport
        List<Flight> allFlights = CsvDatabase.ReadCsvFile<Flight>(@"..\..\..\Tables\FlightDb.csv");
        Dictionary<int, Flight> curFlights = new(); //stores flights departing from current airport, int console display as key
        bool noFlights = true;
        int j = 0; //number the display list
        for (int i = 0; i < allFlights.Count; i++)
        {
            if (allFlights[i].ArrivalAirportID == airport2.AirportId && allFlights[i].DepartureAirportID == airport.AirportId && allFlights[i].DepartureTime >= DateTime.Now && allFlights[i].DepartureTime <= DateTime.Now.AddMonths(6) && (allFlights[i].DepartureTime).ToString("yyyy-MM-dd") == departDate) //if flight is within the next 6 months and also of the selected date
            {
                if (noFlights == true)
                {
                    noFlights = false;
                    Console.WriteLine("\nDeparting flights:");
                }
                j++;
                curFlights.Add(j, allFlights[i]);
                Console.WriteLine("  " + j + ". " + allFlights[i].FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + allFlights[i].DepartureTime + " - Arrival: " + allFlights[i].ArrivalTime + ")"); //TODO format spacing and add price, remove seconds
            }
        }
        if (noFlights == true)
        {
            Console.WriteLine("No departing flights from " + airport.Code + " are available on " + departDate); //TODO go back if no flights
        }
        Console.Write("Enter your choice: ");
        //else user selects a flight by number, which is used to get id
        Flight chosenDepartFlight = curFlights[Convert.ToInt32(Console.ReadLine())];
        Flight chosenReturnFlight = null;
        //for roundtrip
        if (roundtrip == "Y")
        {
            curFlights = new();
            noFlights = true;
            j = 0;
            for (int i = 0; i < allFlights.Count; i++)
            {
                if (allFlights[i].ArrivalAirportID == airport.AirportId && allFlights[i].DepartureAirportID == airport2.AirportId && allFlights[i].DepartureTime > chosenDepartFlight.ArrivalTime && (allFlights[i].DepartureTime).ToString("yyyy-MM-dd") == returnDate) //TODO should only show flights with date after selected depart date?
                {
                    if (noFlights == true)
                    {
                        noFlights = false;
                        Console.WriteLine("\nReturning flights:");
                    }
                    j++;
                    curFlights.Add(j, allFlights[i]);
                    Console.WriteLine("  " + j + ". " + allFlights[i].FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + allFlights[i].DepartureTime + " - Arrival: " + allFlights[i].ArrivalTime + ")"); //TODO format spacing
                }
            }
            if (noFlights == true)
            {
                Console.WriteLine("No returning flights from " + airport.Code + " are available on " + returnDate); //TODO go back if no flights
            }
            Console.Write("Enter your choice: ");
            //else user selects a flight by number, which is used to get id
            chosenReturnFlight = curFlights[Convert.ToInt32(Console.ReadLine())];
        }

        //proceed to checkout or browse more flights
        if (roundtrip == "Y")
        {
            double price = chosenDepartFlight.Price + chosenReturnFlight.Price;
            int points = 10 * (chosenDepartFlight.PointsEarned + chosenReturnFlight.PointsEarned);
            Console.Write("\n" + airport.Code + " to " + airport2.Code + " (roundtrip) costs $" + price.ToString("F2") + " (" + points + " points). Proceed to checkout? (Y/N): ");
        }
        else
        {
            Console.Write("\n" + airport.Code + " to " + airport2.Code + " costs $" + chosenDepartFlight.Price.ToString("F2") + " (" + 10 * chosenDepartFlight.PointsEarned + " points). Proceed to checkout? (Y/N): ");
        }
        string proceed = Console.ReadLine();
        if (proceed == "Y")
        {
            if (loggedIn == false)
            {
                Console.WriteLine("\nPlease log in or create an account first!\n  1. Log in\n  2. Create Account");
                Console.Write("Enter your choice: ");
                int command = Convert.ToInt32(Console.ReadLine());

                if (command == 1)
                {
                    LogIn();
                }
                else if (command == 2)
                {
                    CreateCustomerAccount();
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            }

            //after login, pay and book trip, creating a boarding pass and orders
            Console.WriteLine("\nHow would you like to pay?\n  1. Card\n  2. Points (You currently have " + currentCustomer.MileagePoints + " points)");
            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                if (roundtrip == "Y")
                {
                    Customer.BookTrip(currentCustomer, false, chosenDepartFlight.FlightId, chosenReturnFlight.FlightId);
                }
                else
                {
                    Customer.BookTrip(currentCustomer, false, chosenDepartFlight.FlightId);
                }
            }
            else if (choice == 2)
            {
                try
                {
                    if (roundtrip == "Y")
                    {
                        Customer.BookTrip(currentCustomer, true, chosenDepartFlight.FlightId, chosenReturnFlight.FlightId);
                    }
                    else
                    {
                        Customer.BookTrip(currentCustomer, true, chosenDepartFlight.FlightId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n" + ex.Message); //TODO go back
                }
            }
            else
            {
                Console.WriteLine("Invalid Input!");
            }
            Console.WriteLine("\nFlight Booked Successfully!");
            //TODO stuff here
        }
        else if (proceed == "N")
        {
            //TODO go back
        }
        else
        {
            Console.WriteLine("Invalid Input!"); //TODO solve invalid inputs
        }
    }

    static void CancelFlight()
    {
        if (loggedIn == false) //check if logged in or else there are no flights to cancel
        {
            Console.WriteLine("\nPlease log in or create an account first!\n  1. Log in\n  2. Create Account");
            Console.Write("Enter your choice: ");
            int command = Convert.ToInt32(Console.ReadLine());

            if (command == 1)
            {
                LogIn();
            }
            else if (command == 2)
            {
                CreateCustomerAccount();
            }
            else
            {
                Console.WriteLine("Invalid Input!"); //TODO what after
            }
        }

        //display all booked flights
        Customer.GetOrders(currentCustomer); //get all order history
        if (currentCustomer.ActiveOrders.Count > 0)
        {
            Console.WriteLine("\nCurrent Cancellable Bookings:");
            for (int i = 0; i < currentCustomer.ActiveOrders.Count; i++)
            {
                Console.Write("  " + (i + 1) + ". ");
                Customer.PrintFlightInfo(currentCustomer, i);
            }
            Console.Write("Enter your choice: ");
            int cancelChoice = Convert.ToInt32(Console.ReadLine());
            Console.Write("Are you sure you would like to cancel Flight " + cancelChoice + "? (Y/N): ");
            string cancelString = Console.ReadLine(); //TODO toLower()
            if (cancelString == "Y")
            {
                Order.CancelOrder(currentCustomer.ActiveOrders[cancelChoice - 1], currentCustomer);
                Console.WriteLine("\nFlight Canceled!");
            }
            else if (cancelString == "N")
            {
                //TODO
            }
            else
            {
                Console.WriteLine("Invalid Input!"); //TODO what after
            }
        }
        else
        {
            Console.WriteLine("\nNo Current Bookings");
        }
    }

    static void ViewAccountHistory()
    {
        if (loggedIn == false) //check if logged in or else there are no flights to cancel
        {
            Console.WriteLine("\nPlease log in or create an account first!\n  1. Log in\n  2. Create Account");
            Console.Write("Enter your choice: ");
            int command = Convert.ToInt32(Console.ReadLine());

            if (command == 1)
            {
                LogIn();
            }
            else if (command == 2)
            {
                CreateCustomerAccount();
            }
            else
            {
                Console.WriteLine("Invalid Input!"); //TODO what after
            }
        }

        Customer.ViewAccountHistory(currentCustomer);
    }

    /* TODO this function
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
      }
    */

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
                case 1:
                    BookFlight();
                    break;
                case 2:
                    CancelFlight();
                    break;
                case 3:
                    ViewAccountHistory();
                    break;
                case 4:
                    //ChangePassword();
                    break;
                case 5:
                    Console.WriteLine("\nLogging out...");
                    currentCustomer = null;
                    loggedIn = false;
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
                    currentUser = null;
                    loggedIn = false;
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
            }
        }
    }

    static void AddFlight()
    {
        //Flight.AssignPlaneForFlight();


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
                    currentUser = null;
                    loggedIn = false;
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

                        Flight.ShowAllFlights();
                        Console.Write("Enter flight number: ");

                        string num = Console.ReadLine();
                        

                        Flight flight = Flight.FindFlightByFlightNumber(num);

                        

                        double percentCapacity = Accountant.CalcPercentCapacity(flight);
                        Console.WriteLine("Percent Capacity: " + percentCapacity);
                        break;

                    case 2:

                        Flight.ShowAllFlights();

                        Console.Write("Enter flight number: ");

                        string num1 = Console.ReadLine();

                        Flight flight2 = Flight.FindFlightByFlightNumber(num1);

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
                        currentUser = null;
                        loggedIn = false;
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

                            Flight.FlightWithNoPlane();
                            Console.WriteLine("Enter flight number: ");
                            string flightNumber = Console.ReadLine();

                            Flight flight = Flight.FindFlightByFlightNumber(flightNumber);
                            // Retrieve flight by ID
                            Console.WriteLine("Assigning Plane for Flight " + flightNumber);

                            // Assign plane for flight
                            MarketingManager.AssignPlaneForFlight(flight);

                            Console.WriteLine("Planed Assigned for flight: " + flightNumber + "\n");

                            break;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while assigning plane for flight: " + ex.Message);
                        }
                        break;
                    case 2:
                        try
                        {
                           

                            Console.WriteLine("Assinging Plane for all flights");



                            MarketingManager.AssignPlaneForAllFlights();

                            Console.WriteLine("Planes assigned for all flights");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while assigning planes for all flights: " + ex.Message);
                        }
                        break;
                    case 3:
                        Console.WriteLine("\nLogging out...");
                        currentUser = null;
                        loggedIn = false;
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