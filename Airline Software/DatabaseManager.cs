using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class DatabaseManager
    {
        private string _connectionString; 

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public int CreateUser(int id, int age, string phoneNumber, string address, string city, string state, string zip, string firstName, string lastName, string password, string creditCardNumber, UserType userType, string email, DateTime dateOfBirth)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Users(UserType, FirstName, LastName, Email, Password, Address, City, State, Zip, PhoneNumber, DateOfBirth) VALUES (@UserTtype, @FirstName, @LastName, @Email, @Password, @Address, @City, @State, @Zip @PhoneNumber, @DateOfBirth); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@State", state);
                    command.Parameters.AddWithValue("@Zip", zip);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);

                    int userId = Convert.ToInt32(command.ExecuteScalar());
                    return userId;
                }
            }
        }
    }
}
