using Newtonsoft.Json;
using System.Collections.Generic;

namespace OFXUpload.Models.DTO
{
  public class StoneDTO
  {
    public Header Header { get; set; } = new Header();
    public Financialtransactions FinancialTransactions { get; set; } = new Financialtransactions();
    public Financialtransactionsaccounts FinancialTransactionsAccounts { get; set; } = new Financialtransactionsaccounts();

    public Payments Payments { get; set; }

  }

  public class Conciliation
  {
    public object FinancialEvents { get; set; }
    public Financialtransactionsaccounts FinancialTransactionsAccounts { get; set; }
    public object FinancialEventAccounts { get; set; }
    public Payments Payments { get; set; }
    public Trailer Trailer { get; set; }
  }

  public class Header
  {
    public string GenerationDateTime { get; set; }
    public string StoneCode { get; set; }
    public string LayoutVersion { get; set; }
    public string FileId { get; set; }
    public string ReferenceDate { get; set; }
  }


  public class Financialtransactions
  {
    public Transaction[] Transaction { get; set; }
  }

  public class Transaction
  {
    public Events Events { get; set; }
    public string AcquirerTransactionKey { get; set; }
    public string InitiatorTransactionKey { get; set; }
    public string AuthorizationDateTime { get; set; }
    public string CaptureLocalDateTime { get; set; }
    public string International { get; set; }
    public string AccountType { get; set; }
    public string InstallmentType { get; set; }
    public string NumberOfInstallments { get; set; }
    public string AuthorizedAmount { get; set; }
    public string CapturedAmount { get; set; }
    public string AuthorizationCurrencyCode { get; set; }
    public string IssuerAuthorizationCode { get; set; }
    public string BrandId { get; set; }
    public string CardNumber { get; set; }
    public Poi Poi { get; set; }
    public string EntryMode { get; set; }
    public Installments Installments { get; set; }
    public Cancellations Cancellations { get; set; }
  }

  public class Events
  {
    public string CancellationCharges { get; set; }
    public string Cancellations { get; set; }
    public string Captures { get; set; }
    public string ChargebackRefunds { get; set; }
    public string Chargebacks { get; set; }
    public string Payments { get; set; }
  }

  public class Poi
  {
    public string PoiType { get; set; }
  }

  public class Installments
  {
    public Installment[] Installment { get; set; }
  }
  public class Installment
  {
    public string InstallmentNumber { get; set; }
    public string GrossAmount { get; set; }
    public string NetAmount { get; set; }
    public string PrevisionPaymentDate { get; set; }
    public string PaymentDate { get; set; }
    public string PaymentId { get; set; }
    public string AcquirerTransactionKey { get; set; }
    public string InitiatorTransactionKey { get; set; }

  }

  public class Financialtransactionsaccounts
  {
    public Transaction[] Transaction { get; set; }
  }

  public class FinancialTransacion
  {
    public Events Events { get; set; }
    public string AcquirerTransactionKey { get; set; }
    public string InitiatorTransactionKey { get; set; }
    public string AuthorizationDateTime { get; set; }
    public string CaptureLocalDateTime { get; set; }
    public Poi Poi { get; set; }
    public string EntryMode { get; set; }
    public Installments Installments { get; set; }
    public Cancellations Cancellations { get; set; }
  }


  public class Cancellations
  {
    public Cancellation Cancellation { get; set; }
  }

  public class Cancellation
  {
    public string OperationKey { get; set; }
    public string CancellationDateTime { get; set; }
    public string ReturnedAmount { get; set; }
    public Billing Billing { get; set; }
    public string PaymentId { get; set; }
  }

  public class Billing
  {
    public string ChargedAmount { get; set; }
    public string ChargeDate { get; set; }
  }

  public class Payments
  {
    public Payment[] Payment { get; set; }
  }

  public class Payment
  {
    public string Id { get; set; }
    public string WalletTypeId { get; set; }
    public string TotalAmount { get; set; }
    public string TotalFinancialAccountsAmount { get; set; }
    public string LastNegativeAmount { get; set; }
    public Favoredbankaccount FavoredBankAccount { get; set; }
  }

  public class Favoredbankaccount
  {
    public string BankCode { get; set; }
    public string BankBranch { get; set; }
    public string BankAccountNumber { get; set; }
  }

  public class Trailer
  {
    public string CapturedTransactionsQuantity { get; set; }
    public string CanceledTransactionsQuantity { get; set; }
    public string PaidInstallmentsQuantity { get; set; }
    public string ChargedCancellationsQuantity { get; set; }
    public string ChargebacksQuantity { get; set; }
    public string ChargebacksRefundQuantity { get; set; }
    public string ChargedChargebacksQuantity { get; set; }
    public string PaidChargebacksRefundQuantity { get; set; }
    public string PaidEventsQuantity { get; set; }
    public string ChargedEventsQuantity { get; set; }
  }
}