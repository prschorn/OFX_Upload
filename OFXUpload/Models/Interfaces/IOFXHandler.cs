using OFXParser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OFXUpload.Models.Interfaces
{
  public interface IOFXHandler
  {
    Extract ExtractFile(HttpPostedFileBase file);
  }
}
