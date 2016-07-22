using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExistingDatabase.Models
{
    public partial class StartpageContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=Startpage;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_IdentityRoleClaim<string>_IdentityRole_RoleId");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_IdentityUserClaim<string>_User_UserId");
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_IdentityUserLogin<string>");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_IdentityUserLogin<string>_User_UserId");
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_IdentityUserRole<string>");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_IdentityUserRole<string>_IdentityRole_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_IdentityUserRole<string>_User_UserId");
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<ConfigurationValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Contact_Project_ProjectID");
            });

            modelBuilder.Entity<Databases>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthenticationType).HasDefaultValueSql("0");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Databases)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Database_Project_ProjectID");
            });

            modelBuilder.Entity<EnvironmentDatabase>(entity =>
            {
                entity.HasKey(e => new { e.EnvironmentId, e.DatabaseId })
                    .HasName("PK_EnvironmentDatabase");

                entity.HasIndex(e => e.DatabaseId)
                    .HasName("IX_EnvironmentDatabase_DatabaseID");

                entity.HasIndex(e => e.EnvironmentId)
                    .HasName("IX_EnvironmentDatabase_EnvironmentID");

                entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentID");

                entity.Property(e => e.DatabaseId).HasColumnName("DatabaseID");

                entity.HasOne(d => d.Database)
                    .WithMany(p => p.EnvironmentDatabase)
                    .HasForeignKey(d => d.DatabaseId);

                entity.HasOne(d => d.Environment)
                    .WithMany(p => p.EnvironmentDatabase)
                    .HasForeignKey(d => d.EnvironmentId);
            });

            modelBuilder.Entity<EnvironmentWebpage>(entity =>
            {
                entity.HasKey(e => new { e.EnvironmentId, e.WebpageId })
                    .HasName("PK_EnvironmentWebpage");

                entity.HasIndex(e => e.EnvironmentId)
                    .HasName("IX_EnvironmentWebpage_EnvironmentID");

                entity.HasIndex(e => e.WebpageId)
                    .HasName("IX_EnvironmentWebpage_WebpageID");

                entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentID");

                entity.Property(e => e.WebpageId).HasColumnName("WebpageID");

                entity.HasOne(d => d.Environment)
                    .WithMany(p => p.EnvironmentWebpage)
                    .HasForeignKey(d => d.EnvironmentId);

                entity.HasOne(d => d.Webpage)
                    .WithMany(p => p.EnvironmentWebpage)
                    .HasForeignKey(d => d.WebpageId);
            });

            modelBuilder.Entity<Environments>(entity =>
            {
                entity.HasIndex(e => e.ProjectId)
                    .HasName("IX_Environments_ProjectID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Environments)
                    .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<Files>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_File_Project_ProjectID");
            });

            modelBuilder.Entity<HourLogEntries>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HourLogId).HasColumnName("HourLogID");

                entity.HasOne(d => d.HourLog)
                    .WithMany(p => p.HourLogEntries)
                    .HasForeignKey(d => d.HourLogId)
                    .HasConstraintName("FK_HourLogEntry_HourLog_HourLogID");
            });

            modelBuilder.Entity<HourLogs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<RemoteDesktops>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.RemoteDesktops)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_RemoteDesktop_Project_ProjectID");
            });

            modelBuilder.Entity<WebpageBrowsers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<WebpageTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Webpages>(entity =>
            {
                entity.HasIndex(e => e.WebpageBrowserId)
                    .HasName("IX_Webpages_WebpageBrowserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.WebpageBrowserId).HasColumnName("WebpageBrowserID");

                entity.Property(e => e.WebpageTypeId).HasColumnName("WebpageTypeID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Webpages)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Webpage_Project_ProjectID");

                entity.HasOne(d => d.WebpageBrowser)
                    .WithMany(p => p.Webpages)
                    .HasForeignKey(d => d.WebpageBrowserId);

                entity.HasOne(d => d.WebpageType)
                    .WithMany(p => p.Webpages)
                    .HasForeignKey(d => d.WebpageTypeId)
                    .HasConstraintName("FK_Webpage_WebpageType_WebpageTypeID");
            });
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ConfigurationValues> ConfigurationValues { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<Databases> Databases { get; set; }
        public virtual DbSet<EnvironmentDatabase> EnvironmentDatabase { get; set; }
        public virtual DbSet<EnvironmentWebpage> EnvironmentWebpage { get; set; }
        public virtual DbSet<Environments> Environments { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<HourLogEntries> HourLogEntries { get; set; }
        public virtual DbSet<HourLogs> HourLogs { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<RemoteDesktops> RemoteDesktops { get; set; }
        public virtual DbSet<WebpageBrowsers> WebpageBrowsers { get; set; }
        public virtual DbSet<WebpageTypes> WebpageTypes { get; set; }
        public virtual DbSet<Webpages> Webpages { get; set; }
    }
}