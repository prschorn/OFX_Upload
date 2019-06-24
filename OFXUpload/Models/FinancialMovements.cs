using OFXParser.Entities;
using OFXUpload.Database;
using OFXUpload.Models.Interfaces;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OFXUpload.Models
{
  public class FinancialMovements : IFinancialMovements
  {
    private readonly IFinancialAccountRepository financialAccountRepository;
    private readonly IStoneRepository stoneRepository;
    private readonly IMovementHandler movementHandler;
    public FinancialMovements(IFinancialAccountRepository financialAccountRepository, IStoneRepository stoneRepository, IMovementHandler movementHandler)
    {
      this.financialAccountRepository = financialAccountRepository;
      this.stoneRepository = stoneRepository;
      this.movementHandler = movementHandler;
    }
    /// <summary>
    /// Handle the information of a parsed OFX file and return a list of documents that couldn't be imported due to already being imported for that bank account and date.
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> SaveOFXInformation(Extract extractedFile)
    {
      var importedDocuments = new List<string>();
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

          //Validate already imported documents for this account and date
          var importedTransactions = extractedFile.Transactions.Where(x => dbContext.FinancialAccountMovements.Any(y => y.DocumentNumber == x.Id
                                                                                                                        && x.Date == y.Date
                                                                                                                        && y.FinancialAccountBalance.FinancialAccount.Number == extractedFile.BankAccount.AccountCode)).ToList();


          foreach (var transaction in importedTransactions)
          {
            //REMOVE ALREADY IMPORTED TRANSACTION FROM THE EXTRACTED FILE
            extractedFile.Transactions.Remove(transaction);
            //ADD DOCUMENT NUMBER TO A LIST SO IT CAN BE SHOWED TO THE USER
            importedDocuments.Add(transaction.Id);
          }

          if (extractedFile.Transactions.Count == 0)
            return importedDocuments;
          //CALCULATE THE INITIAL BALANCE BASED IN THE FINAL BALANCE - ALL THE TRANSACTIONS ( MINUS THE ALREADY IMPORTED ONE )
          var initialBalance = extractedFile.FinalBalance - extractedFile.Transactions.Sum(x => x.TransactionValue);

          var financialBalance = new FinancialAccountBalance
          {
            InitialDate = extractedFile.InitialDate,
            FinancialAccountId = financialAccount.Id,
            EndDate = extractedFile.FinalDate,
            EndBalance = Convert.ToDecimal(extractedFile.FinalBalance),
            InitialBalance = Convert.ToDecimal(initialBalance)
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
              DocumentNumber = item.Id,
            };
            dbContext.FinancialAccountMovements.Add(movement);
            await dbContext.SaveChangesAsync();

            //Verify if it's a stone payment
            if (item.Description.ToLower().Contains("stone"))
            {
              var stoneObject = await this.stoneRepository.GetTransaction(new DTO.StoneRequestData { Date = item.Date });

              var installments = this.movementHandler.GetAccountTransactionsByMovement(item, stoneObject);

              foreach (var installment in installments)
              {
                var FATransaction = new FinancialAccountTransaction
                {
                  MovementId = movement.Id,
                  TransactionId = Convert.ToInt32(installment.PaymentId),
                  Date = installment.PaymentDate.ConvertToDateTime(),
                  GrossAmount = Convert.ToDecimal(installment.GrossAmount),
                  PaymentAmount = Convert.ToDecimal(installment.NetAmount),
                  InitiatorTransactionKey = installment.InitiatorTransactionKey,
                  AcquirerTransactionKey = installment.AcquirerTransactionKey
                };
                dbContext.FinancialAccountTransactions.Add(FATransaction);
              }
              await dbContext.SaveChangesAsync();
            }
          }
          await dbContext.SaveChangesAsync();
          dbContext.Database.CurrentTransaction.Commit();
          return importedDocuments;
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