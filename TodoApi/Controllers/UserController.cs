using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using UserDB;

namespace UserDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegister userRegister)
        {
            using (var connection = new SqliteConnection("Data Source=users.db"))
            {
                connection.Open();

                CreateTableIfNotExists(connection);

                var insertUserCommand = connection.CreateCommand();
                insertUserCommand.CommandText = "INSERT INTO Users (name, password, amount) VALUES (@name, @password, @amount)";
                insertUserCommand.Parameters.AddWithValue("@name", userRegister.UserName);
                insertUserCommand.Parameters.AddWithValue("@password", userRegister.Password);

                decimal defaultAmount = 1000m;
                insertUserCommand.Parameters.AddWithValue("@amount", defaultAmount);
                insertUserCommand.ExecuteNonQuery();

                connection.Close();
            }

            return Ok("User created successfully!");
        }

        private static void CreateTableIfNotExists(SqliteConnection connection)
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, name TEXT NOT NULL, password TEXT NOT NULL, amount REAL NOT NULL)";
            createTableCommand.ExecuteNonQuery();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            using (var connection = new SqliteConnection("Data Source=users.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE name = @name AND password = @password";
                command.Parameters.AddWithValue("@name", userLogin.UserName);
                command.Parameters.AddWithValue("@password", userLogin.Password);
                int userCount = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                if (userCount > 0)
                {
                    return Ok("Login successful!");
                }
                else
                {
                    return BadRequest("Incorrect password or username");
                }
            }
        }

        [HttpGet("getmoney")]
        public IActionResult GetMoney([FromQuery] string userName)
        {
            using (var connection = new SqliteConnection("Data Source=users.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT amount FROM Users WHERE name = @name";
                command.Parameters.AddWithValue("@name", userName);
                string? money = command.ExecuteScalar()?.ToString();

                connection.Close();

                return Ok(money);
            }
        }
    }
}