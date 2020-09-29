using System.Threading.Tasks;

namespace Candid.GuideStarAPI.Resources
{
  public class PremierResource : AbstractResource
  {
    public static string GetOrganization(string ein)
    {
      var EIN = new EIN(ein);

      return Get(BuildGetRequest(EIN, Domain.PremierV3));
    }

    public static async Task<string> GetOrganizationAsync(string ein)
    {
      var EIN = new EIN(ein);

      return await GetAsync(BuildGetRequest(EIN, Domain.PremierV3))
        .ConfigureAwait(false);
    }
  }
}
