using OFXParser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFXUpload.Models.Interfaces
{
  public interface IFinancialMovements
  {
    Task<string> SaveOFXInformation(Extract extractedFile);
  }
}
