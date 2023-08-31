using AdvanticaAutomationTestServer.Models;
using AdvanticaAutomationTestServer.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdvanticaAutomationTestServer.Infractructure
{
    public class WorkerContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; } = null!;

        public WorkerContext(DbContextOptions<WorkerContext> options) : base(options)
        {
            //Создание базы данных при первом обращении
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worker>().HasData(
                new Worker { Id = 1, FirstName = "Александр", LastName = "Белый", MiddleName = "Геннадьевич", Birthday = 12152000, Sex = Sex.Male, HaveChildren = false },
                new Worker { Id = 2, FirstName = "Дмитрий", LastName = "Давыдов", MiddleName = "Евгеньевич", Birthday = 2571984, Sex = Sex.Male, HaveChildren = true },
                new Worker { Id = 3, FirstName = "Елена", LastName = "Уткина", MiddleName = "Владимировна", Birthday = 3111999, Sex = Sex.Female, HaveChildren = true }
            );
        }
    }
}
