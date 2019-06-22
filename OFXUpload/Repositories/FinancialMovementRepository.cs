using OFXUpload.Database;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OFXUpload.Repositories
{
  public class FinancialMovementRepository : IFinancialMovementRepository
  {
    public async Task<IEnumerable<FinancialAccountMovement>> GetAllMovements()
    {
      using (var dbContext = new FinancialContextEntities())
      {
        return await dbContext.FinancialAccountMovements
                .Include(x => x.FinancialAccountBalance)
                .Include(x => x.FinancialAccountBalance.FinancialAccount)
                .Include(x => x.FinancialAccountBalance.FinancialAccount.Bank)
                .OrderBy(x=> x.FinancialAccountBalance.Id).OrderBy(x => x.DocumentNumber)
                  .ToListAsync();
      }


    }
  }
}