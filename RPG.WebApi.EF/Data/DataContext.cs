using Microsoft.EntityFrameworkCore;
using RPG.WebApi.EF.Models;

namespace RPG.WebApi.EF.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Character> Characters { get; set; }
    }
}
