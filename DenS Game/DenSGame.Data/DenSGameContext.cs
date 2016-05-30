using DenSGame.Data.Models;
using System.Data.Entity;

namespace DenSGame.Data
{
    public class DenSGameContext : DbContext
    {
        public DenSGameContext() : base("DenSGameConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                .HasMany(t => t.ChosenBy)
                .WithMany()
                .Map(a_mapping => a_mapping.ToTable("Answer_ChosenBy_User"));
        }
    }
}