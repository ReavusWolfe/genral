namespace ReavusWolfe.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeopleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        HourlyRate = c.Decimal(precision: 18, scale: 2),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Type = c.String(),
                        OneOffDueDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.PeopleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PeopleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CurrentFunds = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Moneys", "PeopleId", c => c.Int());
            AddColumn("dbo.Moneys", "CompanyId", c => c.Int());
            AddColumn("dbo.Moneys", "InitialAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Moneys", "AmountLeft", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Moneys", "IsPaidOff", c => c.Boolean(nullable: false));
            AddColumn("dbo.Moneys", "DueDate", c => c.DateTime());
            CreateIndex("dbo.Moneys", "PeopleId");
            CreateIndex("dbo.Moneys", "CompanyId");
            AddForeignKey("dbo.Moneys", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Moneys", "PeopleId", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moneys", "PeopleId", "dbo.People");
            DropForeignKey("dbo.Incomes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Incomes", "PeopleId", "dbo.People");
            DropForeignKey("dbo.Moneys", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Incomes", new[] { "UserId" });
            DropIndex("dbo.Incomes", new[] { "PeopleId" });
            DropIndex("dbo.Moneys", new[] { "CompanyId" });
            DropIndex("dbo.Moneys", new[] { "PeopleId" });
            DropColumn("dbo.Moneys", "DueDate");
            DropColumn("dbo.Moneys", "IsPaidOff");
            DropColumn("dbo.Moneys", "AmountLeft");
            DropColumn("dbo.Moneys", "InitialAmount");
            DropColumn("dbo.Moneys", "CompanyId");
            DropColumn("dbo.Moneys", "PeopleId");
            DropTable("dbo.Users");
            DropTable("dbo.Incomes");
            DropTable("dbo.People");
            DropTable("dbo.Companies");
        }
    }
}
