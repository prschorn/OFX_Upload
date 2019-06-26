using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OFXUpload.Database.Entities
{
  public class FinancialAccountBalance
  {
    [Index(IsUnique =true)]
    public int Id { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime EndDate { get; set; }
    public int FinancialAccountId { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal EndBalance { get; set; }

    public virtual FinancialAccount FinancialAccount { get; set; }


  }
}