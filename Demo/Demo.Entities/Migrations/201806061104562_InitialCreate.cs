namespace Helpa.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowStatus = c.Boolean(nullable: false),
                        GenderId = c.Int(),
                        ProfileImage = c.String(),
                        Description = c.String(maxLength: 500),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        LocationId = c.Int(),
                        AverageRating = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        LocationGeography = c.Geography(),
                        RowStatus = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Url = c.String(maxLength: 50),
                        Title = c.String(maxLength: 50),
                        Message = c.String(maxLength: 500),
                        RowStatus = c.String(maxLength: 1),
                        NotificationState = c.String(maxLength: 1),
                        CreatedDate = c.DateTime(nullable: false),
                        ReadDate = c.DateTime(),
                        NotificationType = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.UserLogs",
                c => new
                    {
                        UserLogId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DeviceId = c.String(),
                        RowStatus = c.String(maxLength: 1),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserLogId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "IMG.Carousels",
                c => new
                    {
                        CarouselId = c.Int(nullable: false, identity: true),
                        ImageId = c.Int(nullable: false),
                        HelperId = c.Int(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CarouselId)
                .ForeignKey("FIL.Files", t => t.ImageId)
                .ForeignKey("USR.Helpers", t => t.HelperId)
                .Index(t => t.ImageId)
                .Index(t => t.HelperId);
            
            CreateTable(
                "FIL.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileFolder = c.String(nullable: false, maxLength: 4000),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                    })
                .PrimaryKey(t => t.FileId);
            
            CreateTable(
                "DOC.Documents",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        HelperId = c.Int(nullable: false),
                        DocumentType = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("USR.Helpers", t => t.HelperId)
                .ForeignKey("FIL.Files", t => t.FileId)
                .Index(t => t.FileId)
                .Index(t => t.HelperId);
            
            CreateTable(
                "USR.Helpers",
                c => new
                    {
                        HelperId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        LocationType = c.Int(),
                        Qualification = c.String(maxLength: 50),
                        Experience = c.Int(),
                        MinAgeGroup = c.Int(),
                        MaxAgeGroup = c.Int(),
                        Description = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HelperId)
                .ForeignKey("LCN.LocationTypes", t => t.LocationType)
                .Index(t => t.LocationType);
            
            CreateTable(
                "NTW.Favourates",
                c => new
                    {
                        FavourateId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        HelperId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FavourateId)
                .ForeignKey("USR.Helpers", t => t.HelperId)
                .Index(t => t.HelperId);
            
            CreateTable(
                "LCN.HelperLocations",
                c => new
                    {
                        HelperLocationId = c.Int(nullable: false, identity: true),
                        HelperId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HelperLocationId)
                .ForeignKey("LCN.Location", t => t.LocationId)
                .ForeignKey("USR.Helpers", t => t.HelperId)
                .Index(t => t.HelperId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "LCN.Location",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Locality = c.String(maxLength: 50),
                        District = c.String(maxLength: 50),
                        LocationName = c.String(nullable: false, maxLength: 50),
                        LocationGeography = c.Geography(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "SVC.HelperServices",
                c => new
                    {
                        HelperServiceId = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        HelperId = c.Int(nullable: false),
                        HourPrice = c.Boolean(nullable: false),
                        MinHourPrice = c.Decimal(storeType: "money"),
                        MaxHourPrice = c.Decimal(storeType: "money"),
                        DayPrice = c.Boolean(nullable: false),
                        MinDayPrice = c.Decimal(storeType: "money"),
                        MaxDayPrice = c.Decimal(storeType: "money"),
                        MonthlyPrice = c.Boolean(nullable: false),
                        MinMonthPrice = c.Decimal(storeType: "money"),
                        MaxMonthPrice = c.Decimal(storeType: "money"),
                        Status = c.Boolean(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HelperServiceId)
                .ForeignKey("SVC.Services", t => t.ServiceId)
                .ForeignKey("USR.Helpers", t => t.HelperId)
                .Index(t => t.ServiceId)
                .Index(t => t.HelperId);
            
            CreateTable(
                "SVC.HelperServiceScope",
                c => new
                    {
                        HelperServiceScopeId = c.Int(nullable: false, identity: true),
                        HelperServiceId = c.Int(nullable: false),
                        HelperScopeId = c.Int(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HelperServiceScopeId)
                .ForeignKey("SVC.Scopes", t => t.HelperScopeId)
                .ForeignKey("SVC.HelperServices", t => t.HelperServiceId)
                .Index(t => t.HelperServiceId)
                .Index(t => t.HelperScopeId);
            
            CreateTable(
                "SVC.Scopes",
                c => new
                    {
                        ScopeId = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        ScopeName = c.String(nullable: false, maxLength: 50),
                        Status = c.Boolean(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ScopeId)
                .ForeignKey("SVC.Services", t => t.ServiceId)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "USR.JobScope",
                c => new
                    {
                        JobScopeId = c.Int(nullable: false, identity: true),
                        JobServiceId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobScopeId)
                .ForeignKey("USR.JobServices", t => t.JobServiceId)
                .ForeignKey("SVC.Scopes", t => t.ScopeId)
                .Index(t => t.JobServiceId)
                .Index(t => t.ScopeId);
            
            CreateTable(
                "USR.JobServices",
                c => new
                    {
                        JobServiceId = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobServiceId)
                .ForeignKey("USR.Jobs", t => t.JobId)
                .ForeignKey("SVC.Services", t => t.ServiceId)
                .Index(t => t.JobId)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "USR.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        CreatedUserId = c.Int(nullable: false),
                        JobTiltle = c.String(nullable: false, maxLength: 50),
                        JobDescription = c.String(nullable: false, unicode: false),
                        JobType = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        HelperType = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        LocationId = c.Int(nullable: false),
                        TimeId = c.Int(nullable: false),
                        PriceId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("LCN.JobLocation", t => t.LocationId)
                .ForeignKey("USR.JobPrice", t => t.PriceId)
                .ForeignKey("USR.JobTime", t => t.TimeId)
                .Index(t => t.LocationId)
                .Index(t => t.TimeId)
                .Index(t => t.PriceId);
            
            CreateTable(
                "LCN.JobLocation",
                c => new
                    {
                        JobLocationId = c.Int(nullable: false, identity: true),
                        JobLocationName = c.String(maxLength: 50),
                        JobLocationGeography = c.Geography(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobLocationId);
            
            CreateTable(
                "USR.JobPrice",
                c => new
                    {
                        JobPriceId = c.Int(nullable: false, identity: true),
                        Hourly = c.Boolean(nullable: false),
                        Daily = c.Boolean(nullable: false),
                        Monthly = c.Boolean(nullable: false),
                        MinPrice = c.Decimal(storeType: "money"),
                        MaxPrice = c.Decimal(storeType: "money"),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobPriceId);
            
            CreateTable(
                "USR.JobTime",
                c => new
                    {
                        JobTimeId = c.Int(nullable: false, identity: true),
                        Sunday = c.Boolean(nullable: false),
                        Monday = c.Boolean(nullable: false),
                        Tuesday = c.Boolean(nullable: false),
                        Wednesday = c.Boolean(nullable: false),
                        Thursday = c.Boolean(nullable: false),
                        Friday = c.Boolean(nullable: false),
                        Saturday = c.Boolean(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobTimeId);
            
            CreateTable(
                "USR.Recievers",
                c => new
                    {
                        ReceiverId = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        ReceiverName = c.String(maxLength: 50),
                        ReceiverGenderId = c.Int(nullable: false),
                        RecieverAge = c.Int(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReceiverId)
                .ForeignKey("USR.Jobs", t => t.JobId)
                .Index(t => t.JobId);
            
            CreateTable(
                "SVC.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(nullable: false, maxLength: 50),
                        Status = c.Boolean(nullable: false),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ServiceId);
            
            CreateTable(
                "LCN.LocationTypes",
                c => new
                    {
                        LocationTypeId = c.Int(nullable: false, identity: true),
                        LocationType = c.String(nullable: false, maxLength: 50),
                        RowStatus = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LocationTypeId);
            
            CreateTable(
                "RTG.ReviewAndRating",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        HelperId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Review = c.String(maxLength: 500),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("USR.Helpers", t => t.HelperId)
                .Index(t => t.HelperId);
            
            CreateTable(
                "RTG.ReviewComments",
                c => new
                    {
                        ReviewCommentId = c.Int(nullable: false, identity: true),
                        ReviewId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ReviewComment = c.String(nullable: false, maxLength: 500),
                        RowStatus = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReviewCommentId)
                .ForeignKey("RTG.ReviewAndRating", t => t.ReviewId)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.StaticPages",
                c => new
                    {
                        StaticPageId = c.Int(nullable: false, identity: true),
                        PageTitle = c.String(maxLength: 255),
                        PageContent = c.String(unicode: false, storeType: "text"),
                        PageType = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StaticPageId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DOC.Documents", "FileId", "FIL.Files");
            DropForeignKey("RTG.ReviewAndRating", "HelperId", "USR.Helpers");
            DropForeignKey("RTG.ReviewComments", "ReviewId", "RTG.ReviewAndRating");
            DropForeignKey("USR.Helpers", "LocationType", "LCN.LocationTypes");
            DropForeignKey("SVC.HelperServices", "HelperId", "USR.Helpers");
            DropForeignKey("SVC.HelperServiceScope", "HelperServiceId", "SVC.HelperServices");
            DropForeignKey("USR.JobScope", "ScopeId", "SVC.Scopes");
            DropForeignKey("SVC.Scopes", "ServiceId", "SVC.Services");
            DropForeignKey("USR.JobServices", "ServiceId", "SVC.Services");
            DropForeignKey("SVC.HelperServices", "ServiceId", "SVC.Services");
            DropForeignKey("USR.JobScope", "JobServiceId", "USR.JobServices");
            DropForeignKey("USR.Recievers", "JobId", "USR.Jobs");
            DropForeignKey("USR.Jobs", "TimeId", "USR.JobTime");
            DropForeignKey("USR.JobServices", "JobId", "USR.Jobs");
            DropForeignKey("USR.Jobs", "PriceId", "USR.JobPrice");
            DropForeignKey("USR.Jobs", "LocationId", "LCN.JobLocation");
            DropForeignKey("SVC.HelperServiceScope", "HelperScopeId", "SVC.Scopes");
            DropForeignKey("LCN.HelperLocations", "HelperId", "USR.Helpers");
            DropForeignKey("LCN.HelperLocations", "LocationId", "LCN.Location");
            DropForeignKey("NTW.Favourates", "HelperId", "USR.Helpers");
            DropForeignKey("DOC.Documents", "HelperId", "USR.Helpers");
            DropForeignKey("IMG.Carousels", "HelperId", "USR.Helpers");
            DropForeignKey("IMG.Carousels", "ImageId", "FIL.Files");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserLogs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "ReceiverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("RTG.ReviewComments", new[] { "ReviewId" });
            DropIndex("RTG.ReviewAndRating", new[] { "HelperId" });
            DropIndex("USR.Recievers", new[] { "JobId" });
            DropIndex("USR.Jobs", new[] { "PriceId" });
            DropIndex("USR.Jobs", new[] { "TimeId" });
            DropIndex("USR.Jobs", new[] { "LocationId" });
            DropIndex("USR.JobServices", new[] { "ServiceId" });
            DropIndex("USR.JobServices", new[] { "JobId" });
            DropIndex("USR.JobScope", new[] { "ScopeId" });
            DropIndex("USR.JobScope", new[] { "JobServiceId" });
            DropIndex("SVC.Scopes", new[] { "ServiceId" });
            DropIndex("SVC.HelperServiceScope", new[] { "HelperScopeId" });
            DropIndex("SVC.HelperServiceScope", new[] { "HelperServiceId" });
            DropIndex("SVC.HelperServices", new[] { "HelperId" });
            DropIndex("SVC.HelperServices", new[] { "ServiceId" });
            DropIndex("LCN.HelperLocations", new[] { "LocationId" });
            DropIndex("LCN.HelperLocations", new[] { "HelperId" });
            DropIndex("NTW.Favourates", new[] { "HelperId" });
            DropIndex("USR.Helpers", new[] { "LocationType" });
            DropIndex("DOC.Documents", new[] { "HelperId" });
            DropIndex("DOC.Documents", new[] { "FileId" });
            DropIndex("IMG.Carousels", new[] { "HelperId" });
            DropIndex("IMG.Carousels", new[] { "ImageId" });
            DropIndex("dbo.UserLogs", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "ReceiverId" });
            DropIndex("dbo.Notifications", new[] { "SenderId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "LocationId" });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.StaticPages");
            DropTable("RTG.ReviewComments");
            DropTable("RTG.ReviewAndRating");
            DropTable("LCN.LocationTypes");
            DropTable("SVC.Services");
            DropTable("USR.Recievers");
            DropTable("USR.JobTime");
            DropTable("USR.JobPrice");
            DropTable("LCN.JobLocation");
            DropTable("USR.Jobs");
            DropTable("USR.JobServices");
            DropTable("USR.JobScope");
            DropTable("SVC.Scopes");
            DropTable("SVC.HelperServiceScope");
            DropTable("SVC.HelperServices");
            DropTable("LCN.Location");
            DropTable("LCN.HelperLocations");
            DropTable("NTW.Favourates");
            DropTable("USR.Helpers");
            DropTable("DOC.Documents");
            DropTable("FIL.Files");
            DropTable("IMG.Carousels");
            DropTable("dbo.UserLogs");
            DropTable("dbo.Notifications");
            DropTable("dbo.Locations");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
        }
    }
}
