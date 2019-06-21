using OFXParser.Entities;
using OFXUpload.Database;
using OFXUpload.Models.Interfaces;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OFXUpload.Models
{
  public class FinancialMovements : IFinancialMovements
  {
    private readonly IFinancialAccountRepository financialAccountRepository;
    public FinancialMovements(IFinancialAccountRepository financialAccountRepository)
    {
      this.financialAccountRepository = financialAccountRepository;
    }
    public async Task<string> SaveOFXInformation(Extract extractedFile)
    {
      using (var dbContext = new FinancialContextEntities())
      {
        try
        {
          dbContext.Database.BeginTransaction();

          //VERIFY IS THERE IS AN ACCOUNT ON DB WITH THE INFORMARTION PASSED THROUGH THE OFX FILE
          var financialAccount = await this.financialAccountRepository.GetFinancialAccount(extractedFile.BankAccount.Bank.Code.ToString(),
                                                                                           extractedFile.BankAccount.AccountCode,
                                                                                           extractedFile.BankAccount.AgencyCode);

          if (financialAccount == null)
            throw new Exception("Conta informada não encontrada, por favor verifique os dados.");

          var initialBalance = extractedFile.FinalBalance -  extractedFile.Transactions.Sum(x => x.TransactionValue);

          var financialBalance = new FinancialAccountBalance
          {
            InitialDate = extractedFile.InitialDate,
            FinancialAccountId = financialAccount.Id,
            EndDate = extractedFile.FinalDate,
            EndBalance =Convert.ToDecimal(extractedFile.FinalBalance),
            InitialBalance =Convert.ToDecimal(initialBalance)
          };
          //Add balance
          dbContext.FinancialAccountBalances.Add(financialBalance);
          await dbContext.SaveChangesAsync();

          //start adding movements
          foreach (var item in extractedFile.Transactions)
          {
            var value = Convert.ToDecimal(item.TransactionValue);
            var movement = new FinancialAccountMovement
            {
              FinancialAccountBalanceId = financialBalance.Id,
              Description = item.Description,
              Type = (item.Type == "CREDIT" ? "C" : "D"),
              Value = value,
              Date = item.Date,
              DocumentNumber = item.Checksum.ToString(),
            };
            dbContext.FinancialAccountMovements.Add(movement);
          }
          await dbContext.SaveChangesAsync();
          dbContext.Database.CurrentTransaction.Commit();
          return "OK";
        }
        catch (Exception)
        {
          dbContext.Database.CurrentTransaction.Rollback();
          throw;
        }
      }
    }

  }
}