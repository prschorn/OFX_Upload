using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OFXUpload.Database.Entities
{
  public class FinancialAccount
  {
    [Index(IsUnique =true)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Agency { get; set; }
    public string AgencyDV { get; set; }
    [Required]
    public string Number { get; set; }
    [Required]
    public DateTime CreatedOn { get; set; }
    public Nullable<DateTime> DeletedOn { get; set; }
    [Required]
    public int BankId { get; set; }

    public virtual Bank Bank { get; set; }

    public virtual IList<FinancialAccountBalance> FinancialAccountBalances { get; set; }
  }
}