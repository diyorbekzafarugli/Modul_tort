using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;

namespace TelegramBot.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<CurrencyLog> CurrencyLogs { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}