using System;

struct User
{
    public string name;
    public string email;
    public string password;
}

struct Flight
{
    public string origin;
    public string destination;
    public string date;
    public int num_passengers;
}

class Program
{
    static User user = new User();
    static Flight flight = new Flight();

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
        user.name = Console.ReadLine();
        Console.Write("Enter your email: ");
        user.email = Console.ReadLine();
        Console.Write("Create a password: ");
        user.password = Console.ReadLine();
        Console.WriteLine("\nAccount created successfully!");
    }

    static void LogIn()
{
    Console.WriteLine("\n\n**** LOG IN ****");
    Console.Write("\nEnter your email: ");
    string email = Console.ReadLine();
    Console.Write("Enter your password: ");
    string password = Console.ReadLine();
    if (email == user.email && password == user.password)
    {
        Console.WriteLine("\nWelcome, " + user.name + "!");
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

static void AdminLogIn()
{
    Console.WriteLine("\n\n**** ADMIN LOG IN ****");
    Console.Write("Enter your admin ID: ");
    string adminId = Console.ReadLine();
    Console.Write("Enter your password: ");
    string password = Console.ReadLine();

    if (adminId == "flightmanager" && password == "password1")
    {
        FlightManagerFunctionality();
    }
    else if (adminId == "accountant" && password == "password2")
    {
        AccountantFunctionality();
    }
    else if (adminId == "loadengineer" && password == "password3")
    {
        LoadEngineerFunctionality();
    }
    else if (adminId == "marketingmanager" && password == "password4")
    {
        MarketingManagerFunctionality();
    }
    else
    {
        Console.WriteLine("\nInvalid admin ID or password!");
    }
}

static void UserLogIn()
{

    Console.WriteLine("\n\n**** LOG IN ****");
    Console.Write("\nEnter your email: ");
    string email = Console.ReadLine();
    Console.Write("Enter your password: ");
    string password = Console.ReadLine();

    bool userExists = false;
    if (email == user.email && password == user.password)
    {
        Console.WriteLine("\nWelcome, " + user.name + "!");
        HomePage();
        userExists = true;
    }
    else
    {
        Console.WriteLine("\nInvalid email or password!");
    }

    if (!userExists)
    {
        Console.WriteLine("\nInvalid email or password!");
    }
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
        Console.WriteLine("\n\n**** ACCOUNTANT FUNCTIONALITY ****");
        Console.WriteLine("1. View Financial Report");
        Console.WriteLine("2. Generate Invoice");
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
        // Code to handle marketing manager functionality goes here
        Console.WriteLine("\n\n**** MARKETING MANAGER FUNCTIONALITY ****");
        Console.WriteLine("1. View Marketing Report");
        Console.WriteLine("2. Launch Marketing Campaign");
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


}
