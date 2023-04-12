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

            string filePath = @"..\..\..\Tables\UserDb.csv";
            List<User> users = CsvDatabase.ReadCsvFile<User>(filePath);
            users.Add(newUser);
            CsvDatabase.WriteCsvFile(filePath, users);
            return newUser;
        }

        public static void UpdateUser(User user, string firstName = "", string lastName = "", string email = "", string phoneNumber = "", int age = -1, string address = "", string city = "", string state = "", string zipCode = "", string password = "", string userType = "")
        {
            //update user info if changed
            user.FirstName = string.IsNullOrEmpty(firstName) ? user.FirstName : firstName;
            user.LastName = string.IsNullOrEmpty(lastName) ? user.LastName : lastName;
            user.Email = string.IsNullOrEmpty(email) ? user.Email : email;
            user.PhoneNumber = string.IsNullOrEmpty(phoneNumber) ? user.PhoneNumber : phoneNumber;
            user.Age = age == -1 ? user.Age : age;
            user.Address = string.IsNullOrEmpty(address) ? user.Address : address;
            user.City = string.IsNullOrEmpty(city) ? user.City : city;
            user.State = string.IsNullOrEmpty(state) ? user.State : state;
            user.ZipCode = string.IsNullOrEmpty(zipCode) ? user.ZipCode : zipCode;
            user.Password = string.IsNullOrEmpty(password) ? user.Password : password;
            user.UserType = string.IsNullOrEmpty(userType) ? user.UserType : userType;

            //get csv file to update
            string filePath = @"..\..\..\Tables\UserDb.csv";
            List<User> users = CsvDatabase.ReadCsvFile<User>(filePath);

            //update customer info in csv file
            CsvDatabase.UpdateRecord(users, p => p.Id, user.Id, (current, updated) =>
            {
                current.FirstName = updated.FirstName;
                current.LastName = updated.LastName;
                current.Email = updated.Email;
                current.PhoneNumber = updated.PhoneNumber;
                current.Age = updated.Age;
                current.Address = updated.Address;
                current.City = updated.City;
                current.State = updated.State;
                current.ZipCode = updated.ZipCode;
                current.Password = updated.Password;
                current.UserType = updated.UserType;
            }, user);

            CsvDatabase.WriteCsvFile(filePath, users);
        }

        public static int GenerateId()
        {
            // read into userDatabase
            string filePath = @"C:..\..\..\Tables\UserDb.csv"; // file path to 'table' (csv file) -- need to make it the relative path but i couldnt figure out how
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
