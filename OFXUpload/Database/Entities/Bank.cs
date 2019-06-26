using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OFXUpload.Database.Entities
{

  public class Bank
  {
    [Index(IsUnique=true)]
    public int Id { get; set; }
    [Required]
    public string Number { get; set; }
    [Required]
    public string Name { get; set; }

    public virtual IList<FinancialAccount> FinancialAccounts { get; set; }
  }
}