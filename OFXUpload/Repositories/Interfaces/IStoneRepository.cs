using OFXUpload.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFXUpload.Repositories.Interfaces
{
  public interface IStoneRepository
  {
    Task<Conciliation> VerifyTransaction(StoneRequestData data);
  }
}
