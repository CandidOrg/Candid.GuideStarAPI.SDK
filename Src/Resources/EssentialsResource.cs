using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace Candid.GuideStarAPI.Resources
{
  class EssentialsResource : AbstractResource
  {
    public static string GetOrganization(SearchPayload form)
    {
      var response = Get(BuildPostEssentialsRequest(form));

      return response;
    }

    public static async Task<string> GetOrganizationAsync(SearchPayload form)
    {
      var response = await GetAsync(BuildPostEssentialsRequest(form));

      return response;
    }

    private static Request BuildPostEssentialsRequest(SearchPayload payload)
    {
      var dict = JsonSerializer
        .Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(payload));

      return new Request(
        HttpMethod.Post,
        GuideStarClient.GetSubscriptionKey(),
        Domain.EssentialsV2,
        postParams: dict
      );
    }
  }
}
