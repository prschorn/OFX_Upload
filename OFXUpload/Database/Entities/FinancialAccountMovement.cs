using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OFXUpload.Database.Entities
{
  public class FinancialAccountMovement
  {
    [Index(IsUnique = true)]
    public int Id { get; set; }
    public int FinancialAccountBalanceId { get; set; }
    public string Description { get; set; }
    public string DocumentNumber { get; set; }
    public decimal Value { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; }

    public virtual FinancialAccountBalance FinancialAccountBalance { get; set; }
  }
}