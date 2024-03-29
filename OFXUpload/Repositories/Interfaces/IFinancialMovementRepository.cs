﻿using OFXUpload.Database;
using OFXUpload.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFXUpload.Repositories.Interfaces
{
  public interface IFinancialMovementRepository
  {
    Task<IEnumerable<FinancialAccountMovement>> GetAllMovements();
  }
}
