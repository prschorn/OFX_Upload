using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OFXUpload.Database.Entities
{
  public class FinancialAccountTransaction
  {
    [Index(IsUnique =true)]
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int MovementId { get; set; }
    public DateTime Date { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal PaymentAmount { get; set; }
    public string InitiatorTransactionKey { get; set; }
    public string AcquirerTransactionKey { get; set; }

    public virtual FinancialAccountMovement FinancialAccountMovement { get; set; }
  }
}