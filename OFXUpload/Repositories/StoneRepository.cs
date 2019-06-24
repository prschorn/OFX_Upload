using Newtonsoft.Json;
using OFXUpload.Models;
using OFXUpload.Models.DTO;
using OFXUpload.Models.Interfaces;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OFXUpload.Repositories
{
  public class StoneRepository : IStoneRepository
  {
    private string authorizationToken;
    private string authorizationData;
    private string authorizationEncryptedData;
    private List<StoneDTO> stonePayments { get; set; } = new List<StoneDTO>();
    private readonly IXmlHandler xmlHandler;

    public StoneRepository(IXmlHandler xmlHandler)
    {
      this.xmlHandler = xmlHandler;
    }

    /// <summary>
    /// Call Stone api to gatter payment information and return it.
    /// </summary>
    public async Task<StoneDTO> GetTransaction(StoneRequestData data)
    {
      var stoneData = new StoneDTO();
      if(this.stonePayments != null)
      {
        var stoneReference = this.stonePayments.Where(x => x.Header.ReferenceDate == data.Date.ToString("yyyyMMdd")).FirstOrDefault();
        if (stoneReference != null)
          return stoneReference;
      }
      var tokenInfo = this.GetToken();

      using (var client = new HttpClient())
      {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{ConfigurationManager.AppSettings.Get("STONE_HOST")}{data.Date.ToString("yyyyMMdd")}?affiliationCode={ConfigurationManager.AppSettings.Get("AFILIATION_CODE")}");

        //add headers
        request.Headers.Add("Authorization", tokenInfo["authorization"]);
        request.Headers.Add("X-Authorization-Raw-Data", tokenInfo["x-authorization-raw-data"]);
        request.Headers.Add("X-Authorization-Encrypted-Data", tokenInfo["x-authorization-encrypted-data"]);

        var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {

          var xmlBody = new XmlDocument();
          xmlBody.LoadXml(body);
          stoneData.FinancialTransactions.Transaction = this.xmlHandler.ExtractFinancialTransactions(xmlBody).ToArray();
          stoneData.FinancialTransactionsAccounts = this.xmlHandler.ExtractFinancialAccount(xmlBody);
          stoneData.Header = this.xmlHandler.ExtractHeader(xmlBody);
          stoneData.Payments = this.xmlHandler.ExtractPayments(xmlBody);

          this.stonePayments.Add(stoneData);

          return stoneData;
        }

        throw new Exception($"erro ao consultar api Stone: {response.StatusCode}: {response.ReasonPhrase}");
      }
    }
    private Dictionary<string, string> GetToken()
    {

      this.authorizationToken = $"Bearer {ConfigurationManager.AppSettings.Get("CLIENT_APPLICATION_KEY")}";
      this.authorizationData = Criptography.GenerateKey();
      this.authorizationEncryptedData = Criptography.GenerateHMACSHA512(this.authorizationData, ConfigurationManager.AppSettings.Get("CLIENT_APPLICATION_SECRET_KEY"));

      return new Dictionary<string, string>()
        {
          {"authorization",this.authorizationToken },
          {"x-authorization-raw-data",this.authorizationData },
          {"x-authorization-encrypted-data",this.authorizationEncryptedData },
        };
    }
  }
}