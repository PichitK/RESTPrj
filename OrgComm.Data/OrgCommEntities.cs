using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using OrgComm.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;

namespace OrgComm.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class OrgCommEntities : DbContext, IDisposable
    {
        // Flag: Has Dispose already been called? 
        private bool _disposed = false;

        private DbConnection Connection { get; set; }
        
        private OrgCommEntities()
            : base()
        {
        }

        public OrgCommEntities(string connectionString): this(new MySqlConnection(connectionString), false)
        {
        }

        private OrgCommEntities(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
            this.Connection = existingConnection;
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<MemberToken> Tokens { get; set; }
        public DbSet<MemberDevice> Devices { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<OfflineMessage> OfflineMessages { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsContent> NewsContent { get; set; }
        public DbSet<StickerPackage> Stickers { get; set; }
        public DbSet<StickerItem> StickerItems { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Lookup> Lookups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using Fluent API here

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<News>().HasMany(t => t.Contents).WithRequired().HasForeignKey(t => t.NewsId);
            modelBuilder.Entity<News>().HasMany(t => t.Likes).WithRequired().HasForeignKey(t => t.NewsId);
            modelBuilder.Entity<News>().HasMany(t => t.Comments).WithRequired().HasForeignKey(t => t.NewsId);

            modelBuilder.Entity<StickerPackage>().HasMany(t => t.Items).WithRequired().HasForeignKey(t => t.StickerId);
            //modelBuilder.Entity<Member>().HasRequired(t => t.Company).WithRequiredPrincipal();
            //modelBuilder.Entity<Member>().HasRequired(t => t.Department).WithRequiredPrincipal();
            //modelBuilder.Entity<Member>().HasRequired(t => t.Position).WithRequiredPrincipal();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~OrgCommEntities()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return; 

            if (disposing)
            {
                // free managed resources
                if (this.Connection != null)
                {
                    this.Connection.Dispose();
                }
            }

            // free native resources if there are any.

            _disposed = true;
        }
    }
}
