using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OFXUpload.Models
{
  public class Criptography
  {

    public static string GenerateHMACSHA512(string data, string key)
    {
      var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
      var b = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

      return ByteToString(b).ToLower();
    }

    public static string GenerateKey()
    {
      var key = new byte[64];

      using (var rng = new RNGCryptoServiceProvider())
      {
        // The array is now filled with cryptographically strong random bytes.
        rng.GetBytes(key);
      }
      return ByteToString(key);
    }

    private static string ByteToString(byte[] buff)
    {
      string sbinary = "";
      for (int i = 0; i < buff.Length; i++)
        sbinary += buff[i].ToString("X2"); /* hex format */
      return sbinary;
    }

  }
}