using OFXParser;
using OFXParser.Entities;
using OFXUpload.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OFXUpload.Models
{
  public class OFXHandler : IOFXHandler
  {
    public Extract ExtractFile(HttpPostedFileBase file)
    {
      var filename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.{file.FileName.Split('.')[1]}");
      file.SaveAs(filename);
      var extractedFile = Parser.GetExtract(filename, new ParserSettings());
      File.Delete(filename);

      return extractedFile;
    }

  }
}