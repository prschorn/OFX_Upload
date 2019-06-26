namespace OFXUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id, unique: true);
            
            CreateTable(
                "dbo.FinancialAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Agency = c.String(),
                        AgencyDV = c.String(),
                        Number = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        DeletedOn = c.DateTime(),
                        BankId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.FinancialAccountBalances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitialDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        FinancialAccountId = c.Int(nullable: false),
                        InitialBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EndBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinancialAccounts", t => t.FinancialAccountId, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.FinancialAccountId);
            
            CreateTable(
                "dbo.FinancialAccountMovements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinancialAccountBalanceId = c.Int(nullable: false),
                        Description = c.String(),
                        DocumentNumber = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.String(),
                        Date = c.DateTime(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinancialAccountBalances", t => t.FinancialAccountBalanceId, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.FinancialAccountBalanceId);
            
            CreateTable(
                "dbo.FinancialAccountTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionId = c.Int(nullable: false),
                        MovementId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        GrossAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InitiatorTransactionKey = c.String(),
                        AcquirerTransactionKey = c.String(),
                        FinancialAccountMovement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinancialAccountMovements", t => t.FinancialAccountMovement_Id)
                .Index(t => t.Id, unique: true)
                .Index(t => t.FinancialAccountMovement_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinancialAccountTransactions", "FinancialAccountMovement_Id", "dbo.FinancialAccountMovements");
            DropForeignKey("dbo.FinancialAccountMovements", "FinancialAccountBalanceId", "dbo.FinancialAccountBalances");
            DropForeignKey("dbo.FinancialAccountBalances", "FinancialAccountId", "dbo.FinancialAccounts");
            DropForeignKey("dbo.FinancialAccounts", "BankId", "dbo.Banks");
            DropIndex("dbo.FinancialAccountTransactions", new[] { "FinancialAccountMovement_Id" });
            DropIndex("dbo.FinancialAccountTransactions", new[] { "Id" });
            DropIndex("dbo.FinancialAccountMovements", new[] { "FinancialAccountBalanceId" });
            DropIndex("dbo.FinancialAccountMovements", new[] { "Id" });
            DropIndex("dbo.FinancialAccountBalances", new[] { "FinancialAccountId" });
            DropIndex("dbo.FinancialAccountBalances", new[] { "Id" });
            DropIndex("dbo.FinancialAccounts", new[] { "BankId" });
            DropIndex("dbo.FinancialAccounts", new[] { "Id" });
            DropIndex("dbo.Banks", new[] { "Id" });
            DropTable("dbo.FinancialAccountTransactions");
            DropTable("dbo.FinancialAccountMovements");
            DropTable("dbo.FinancialAccountBalances");
            DropTable("dbo.FinancialAccounts");
            DropTable("dbo.Banks");
        }
    }
}
