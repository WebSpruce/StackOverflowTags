using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Data;

namespace StackOverflowTags.Helper
{
    public static class ResetAutoIncrement
    {
        public static async Task Reset(AppDbContext _context) 
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Tag'";
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
