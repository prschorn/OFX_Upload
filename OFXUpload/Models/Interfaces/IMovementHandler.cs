using OFXUpload.Database;
using OFXUpload.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFXUpload.Models.Interfaces
{
  public interface IMovementHandler
  {
    List<Installment> GetAccountTransactionsByMovement(OFXParser.Entities.Transaction movement, StoneDTO stoneObject);
  }
}
