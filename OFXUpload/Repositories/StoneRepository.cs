using Newtonsoft.Json;
using OFXUpload.Models;
using OFXUpload.Models.DTO;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace OFXUpload.Repositories
{
  public class StoneRepository : IStoneRepository
  {
    private string authorizationToken;
    private string authorizationData;
    private string authorizationEncryptedData;
    public StoneDTO stoneData { get; set; } = new StoneDTO();

    /// <summary>
    /// Call Stone api to gatter payment information and return it.
    /// </summary>
    public async Task<Conciliation> VerifyTransaction(StoneRequestData data)
    {
      var stoneReference = this.stoneData.Conciliation.Where(x => Convert.ToDateTime(x.Header.ReferenceDate) == data.Date).FirstOrDefault();
      if (stoneReference != null)
      {
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
          var jsonBody = JsonConvert.SerializeXmlNode(xmlBody);
          var conciliation = JsonConvert.DeserializeObject<Conciliation>(jsonBody);

          this.stoneData.Conciliation.Add(conciliation);

          return conciliation;
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