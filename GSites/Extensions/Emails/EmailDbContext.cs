using Microsoft.EntityFrameworkCore;

namespace GSites.Extensions.Emails;

public class EmailDbContext(DbContextOptions<EmailDbContext> options) : DbContext(options)
{
    public DbSet<Email> Emails { get; set; }

    public DbSet<EmailSettings> Settings { get; set; }
}