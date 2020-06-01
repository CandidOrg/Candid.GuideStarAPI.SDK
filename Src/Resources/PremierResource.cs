using System.Net.Http;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI.Resources
{
  public class PremierResource : AbstractResource
  {
    public static string GetOrganization(string ein)
    {
      var EIN = new EIN(ein);

      var response = Get(BuildGetRequest(EIN, Domain.PremierV3));

      return response;
    }

    public static async Task<string> GetOrganizationAsync(string ein)
    {
      var EIN = new EIN(ein);

      var response = await GetAsync(BuildGetRequest(EIN, Domain.PremierV3));

      return response;
    }
  }
}
