namespace OFXUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetransactionmovementfk : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinancialAccountTransactions", "FinancialAccountMovement_Id", "dbo.FinancialAccountMovements");
            DropIndex("dbo.FinancialAccountTransactions", new[] { "FinancialAccountMovement_Id" });
            RenameColumn(table: "dbo.FinancialAccountTransactions", name: "FinancialAccountMovement_Id", newName: "FinancialAccountMovementId");
            AlterColumn("dbo.FinancialAccountTransactions", "FinancialAccountMovementId", c => c.Int(nullable: false));
            CreateIndex("dbo.FinancialAccountTransactions", "FinancialAccountMovementId");
            AddForeignKey("dbo.FinancialAccountTransactions", "FinancialAccountMovementId", "dbo.FinancialAccountMovements", "Id", cascadeDelete: true);
            DropColumn("dbo.FinancialAccountTransactions", "MovementId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FinancialAccountTransactions", "MovementId", c => c.Int(nullable: false));
            DropForeignKey("dbo.FinancialAccountTransactions", "FinancialAccountMovementId", "dbo.FinancialAccountMovements");
            DropIndex("dbo.FinancialAccountTransactions", new[] { "FinancialAccountMovementId" });
            AlterColumn("dbo.FinancialAccountTransactions", "FinancialAccountMovementId", c => c.Int());
            RenameColumn(table: "dbo.FinancialAccountTransactions", name: "FinancialAccountMovementId", newName: "FinancialAccountMovement_Id");
            CreateIndex("dbo.FinancialAccountTransactions", "FinancialAccountMovement_Id");
            AddForeignKey("dbo.FinancialAccountTransactions", "FinancialAccountMovement_Id", "dbo.FinancialAccountMovements", "Id");
        }
    }
}
