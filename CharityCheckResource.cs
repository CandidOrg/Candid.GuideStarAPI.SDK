using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI
{
  public class CharityCheckResource
  {
    public static string GetCharityCheck(string ein)
    {
      var EIN = new EIN(ein);

      var client = GuideStarClient.GetRestClient();
      var response = client.Request(BuildGetCharityCheckRequest(EIN));

      return response.Content;
    }

    public static async Task<string> GetOrganizationAsync(string ein)
    {
      var EIN = new EIN(ein);

      var client = GuideStarClient.GetRestClient();
      var response = await client.RequestAsync(BuildGetCharityCheckRequest(EIN));

      return response.Content;
    }

    private static Request BuildGetCharityCheckRequest(EIN ein)
    {


      return new Request(
                  HttpMethod.Get,
                  GuideStarClient.GetSubscriptionKey(),
                  Domain.CharityCheckV1,
                  ein.EinString
              );
    }
  }
}
