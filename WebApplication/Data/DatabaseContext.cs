using Microsoft.EntityFrameworkCore;

namespace WebApplication.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    
    
}