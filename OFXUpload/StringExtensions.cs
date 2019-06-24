using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OFXUpload
{
  public static class StringExtensions
  {
    public static DateTime ConvertToDateTime(this string data)
    {
      if (data.Length != 8)
        throw new Exception("Lenght of date incorrect, impossible to convert");
      //20190404
      var year = data.Substring(0, 4);
      var month = data.Substring(4, 2);
      var day = data.Substring(6, 2);

      return new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
    }
  }
}