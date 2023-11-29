using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using GameStats; // Assuming this namespace contains the GameStatistics model

namespace GameStats.Controllers // Corrected namespace name
{
    [ApiController]
    [Route("[controller]")]
    public class GameStatisticsController : ControllerBase
    {
        [HttpPost("result")]
        public IActionResult Result([FromBody] GameStatistics gameStatistics)
        {
            using (var connection = new SqliteConnection("Data Source=games.db"))
            {
                connection.Open();

                CreateTableIfNotExists(connection);

                var insertGameCommand = connection.CreateCommand();
                insertGameCommand.CommandText = "INSERT INTO GameStatistics (PlayerScore, DealerScore, GameResult) VALUES (@PlayerScore, @DealerScore, @GameResult)";
                insertGameCommand.Parameters.Add("@PlayerScore", SqliteType.Integer).Value = gameStatistics.PlayerScore;
                insertGameCommand.Parameters.Add("@DealerScore", SqliteType.Integer).Value = gameStatistics.DealerScore;
                insertGameCommand.Parameters.Add("@GameResult", SqliteType.Text).Value = gameStatistics.GameResult;
                insertGameCommand.ExecuteNonQuery();

                connection.Close();
            }

            return Ok("Game statistics saved successfully!");
        }

        public static void CreateTableIfNotExists(SqliteConnection connection)
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS GameStatistics (Id INTEGER PRIMARY KEY, PlayerScore INTEGER NOT NULL, DealerScore INTEGER NOT NULL, GameResult TEXT NOT NULL)";
            createTableCommand.ExecuteNonQuery();
        }
    }
}
