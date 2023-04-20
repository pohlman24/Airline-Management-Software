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
                    UserLogIn();
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
                    ChangePassword();
                    break;

                case 7:
                    AdminLogIn();
                    break;

                case 8:
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
        if (password == user.Password)
        {
            Console.WriteLine("\nWelcome, " + user.FirstName + user.LastName + "!");
            HomePage();
        }
        else
        {
            Console.WriteLine("\nInvalid email or password!");
        }
    }

    static void BookFlight()
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
    }
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
                    ChangePassword();
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

    static void DefaultPage()
    {
        Console.WriteLine("\n\n**** AIRLINE SERVICE SYSTEM ****");
        Console.WriteLine("1. Create Account");
        Console.WriteLine("2. Log In");
        Console.WriteLine("3. Book a Flight");
        Console.WriteLine("4. Cancel a Flight");
        Console.WriteLine("5. View Account History");
        Console.WriteLine("6. Change Password");
        Console.WriteLine("7. Admin Log In");
        Console.WriteLine("8. Exit");
        Console.Write("Enter your choice: ");
    }

    static void FlightManagerFunctionality()
    {
        // Code to handle flight manager functionality goes here
        while (true)
        {
            Console.WriteLine("\n\n**** FLIGHT MANAGER FUNCTIONALITY ****");
            Console.WriteLine("1. View All Flights");
            Console.WriteLine("2. Add a New Flight");
            Console.WriteLine("3. Delete a Flight");
            Console.WriteLine("4. Update a Flight");
            Console.WriteLine("5. Log Out");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("\nThis feature is not yet implemented.");
                    break;
                case 2:
                    Console.WriteLine("\nThis feature is not yet implemented.");
                    break;
                case 3:
                    Console.WriteLine("\nThis feature is not yet implemented.");
                    break;
                case 4:
                    Console.WriteLine("\nThis feature is not yet implemented.");
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
                int choice = int.Parse(Console.ReadLine());
                Accountant accountant = new Accountant(id, firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, userType);

                switch (choice)
                {
                    case 1:
                        try
                        {
                            // Calculate the percent capacity for a flight
                            Flight flight = GetFlightById(flightId);
                            double percentCapacity = Accountant.CalcPercentCapacity(flight);
                            Console.WriteLine("Percent Capacity: " + percentCapacity);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while calculating the percent capacity: " + ex.Message);
                        }
                        break;
                    case 2:
                        try
                        {
                            // Calculate the income generated by a flight
                            Flight flight2 = GetFlightById(flightId);
                            double income = Accountant.CalcIncomeFlight(flight2);
                            Console.WriteLine("Income per Flight: " + income);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while calculating the income generated by a flight: " + ex.Message);
                        }
                        break;
                    case 3:
                        try
                        {
                            // Calculate the total income generated by all flights
                            double totalIncome = Accountant.CalcIncomeWhole();
                            Console.WriteLine("Income all Flight: " + totalIncome);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while calculating the total income generated by all flights: " + ex.Message);
                        }
                        break;
                    case 4:
                        try
                        {
                            // Calculate the number of flights scheduled within a time range
                            Console.WriteLine("Enter time range:  (day or month or year)");
                            string range = Console.ReadLine();
                            int numFlights = Accountant.CalcNumFlights(range);
                            Console.WriteLine("Num of flights: " + numFlights);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while calculating the number of flights scheduled within a time range: " + ex.Message);
                        }
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


    static void LoadEngineerFunctionality()
    {
        // Code to handle load engineer functionality goes here
        while (true)
        {
            Console.WriteLine("\n\n**** LOAD ENGINEER FUNCTIONALITY ****");
            Console.WriteLine("1. View Load Report");
            Console.WriteLine("2. Update Load Information");
            Console.WriteLine("3. Log Out");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("\nThis feature is not yet implemented.");
                    break;
                case 2:
                    Console.WriteLine("\nThis feature is not yet implemented.");
                    break;
                case 3:
                    Console.WriteLine("\nLogging out...");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Please try again.");
                    break;
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
                MarketingManager marketingManager = new MarketingManager(id, firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, userType);

                switch (choice)
                {
                    case 1:
                        try
                        {
                            Flight flight = GetFlightById(flightId);
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