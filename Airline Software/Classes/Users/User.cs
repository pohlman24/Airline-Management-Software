using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline_Software;
using CsvHelper.Configuration.Attributes;

namespace Airline_Software
{
    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string UserType { get; set; }

        public User(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.Age = Age;
            this.Address = Address;
            this.City = City;
            this.State = State;
            this.ZipCode = ZipCode;
            this.Password = Password;
            this.UserType = UserType;
        }

        // we should never create a user directly by calling this function
        // this function will get called by other functions such as the CreateCustomer 
        public static User CreateUser(int id, string firstName, string lastName, string email, string phoneNumber, int age, string address, string city, string state, string zipCode, string password, string userType)
        {
            /*id = GenerateId();*/
            User newUser = new User(id, firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, userType);

            string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/Tables/UserDb.csv";
            List<User> users = CsvDatabase.ReadCsvFile<User>(filePath);
            users.Add(newUser);
            CsvDatabase.WriteCsvFile(filePath, users);
            return newUser;
        }

        public static User UpdateUser()
        {
            return null;
        }

        
        public static int GenerateId()
        {
            // read into userDatabase
            string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/Tables/UserDb.csv"; // file path to 'table' (csv file) -- need to make it the relative path but i couldnt figure out how
            List<User> users = CsvDatabase.ReadCsvFile<User>(filePath);
            if (users.Count != 0)
            {
                Random random = new Random();
                int randomNumber;
                do
                {
                    randomNumber = random.Next(100000, 1000000);
                } 
                while (users.Exists(user => user.Id == randomNumber));

                return randomNumber;
            }
            else
            {
                Random random = new Random();
                int randomNumber;
                randomNumber = random.Next(100000, 1000000);
                return randomNumber;
            }
        }
        public void ChangePassword(string newPassword)
        {
            this.Password = newPassword;
        }

    }
}
