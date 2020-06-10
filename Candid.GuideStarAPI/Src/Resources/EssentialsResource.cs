using System.Net.Http;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI.Resources
{
  public class EssentialsResource : AbstractResource
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
      return new Request(
        HttpMethod.Post,
        GuideStarClient.GetSubscriptionKey(),
        Domain.EssentialsV2,
        postParams: payload.ToJson()
      );
    }
  }
}
