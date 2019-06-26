using OFXUpload.Database;
using OFXUpload.Database.Entities;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OFXUpload.Repositories
{
  public class FinancialAccountRepository : IFinancialAccountRepository
  {
    public async Task<FinancialAccount> GetFinancialAccount(string bank, string account, string agency = "")
    {
      using (var dbContext = new FinancialContextEntities())
      {
        return await dbContext.FinancialAccounts
                         .Where(x => ((agency == null)
                                     || x.Agency == agency)
                                       && x.Bank.Number == bank
                                       && x.Number == account).FirstOrDefaultAsync();
      }
    }
  }
}