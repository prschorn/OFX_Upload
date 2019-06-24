using OFXUpload.Models.DTO;
using OFXUpload.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace OFXUpload.Models
{
  public class XmlHandler : IXmlHandler
  {
    public Financialtransactionsaccounts ExtractFinancialAccount(XmlDocument doc)
    {
      var accounts = doc.GetElementsByTagName("FinancialTransactionsAccounts")[0];
      var transactions = new List<Transaction>();
      var TransactionSerializer = new XmlSerializer(typeof(Transaction));
      var InstallmentSerializer = new XmlSerializer(typeof(Installment));

      foreach (XmlNode transactionxml in accounts.ChildNodes)
      {
        var reader = new StringReader(transactionxml.OuterXml);
        var transaction = (Transaction)TransactionSerializer.Deserialize(reader);
        var installmentsXml = transactionxml.SelectNodes("Installments")[0].SelectNodes("Installment");
        var installments = new List<Installment>();

        foreach (XmlNode installmentXml in installmentsXml)
        {
          reader = new StringReader(installmentXml.OuterXml);
          var installment = (Installment)InstallmentSerializer.Deserialize(reader);
          installment.AcquirerTransactionKey = transaction.AcquirerTransactionKey;
          installment.InitiatorTransactionKey = transaction.InitiatorTransactionKey;
          installments.Add(installment);
        }
        transaction.Installments = new Installments
        {
          Installment = installments.ToArray()
        };
        transactions.Add(transaction);
      }

      return new Financialtransactionsaccounts
      {
        Transaction = transactions.ToArray()
      };

    }

    public List<Transaction> ExtractFinancialTransactions(XmlDocument doc)
    {
      //Get financial transactions
      var financialTransactions = doc.GetElementsByTagName("FinancialTransactions")[0];
      var transactions = new List<Transaction>();
      var TransactionSerializer = new XmlSerializer(typeof(Transaction));
      var InstallmentSerializer = new XmlSerializer(typeof(Installment));

      foreach (XmlNode item in financialTransactions.ChildNodes)
      {
        //read the xml as string
        var rdr = new StringReader(item.OuterXml);
        //deserialize transaction into object
        var transaction = (Transaction)TransactionSerializer.Deserialize(rdr);
        //get installments xml ( serializer can't serialize properly due to inconsitency on array / object return
        var installmentsXml = item.SelectNodes("Installments")[0].SelectNodes("Installment");
        var installments = new List<Installment>();
        foreach (XmlNode installmentXml in installmentsXml)
        {
          //initialize serializer for installment
          //read installment xml as string
          rdr = new StringReader(installmentXml.OuterXml);
          //deserialize into object and add into the list ( always treating installments as list
          var installment = (Installment)InstallmentSerializer.Deserialize(rdr);
          installment.AcquirerTransactionKey = transaction.AcquirerTransactionKey;
          installment.InitiatorTransactionKey = transaction.InitiatorTransactionKey;
          installments.Add(installment);
        }
        transaction.Installments = new Installments
        {
          Installment = installments.ToArray()
        };
        transactions.Add(transaction);
      }


      return transactions;

    }
    public Header ExtractHeader(XmlDocument doc)
    {
      //get header xml
      var headerXml = doc.GetElementsByTagName("Header")[0];
      //initialize serializer for header
      var headerSerializer = new XmlSerializer(typeof(Header));
      //read header xml as string
      var reader = new StringReader(headerXml.OuterXml);
      //return deserialized object
      return (Header)headerSerializer.Deserialize(reader);
    }

    public Payments ExtractPayments(XmlDocument doc)
    {
      var paymentsXml = doc.GetElementsByTagName("Conciliation")[0].ChildNodes.Item(5);
      var payments = new List<Payment>();
      var serializer = new XmlSerializer(typeof(Payment));
      foreach (XmlNode item in paymentsXml.ChildNodes)
      {
        var reader = new StringReader(item.OuterXml);
        var payment = (Payment)serializer.Deserialize(reader);
        payments.Add(payment);
      }

      return new Payments
      {
        Payment = payments.ToArray()
      };
    }
  }
}