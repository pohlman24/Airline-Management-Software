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
            if (currentUser == null && loggedIn == true)
            {
                HomePage();
            }
            DefaultPage();
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateCustomerAccount();
                    
                    break;

                case 2:
                    LogIn();
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
                     //ChangePassword(); DO WE NEED THIS IF THEY ARENT LOGGED IN??
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
        while (true)
        {
            try
            {
                Console.WriteLine("\n\n**** CREATE ACCOUNT ****");
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
                Console.Write("Enter your credit card number: ");
                string creditCardNumber = Console.ReadLine();
                Console.Write("Create a password: ");
                string password = User.HashPassword(Console.ReadLine());
                currentCustomer = Customer.CreateCustomer(firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, "Customer", creditCardNumber);
                Console.WriteLine("\nAccount created successfully! Your ID number is " + currentCustomer.Id);
                loggedIn = true;
                break;
            }
            catch (Exception ex)
            {
                if (RetryCommand("information", "account creation") == false)
                {
                    return;
                }
            }
        }
    }

    // Any user login 
    static void LogIn()
    {
        while (true)
        {
            Console.WriteLine("\n\n**** LOG IN ****");
            Console.Write("Enter your ID number: ");
            int userId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter your password: ");
            string password = User.HashPassword(Console.ReadLine());
            try
            {
                // find user by ID and check if user type is customer
                currentUser = User.FindUserById(userId);
                if (currentUser.UserType == "Customer")
                {
                    currentCustomer = Customer.FindCustomerById(currentUser.Id);
                    currentUser = null;
                }
                
            }
            catch (Exception ex)
            {
                if (RetryCommand("ID", "login") == false)
                {
                    return;
                }
                continue;
            }
            if (currentCustomer != null && password == currentCustomer.Password)
            {
                Console.WriteLine("\nWelcome, " + currentCustomer.FirstName + "!");
                loggedIn = true;
                return;
            }
            else if (currentUser != null && password == currentUser.Password && currentUser.UserType == "Load Engineer")
            {
                Console.WriteLine("\nWelcome, " + currentUser.FirstName + "!");
                loggedIn = true;
                LoadEngineerFunctionality();
                return;
            }
            else if (currentUser != null && password == currentUser.Password && currentUser.UserType == "Flight Manager")
            {
                Console.WriteLine("\nWelcome, " + currentUser.FirstName + "!");
                loggedIn = true;
                FlightManagerFunctionality();
                return;
            }
            else if (currentUser != null && password == currentUser.Password && currentUser.UserType == "Marketing Manager")
            {
                Console.WriteLine("\nWelcome, " + currentUser.FirstName + "!");
                loggedIn = true;
                MarketingManagerFunctionality();
                return;
            }
            else if (currentUser != null && password == currentUser.Password && currentUser.UserType == "Accountant")
            {
                Console.WriteLine("\nWelcome, " + currentUser.FirstName + "!");
                loggedIn = true;
                AccountantFunctionality();
                return;
            }
            else
            {
                if (RetryCommand("password", "login") == false)
                {
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Customer menu options
    /// </summary>
    static void BookFlight()
    {
        Console.WriteLine("\n\n**** BOOK A FLIGHT ****");
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
                if (VerifyUser() == false)
                {
                    return;
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
            if (VerifyUser() == false)
            {
                return;
            }
        }

        Console.WriteLine("\n\n**** CANCEL A FLIGHT ****");
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
            Console.WriteLine("  B.  Go back");
            while (true)
            {
                try
                {
                    Console.Write("Enter your choice: ");
                    string cancelChoice = Console.ReadLine();
                    int cancel = 0;
                    if (cancelChoice == "B")
                    {
                        return;
                    }
                    else
                    {
                        cancel = Convert.ToInt32(cancelChoice);
                        if (cancel > currentCustomer.ActiveOrders.Count)
                        {
                            throw new Exception("Invalid Input");
                        }
                    }
                    while (true)
                    {
                        Console.Write("Are you sure you would like to cancel Flight " + cancelChoice + "? (Y/N): ");
                        string cancelString = Console.ReadLine(); //TODO toLower()
                        if (cancelString == "Y")
                        {
                            Order.CancelOrder(currentCustomer.ActiveOrders[cancel - 1], currentCustomer);
                            Console.WriteLine("\nFlight Canceled!");
                            return;
                        }
                        else if (cancelString == "N")
                        {
                            Console.WriteLine("\nChoose a flight from above, or enter B to go back");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid Input!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nInvalid Input! Choose a flight from above, or enter B to go back");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo Current Bookings");
            Console.Write("\nEnter B to go back: ");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "B")
                {
                    return;
                }
                else
                {
                    Console.Write("\nInvalid input! Enter B to go back: ");
                }
            }
        }
    }

    static void ViewAccountHistory()
    {
        if (loggedIn == false) //check if logged in or else there are no flights to cancel
        {
            if (VerifyUser() == false)
            {
                return;
            }
        }

        Customer.ViewAccountHistory(currentCustomer);
        Console.Write("\nEnter B to go back: ");
        while (true)
        {
            string input = Console.ReadLine();
            if (input == "B")
            {
                break;
            }
            else
            {
                Console.Write("\nInvalid input! Enter B to go back");
            }
        }
    }

    static void ChangePassword()
    {
        Console.WriteLine("\n\n**** CHANGE PASSWORD ****\n");
        while (true)
        {
            Console.Write("Enter your current password: ");
            string currentPassword = User.HashPassword(Console.ReadLine());
            if (currentPassword == currentCustomer.Password)
            {
                Console.Write("Enter your new password: ");
                currentCustomer.Password = User.HashPassword(Console.ReadLine());
                Console.WriteLine("Password changed successfully!");
                Customer.ChangeCustomerPassword(currentCustomer, currentCustomer.Password);
                break;
            }
            else
            {
                Console.WriteLine("\nIncorrect password!");
            }
        }
    }

    static void HomePage()
    {
        int choice;

        while (true)
        {
            Console.WriteLine("\n\n**** HOME PAGE ****");
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
                    ChangePassword();
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

    /// <summary>
    /// Load Engineer Menu section
    /// UI, Create, View, Update, Delete Flight funtions
    /// </summary>
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
            Console.WriteLine("5. View All Aiports");
            Console.WriteLine("6. Log Out");
            Console.Write("Enter your choice: ");
            try
            {
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
                        Airport.DisplayAllAiports();
                        break;
                    case 6:
                        Console.WriteLine("\nLogging out...");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice! Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nInvalid choice! Please try again.");
            }

        }
    }

    static void AddFlight()
    {
        // Get user input for flight details
        Console.WriteLine("**** Add Flight ****");
        Console.WriteLine("Enter your flight details or type 'back' to go back to previous page\n");
        Console.WriteLine("Departure Airport Code: ");
        string departCity = Console.ReadLine().ToUpper();
        if (departCity == "back" || departCity == "BACK" || departCity == "Back" || departCity == "b") { return; }
        Console.WriteLine("Arrival Airport Code: ");
        string arrCity = Console.ReadLine().ToUpper();
        Console.WriteLine("Flight Departure Time: yyyy-mm-dd hh:mm (24-hour)");
        string departTime = Console.ReadLine();

        // loop in case of invalid choice 
        while (true)
        {
            // Confirm input is correct
            Console.WriteLine("\nConfirm the following info is correct (Y/N)");
            Console.WriteLine("Depature Aiport Code: " + departCity);
            Console.WriteLine("Arrival Airport Code: " + arrCity);
            Console.WriteLine("Departure Time: " + departTime);
            string isCorrect = Console.ReadLine().ToUpper();

            if (isCorrect == "Y" || isCorrect == "YES")
            {
                // use Try/Catch incase of invalid airport --- would technically be better to have the check right after the user inputs it
                // but I think that would make the code less organized and theres also the 
                try
                {
                    // get object references 
                    Airport departAirport = Airport.FindAirportbyCode(departCity);
                    Airport arrivalAiport = Airport.FindAirportbyCode(arrCity);
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
                    break;
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
        string flightNumber = Console.ReadLine().ToUpper();
        if (flightNumber == "BACK" || flightNumber == "B") { return; }

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
                    break;
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

    static void UpdateFlight()
    {
        // get flight number of flight to udpate
        Console.WriteLine("**** Update Flight ****");
        Console.WriteLine("Enter flight number or type 'back' to go back to the previous page");
        string flightNumber = Console.ReadLine().ToUpper();
        if (flightNumber == "BACK") { return; } // go back feature 
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
                    "1. Departure Airport Code: {1}\n" +
                    "2. Arrival Airport Code: {2}\n" +
                    "3. Departure Time: {3}\n",
                    flight.FlightNumber,
                    Airport.FindAirportbyId(flight.DepartureAirportID).Code,
                    Airport.FindAirportbyId(flight.ArrivalAirportID).Code,
                    flight.DepartureTime
                );

                // check user input for back option
                string backChoice = Console.ReadLine().ToUpper(); // used for the 'back' check
                if (backChoice == "BACK") { UpdateFlight(); break; }

                // check user input
                int choice = int.Parse(backChoice); // used for the field choice
                // user wants to update departure city
                if (choice == 1)
                {
                    Console.WriteLine("Enter new departure code");
                    string newCity = Console.ReadLine().ToUpper();
                    try
                    {
                        Airport airport = Airport.FindAirportByProperty(newCity);
                        Flight.UpdateFlight(flight, departureAirportID: airport.AirportId);
                        Console.WriteLine("\nFlight Updated\nNotice: The flight number has been updated\nNew flight number: " + flight.FlightNumber);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Airport not found. Available airports shown below");
                        Airport.DisplayAllAiports();
                    }
                }
                // user wants to update arrival city
                else if (choice == 2)
                {
                    Console.WriteLine("Enter new arrival code");
                    string newCity = Console.ReadLine().ToUpper();
                    try
                    {
                        Airport airport = Airport.FindAirportbyCity(newCity);
                        Flight.UpdateFlight(flight, arrivalAirportID: airport.AirportId);
                        Console.WriteLine("\nFlight Updated\nNotice: The flight number has been updated\nNew flight number: " + flight.FlightNumber);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Airport not found. Available airports shown below");
                        Airport.DisplayAllAiports();
                    }

                }
                // user wants to update departure time
                else if (choice == 3)
                {
                    try
                    {
                        Console.WriteLine("Enter new departure time");
                        DateTime newTime = DateTime.Parse(Console.ReadLine());
                        Flight.UpdateFlight(flight, departureTime: newTime);
                        Console.WriteLine("\nFlight Updated");
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Date not formatted correctly");
                    }
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
            Console.WriteLine("2. View All Flights");
            Console.WriteLine("3. View All Airports");
            Console.WriteLine("4. Log Out");
            Console.Write("Enter your choice: ");
            try
            {
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        GenerateFlightManifest();
                        break;
                    case 2:
                        Flight.ShowAllFlights();
                        break;
                    case 3:
                        Airport.DisplayAllAiports();
                        break;
                    case 4:
                        Console.WriteLine("\nLogging out...");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice! Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nInvalid choice! Please try again.");
            }

        }
    }

    static void GenerateFlightManifest()
    {
        Console.WriteLine("\n**** Print Flight Manifest ****");
        Console.WriteLine("Enter flight number or type 'back' to go back");
        string userInput = Console.ReadLine().ToUpper();
        if (userInput == "BACK") { return; }
        try
        {
            Flight flight = Flight.FindFlightByFlightNumber(userInput);
            FlightManager.PrintFlightManifest(flight);

            Console.WriteLine("Save manifest? (Y/N)");
            string isSaving = Console.ReadLine().ToUpper();
            if (isSaving == "Y" || isSaving == "YES")
            {
                FlightManager.SaveFlightManifest(flight);
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nFlight number not found");
            GenerateFlightManifest();
        }
    }

    /// <summary>
    /// Accountant Menu Section
    /// UI, Generate reports on 
    /// </summary>
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
                        try
                        {
                            Console.WriteLine("List of all flights:");
                            Flight.ShowAllFlights();
                            Console.WriteLine("\nEnter flight number or type 'back' or 'b' to go back");
                            string userInput = Console.ReadLine().ToUpper();

                            if (userInput == "BACK" || userInput == "B") 
                            { 
                                AccountantFunctionality(); 
                            }

                            Flight flight = Flight.FindFlightByFlightNumber(userInput);
                            double percentCapacity = Accountant.CalcPercentCapacity(flight);
                            Console.WriteLine("\nPercent Capacity: " + Math.Round(percentCapacity,2)*100 + "%");
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("\nInvalid input format. Please enter a valid flight number.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\nAn error occurred: " + ex.Message);
                        }
                        break;

                    case 2:
                        try
                        {
                            Flight.ShowAllFlights();

                            Console.WriteLine("\nEnter flight number or type 'back' or 'b' to go back");
                            string userInput = Console.ReadLine().ToUpper();

                            if (userInput == "BACK" || userInput == "B")
                            {
                                AccountantFunctionality();
                            }

                            Flight flight2 = Flight.FindFlightByFlightNumber(userInput);

                            double income = Accountant.CalcIncomeFlight(flight2);
                            Console.WriteLine("\nIncome for flight: $" + income);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid input format. Please enter a valid flight number.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                        }
                        break;

                    case 3:
                        try
                        {
                            double totalIncome = Accountant.CalcIncomeWhole();
                            Console.WriteLine("\nIncome of all Flights: " + totalIncome);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                        }
                        break;

                    case 4:
                        try
                        {
                            Console.WriteLine("\nEnter time range (day, month, or year) or type 'back' or 'b' to go back");
                            string userInput = Console.ReadLine().ToUpper();
                            if (userInput == "BACK" || (userInput == "B"))
                            {
                                AccountantFunctionality();
                            }

                            int numFlights = Accountant.CalcNumFlights(userInput);
                            Console.WriteLine("Number of flights this {0}: {1}", userInput.ToLower(), numFlights);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid input format. Please enter a valid time range.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                        }
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

    /// <summary>
    /// Marketing Manager menu section
    /// 
    /// </summary>
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
                            Console.WriteLine("List of flights without assigned planes:\n");
                            Flight.FlightWithNoPlane();
                            Console.WriteLine("\nEnter flight number: ");
                            string flightNumber = Console.ReadLine().ToUpper();

                            Flight flight = Flight.FindFlightByFlightNumber(flightNumber);

                            // Assign plane for flight
                            MarketingManager.AssignPlaneForFlight(flight);
                            Console.WriteLine("\n{0} assigned for flight: {1}",
                                                Plane.FindPlaneById((int)flight.PlaneModelId).Model,
                                                flightNumber);
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
                            MarketingManager.AssignPlaneForAllFlights();
                            Console.WriteLine("Planes successfully assigned for all flights.");
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

    // Helper functions
    static bool RetryCommand(string error, string method) //method to loop another method in case of an exception
    {
        Console.WriteLine("\nInvalid " + error + "!\n  1. Retry " + method + "\n  B. Go back");
        Console.Write("Enter your choice: ");
        while (true)
        {
            string input = Console.ReadLine().ToUpper();
            if (input == "1")
            {
                break;
            }
            else if (input == "B")
            {
                return false;
            }
            else
            {
                Console.Write("\nInvalid input!\n  1. Retry " + method + "\n  B. Go back");
                Console.Write("Enter your choice: ");
            }
        }
        return true;
    }

    static bool VerifyUser()
    {
        Console.WriteLine("\nPlease log in or create an account first!\n  1. Log in\n  2. Create Account\n  B. Go back");
        Console.Write("Enter your choice: ");
        while (true)
        {
            string command = Console.ReadLine();
            if (command == "1")
            {
                LogIn();
                if (loggedIn == true)
                {
                    break;
                }
                Console.WriteLine("\nPlease log in or create an account first!\n  1. Log in\n  2. Create Account\n  B. Go back");
                Console.Write("Enter your choice: ");
            }
            else if (command == "2")
            {
                CreateCustomerAccount();
                if (loggedIn == true)
                {
                    break;
                }
                Console.WriteLine("\nPlease log in or create an account first!\n  1. Log in\n  2. Create Account\n  B. Go back");
                Console.Write("Enter your choice: ");
            }
            else if (command == "B")
            {
                return false;
            }
            else
            {
                Console.WriteLine("\nInvalid input!\n  1. Log in\n  2. Create Account\n  B. Go back");
                Console.Write("Enter your choice: ");
            }
        }
        return true;
    }
}