using Airline_Software;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "AirlineMangement";
        DatabaseManager dbManager = new DatabaseManager(connectionString);

        using (SqlConnection connection = dbManager.GetConnection())
        {
            dbManager.CreateUser(1, 23, "419", "5934 porsha", "Sylvanai", "ohio", "43560", "Reece", "Pohlman", "testpass", "123345566", 0, "reecepohlman@gmail.com", DateTime.Now);
        }

        /*Console.WriteLine("1. View Available flights \t 2. View Your Upcoming Flights \t 3. Order History \t 4. Account Settings \t 5. Login/SignUp ");
        string homeScreenInput = Console.ReadLine();
        if(homeScreenInput == "1")
        {
            Console.WriteLine("Viewing Flights");
        }
        else if (homeScreenInput == "2")
        {
        }
        else if (homeScreenInput == "3")
        {
        }
        else if (homeScreenInput == "4")
        {
        }
        else if (homeScreenInput == "5")
        {
            Console.WriteLine("Enter 0 to Sign up Or \nEnter UserID: ");
            string userName = Console.ReadLine();
            if(userName == "0")
            {
                Console.WriteLine("Enter Your Name");
                string name = Console.ReadLine();
                Console.WriteLine("Enter Your Age");
                string age = Console.ReadLine();
                Console.WriteLine("Enter Your Home Address");
                string addresss = Console.ReadLine();
                Console.WriteLine("Enter Your Phone Number");
                string phone = Console.ReadLine();
                Console.WriteLine("Enter Your password");
                string password = Console.ReadLine();
                Console.WriteLine("Confirm password");
                string confirmPwd = Console.ReadLine();
                if(password != confirmPwd)
                {
                    Console.WriteLine("Passwords do not much");
                }
            }
            else
            {
                Console.WriteLine("UserID: " + userName);
                Console.WriteLine("Enter Your Password");
                string password = Console.ReadLine();
            }
        }*/
    }
}