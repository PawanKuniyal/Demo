namespace Helpa.Api
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HelpaContext : DbContext
    {
        public HelpaContext()
            : base("name=HelpaContext")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<StaticPage> StaticPages { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Carousel> Carousels { get; set; }
        public virtual DbSet<HelperLocation> HelperLocations { get; set; }
        public virtual DbSet<JobLocation> JobLocations { get; set; }
        public virtual DbSet<Location1> Locations1 { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<Favourate> Favourates { get; set; }
        public virtual DbSet<ReviewAndRating> ReviewAndRatings { get; set; }
        public virtual DbSet<ReviewComment> ReviewComments { get; set; }
        public virtual DbSet<HelperService> HelperServices { get; set; }
        public virtual DbSet<HelperServiceScope> HelperServiceScopes { get; set; }
        public virtual DbSet<Scope> Scopes { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Helper> Helpers { get; set; }
        public virtual DbSet<JobPrice> JobPrices { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobScope> JobScopes { get; set; }
        public virtual DbSet<JobService> JobServices { get; set; }
        public virtual DbSet<JobTime> JobTimes { get; set; }
        public virtual DbSet<Reciever> Recievers { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Notifications)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.ReceiverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Notifications1)
                .WithRequired(e => e.AspNetUser1)
                .HasForeignKey(e => e.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.UserLogs)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StaticPage>()
                .Property(e => e.PageContent)
                .IsUnicode(false);

            modelBuilder.Entity<StaticPage>()
                .Property(e => e.PageType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.DocumentType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<File>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<File>()
                .HasMany(e => e.Documents)
                .WithRequired(e => e.File)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<File>()
                .HasMany(e => e.Carousels)
                .WithRequired(e => e.File)
                .HasForeignKey(e => e.ImageId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Carousel>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HelperLocation>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<JobLocation>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<JobLocation>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.JobLocation)
                .HasForeignKey(e => e.LocationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location1>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Location1>()
                .HasMany(e => e.HelperLocations)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocationType>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LocationType>()
                .HasMany(e => e.Helpers)
                .WithOptional(e => e.LocationType1)
                .HasForeignKey(e => e.LocationType);

            modelBuilder.Entity<Favourate>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ReviewAndRating>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ReviewAndRating>()
                .HasMany(e => e.ReviewComments)
                .WithRequired(e => e.ReviewAndRating)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReviewComment>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HelperService>()
                .Property(e => e.MinHourPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HelperService>()
                .Property(e => e.MaxHourPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HelperService>()
                .Property(e => e.MinDayPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HelperService>()
                .Property(e => e.MaxDayPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HelperService>()
                .Property(e => e.MinMonthPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HelperService>()
                .Property(e => e.MaxMonthPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HelperService>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HelperService>()
                .HasMany(e => e.HelperServiceScopes)
                .WithRequired(e => e.HelperService)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HelperServiceScope>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Scope>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Scope>()
                .HasMany(e => e.HelperServiceScopes)
                .WithRequired(e => e.Scope)
                .HasForeignKey(e => e.HelperScopeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Scope>()
                .HasMany(e => e.JobScopes)
                .WithRequired(e => e.Scope)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.HelperServices)
                .WithRequired(e => e.Service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Scopes)
                .WithRequired(e => e.Service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.JobServices)
                .WithRequired(e => e.Service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Helper>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Helper>()
                .HasMany(e => e.Documents)
                .WithRequired(e => e.Helper)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Helper>()
                .HasMany(e => e.Carousels)
                .WithRequired(e => e.Helper)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Helper>()
                .HasMany(e => e.HelperLocations)
                .WithRequired(e => e.Helper)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Helper>()
                .HasMany(e => e.Favourates)
                .WithRequired(e => e.Helper)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Helper>()
                .HasMany(e => e.ReviewAndRatings)
                .WithRequired(e => e.Helper)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Helper>()
                .HasMany(e => e.HelperServices)
                .WithRequired(e => e.Helper)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<JobPrice>()
                .Property(e => e.MinPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<JobPrice>()
                .Property(e => e.MaxPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<JobPrice>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<JobPrice>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.JobPrice)
                .HasForeignKey(e => e.PriceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.JobDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.JobType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.HelperType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.JobServices)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.Recievers)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<JobScope>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<JobService>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<JobService>()
                .HasMany(e => e.JobScopes)
                .WithRequired(e => e.JobService)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<JobTime>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.JobTime)
                .HasForeignKey(e => e.TimeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reciever>()
                .Property(e => e.RowStatus)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
