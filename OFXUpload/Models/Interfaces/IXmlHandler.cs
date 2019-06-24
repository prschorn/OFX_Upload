using OFXUpload.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OFXUpload.Models.Interfaces
{
  public interface IXmlHandler
  {
    List<Transaction> ExtractFinancialTransactions(XmlDocument doc);
    Header ExtractHeader(XmlDocument doc);

    Payments ExtractPayments(XmlDocument doc);

    Financialtransactionsaccounts ExtractFinancialAccount(XmlDocument doc);
  }
}
