using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LoadedList> LoadedLists { get; set; }
        public DbSet<OperationsClass> OperationsClasses { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountBalance> AccountsBalances { get; set; }
        public DbSet<AccountGroupBalance> AccountGroupsBalances { get; set; }
        public DbSet<OperationsClassBalance> OperationsClassesBalances { get; set; }
        public DbSet<FullListBalance> FullListsBalances { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}