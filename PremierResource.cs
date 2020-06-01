using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI
{
  public class PremierResource
  {
    public static string GetOrganization(string ein, RestClient client = null)
    {
      var EIN = new EIN(ein);

      client = client ?? GuideStarClient.GetRestClient();
      var response = client.Request(BuildGetOrganizationRequest(EIN));

      return response.Content;
    }

    public static async Task<string> GetOrganizationAsync(string ein, RestClient client = null)
    {
      var EIN = new EIN(ein);

      client = client ?? GuideStarClient.GetRestClient();
      var response = await client.RequestAsync(BuildGetOrganizationRequest(EIN));

      return response.Content;
    }

    private static Request BuildGetOrganizationRequest(EIN ein)
    {


      return new Request(
                  HttpMethod.Get,
                  GuideStarClient.GetSubscriptionKey(),
                  Domain.PremierV3,
                  ein.EinString
              );
    }
  }

  internal class EIN
  {
    public readonly string EinString;
    private int EinLength = 10;
    public EIN(string ein)
    {
      if (ein?.Length != EinLength)
      {
        throw new Exception($"Entered SubscriptionKey of incorrect length. Keys should be {EinLength} characters long");
      }

      EinString = ein;
    }
  }
}
