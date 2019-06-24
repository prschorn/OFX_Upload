using OFXUpload.Database;
using OFXUpload.Models.DTO;
using OFXUpload.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OFXUpload.Models
{
  public class MovementHandler : IMovementHandler
  {

    public List<Installment> GetAccountTransactionsByMovement(OFXParser.Entities.Transaction movement, StoneDTO stoneObject)
    {
      var payment = stoneObject.Payments.Payment.Where(x => Convert.ToDouble(x.TotalAmount) == movement.TransactionValue).FirstOrDefault();

      var transactions = stoneObject.FinancialTransactionsAccounts.Transaction.Where(x => x.Installments.Installment.Any(y => y.PaymentId == payment.Id)).ToList();
      var installments = transactions.Select(x => x.Installments.Installment);
      var returnList = new List<Installment>();
      foreach (Installment[] installment in installments)
      {
        returnList.AddRange(installment);
      }

      return returnList.Distinct().ToList();
    }

  }
}