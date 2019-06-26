using OFXUpload.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OFXUpload.Database
{
  public class FinancialContextEntities : DbContext
  {

    public FinancialContextEntities(): base("name=FinancialContextEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelbuilder)
    {

    }

    public virtual DbSet<Bank> Banks { get; set; }
    public virtual DbSet<FinancialAccount> FinancialAccounts { get; set; }
    public virtual DbSet<FinancialAccountBalance> FinancialAccountBalances { get; set; }
    public virtual DbSet<FinancialAccountMovement> FinancialAccountMovements { get; set; }
    public virtual DbSet<FinancialAccountTransaction> FinancialAccountTransactions { get; set; }

  }
}