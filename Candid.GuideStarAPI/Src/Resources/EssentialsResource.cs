using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI.Resources
{
  public class EssentialsResource : AbstractResource
  {
    public static string GetOrganization(SearchPayload form)
    {
      return Get(BuildPostEssentialsRequest(form));
    }

    public static async Task<string> GetOrganizationAsync(SearchPayload form)
    {
      return await GetAsync(BuildPostEssentialsRequest(form))
        .ConfigureAwait(false);
    }

    private static Request BuildPostEssentialsRequest(SearchPayload payload)
    {
      return BuildPostRequest(Domain.EssentialsV2, payload.ToJson());
    }

    public static string GetLookup()
    {
      return Get(BuildGetRequest(Domain.Lookup));
    }

    public static async Task<string> GetLookupAsync()
    {
      return await GetAsync(BuildGetRequest(Domain.Lookup))
        .ConfigureAwait(false);
    }
  }
}
