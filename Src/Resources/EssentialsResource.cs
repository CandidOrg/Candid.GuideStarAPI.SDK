using System.Net.Http;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI.Resources
{
  class EssentialsResource : AbstractResource
  {
    public static string GetOrganization(SearchFrom ein)
    {
      var EIN = new EIN(ein);

      var response = Get(BuildPostEssentialsRequest());

      return response;
    }

    public static async Task<string> GetOrganizationAsync(SearchForm form)
    {
      var EIN = new EIN(ein);

      var response = await GetAsync(BuildPostEssentialsRequest());

      return response;
    }

    private static Request BuildPostEssentialsRequest()
    {
      return new Request(
        HttpMethod.Post,
        GuideStarClient.GetSubscriptionKey(),
        Domain.EssentialsV2,
        postParams: null
      );
    }
  }
}
